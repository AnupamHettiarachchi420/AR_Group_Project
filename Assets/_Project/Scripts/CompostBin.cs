using UnityEngine;

public class CompostBin : MonoBehaviour, IWasteReceiver
{
    [SerializeField] float fertilityPerItem = 5f;

    public bool TryDeposit(WasteItem item)
    {
        if (item.type != WasteType.CropResidue)
        {
            Debug.Log("The compost bin only takes crop residue.");
            return false;
        }
        ResourceManager.Instance.AddFertility(fertilityPerItem);
        Destroy(item.gameObject);
        return true;
    }
}