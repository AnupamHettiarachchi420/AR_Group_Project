using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] float range = 2f;
    [SerializeField] Transform carryPoint;
    WasteItem carried;

    void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.eKey.wasPressedThisFrame) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, range);

        if (carried != null)
        {
            foreach (var h in hits)
            {
                if (h.TryGetComponent(out CompostBin bin) && bin.TryDeposit(carried))
                {
                    carried = null;
                    return;
                }
            }
            return;
        }

        foreach (var h in hits)
        {
            if (h.TryGetComponent(out WasteItem item)) { PickUp(item); return; }
        }
    }

    void PickUp(WasteItem item)
    {
        carried = item;
        item.GetComponent<Collider>().enabled = false;
        item.transform.SetParent(carryPoint);
        item.transform.localPosition = Vector3.zero;
    }
}