using System.Resources;
using UnityEngine;

public class WaterPump : MonoBehaviour
{
    [SerializeField] float interval = 1f;
    [SerializeField] float energyCost = 2f;
    [SerializeField] float waterProduced = 3f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < interval) return;
        timer = 0f;

        var rm = ResourceManager.Instance;
        if (rm.Water >= rm.MaxWater) return;
        if (rm.TrySpendEnergy(energyCost)) rm.AddWater(waterProduced);
    }
}