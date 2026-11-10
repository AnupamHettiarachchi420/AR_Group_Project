using UnityEngine;

public class CompostBin : MonoBehaviour
{
    [SerializeField] float fertilityPerItem = 5f;

    public bool TryDeposit(WasteItem item)
    {
        if (item.type == WasteType.Plastic)
        {
            Debug.Log("Contamination! Plastic does not belong in the compost.");
            return false;
        }
        ResourceManager.Instance.AddFertility(fertilityPerItem);
        Destroy(item.gameObject);
        return true;
    }
}