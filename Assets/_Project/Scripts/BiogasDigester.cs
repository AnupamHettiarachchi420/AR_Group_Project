using System.Resources;
using UnityEngine;

public class BiogasDigester : MonoBehaviour, IWasteReceiver
{
    [SerializeField] float energyPerItem = 20f;
    [SerializeField] float secondsToDigest = 10f;

    float pendingEnergy;

    public bool TryDeposit(WasteItem item)
    {
        if (item.type != WasteType.Manure)
        {
            Debug.Log("The digester only takes manure.");
            return false;
        }
        pendingEnergy += energyPerItem;
        ResourceManager.Instance.RecordWasteDiverted();
        AudioManager.Play(Sfx.Deliver);
        Destroy(item.gameObject);
        return true;
    }

    void Update()
    {
        if (pendingEnergy <= 0f) return;
        float release = Mathf.Min(pendingEnergy, energyPerItem / secondsToDigest * Time.deltaTime);
        pendingEnergy -= release;
        ResourceManager.Instance.AddEnergy(release);
    }
}