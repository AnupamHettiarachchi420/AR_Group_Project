using UnityEngine;
using UnityEngine.AI;

public enum AnimalState { Idle, Wander }

[RequireComponent(typeof(NavMeshAgent))]
public class AnimalWander : MonoBehaviour
{
    [SerializeField] float wanderRadius = 8f;
    [SerializeField] Vector2 idleTimeRange = new Vector2(2f, 5f);
    [SerializeField] float secondsBetweenManure = 20f;
    [SerializeField] GameObject manurePrefab;

    NavMeshAgent agent;
    AnimalState state;
    float stateTimer;
    float manureTimer;
    Vector3 home;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        home = transform.position;
    }

    void Start()
    {
        EnterIdle();
    }

    void Update()
    {
        manureTimer += Time.deltaTime;
        if (manureTimer >= secondsBetweenManure)
        {
            manureTimer = 0f;
            DropManure();
        }

        switch (state)
        {
            case AnimalState.Idle:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f) EnterWander();
                break;

            case AnimalState.Wander:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.2f)
                    EnterIdle();
                break;
        }
    }

    void EnterIdle()
    {
        state = AnimalState.Idle;
        stateTimer = UnityEngine.Random.Range(idleTimeRange.x, idleTimeRange.y);
        if (agent.isOnNavMesh) agent.ResetPath();
    }

    void EnterWander()
    {
        Vector3 target = home + new Vector3(
            UnityEngine.Random.Range(-wanderRadius, wanderRadius), 0f,
            UnityEngine.Random.Range(-wanderRadius, wanderRadius));

        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 3f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            state = AnimalState.Wander;
        }
        else
        {
            EnterIdle();
        }
    }

    void DropManure()
    {
        if (manurePrefab == null) return;
        Vector3 pos = new Vector3(transform.position.x, 0.25f, transform.position.z);
        Instantiate(manurePrefab, pos + transform.forward * -0.8f, Quaternion.identity);
    }
}