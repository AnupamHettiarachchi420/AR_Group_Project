using System.Collections.Generic;
using UnityEngine;

public enum RobotState { Idle, Collect, Deliver, Recharge }

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class HelperRobot : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] Transform carryPoint;
    [SerializeField] Transform chargingPad;

    [Header("Behaviour")]
    [SerializeField] float pickupDistance = 1.2f;
    [SerializeField] float dropDistance = 2f;
    [SerializeField] float searchInterval = 1f;

    [Header("Battery")]
    [SerializeField] float maxBattery = 60f;
    [SerializeField] float lowBattery = 12f;
    [SerializeField] float drainPerSecondMoving = 1f;
    [SerializeField] float rechargePerSecond = 10f;

    [Header("Debug (read only)")]
    public RobotState state = RobotState.Idle;
    public float battery;

    UnityEngine.AI.NavMeshAgent agent;
    WasteItem target;
    WasteItem carried;
    Transform destination;
    IWasteReceiver receiver;
    float searchTimer;

    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        battery = maxBattery;
    }

    void Update()
    {
        switch (state)
        {
            case RobotState.Idle: UpdateIdle(); break;
            case RobotState.Collect: UpdateCollect(); break;
            case RobotState.Deliver: UpdateDeliver(); break;
            case RobotState.Recharge: UpdateRecharge(); break;
        }
        DrainBattery();
    }

    // ---------- States ----------

    void UpdateIdle()
    {
        if (battery <= lowBattery)
        {
            EnterRecharge();
            return;
        }

        searchTimer -= Time.deltaTime;
        if (searchTimer > 0f) return;
        searchTimer = searchInterval;

        target = FindNearestWaste();
        if (target != null)
        {
            target.Claimed = true;
            agent.SetDestination(target.transform.position);
            state = RobotState.Collect;
        }
    }

    void UpdateCollect()
    {
        // Item was destroyed or the player picked it up first.
        if (target == null || target.Held)
        {
            target = null;
            state = RobotState.Idle;
            return;
        }

        if (FlatDistance(transform.position, target.transform.position) > pickupDistance) return;

        carried = target;
        target = null;
        carried.Held = true;
        carried.GetComponent<Collider>().enabled = false;
        carried.transform.SetParent(carryPoint);
        carried.transform.localPosition = Vector3.zero;

        destination = FindReceiverFor(carried.type);
        if (destination == null)
        {
            ReleaseCarried();
            state = RobotState.Idle;
            return;
        }

        receiver = destination.GetComponent<IWasteReceiver>();
        agent.SetDestination(destination.position);
        state = RobotState.Deliver;
    }

    void UpdateDeliver()
    {
        if (carried == null || destination == null)
        {
            ReleaseCarried();
            state = RobotState.Idle;
            return;
        }

        if (FlatDistance(transform.position, destination.position) > dropDistance) return;

        if (receiver != null && receiver.TryDeposit(carried)) carried = null;
        else ReleaseCarried();

        state = RobotState.Idle;
    }

    void UpdateRecharge()
    {
        bool atPad = chargingPad == null ||
                     FlatDistance(transform.position, chargingPad.position) <= 1.5f;
        if (!atPad) return;

        battery += rechargePerSecond * Time.deltaTime;
        if (battery >= maxBattery)
        {
            battery = maxBattery;
            state = RobotState.Idle;
        }
    }

    // ---------- Helpers ----------

    void EnterRecharge()
    {
        ReleaseAll();
        state = RobotState.Recharge;
        if (chargingPad != null) agent.SetDestination(chargingPad.position);
    }

    void DrainBattery()
    {
        if (state == RobotState.Recharge) return;
        if (agent.velocity.sqrMagnitude < 0.05f) return;

        battery = Mathf.Max(0f, battery - drainPerSecondMoving * Time.deltaTime);
        if (battery <= 0f && (state == RobotState.Collect || state == RobotState.Deliver))
            EnterRecharge();
    }

    WasteItem FindNearestWaste()
    {
        WasteItem best = null;
        float bestDist = float.MaxValue;

        foreach (var item in WasteItem.All)
        {
            if (item.Held || item.Claimed) continue;
            if (item.type == WasteType.Plastic) continue; // nothing can process it yet

            float d = FlatDistance(transform.position, item.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = item;
            }
        }
        return best;
    }

    Transform FindReceiverFor(WasteType type)
    {
        if (type == WasteType.CropResidue)
            return Nearest(FindObjectsByType<CompostBin>(FindObjectsSortMode.None));
        if (type == WasteType.Manure)
            return Nearest(FindObjectsByType<BiogasDigester>(FindObjectsSortMode.None));
        return null;
    }

    Transform Nearest<T>(T[] list) where T : Component
    {
        Transform best = null;
        float bestDist = float.MaxValue;
        foreach (var c in list)
        {
            float d = FlatDistance(transform.position, c.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = c.transform;
            }
        }
        return best;
    }

    void ReleaseAll()
    {
        if (target != null)
        {
            target.Claimed = false;
            target = null;
        }
        ReleaseCarried();
    }

    void ReleaseCarried()
    {
        if (carried == null) return;
        carried.transform.SetParent(null);
        carried.transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z)
                                     + transform.forward;
        carried.GetComponent<Collider>().enabled = true;
        carried.Held = false;
        carried.Claimed = false;
        carried = null;
    }

    static float FlatDistance(Vector3 a, Vector3 b)
    {
        a.y = 0f;
        b.y = 0f;
        return Vector3.Distance(a, b);
    }
}