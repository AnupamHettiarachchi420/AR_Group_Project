using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    public event Action ResourcesChanged;

    [SerializeField] float maxWater = 100f;
    [SerializeField] float maxEnergy = 100f;
    [SerializeField] float startWater = 50f;

    public float Fertility { get; private set; }
    public float Water { get; private set; }
    public float Energy { get; private set; }
    public float MaxWater => maxWater;
    public float MaxEnergy => maxEnergy;

    public int WasteDiverted { get; private set; }
    public int CropsHarvested { get; private set; }
    public float EnergyProduced { get; private set; }

    void Awake()
    {
        Instance = this;
        Water = startWater;
    }

    public void AddFertility(float amount)
    {
        Fertility = Mathf.Clamp(Fertility + amount, 0f, 100f);
        ResourcesChanged?.Invoke();
    }

    public void AddWater(float amount)
    {
        Water = Mathf.Clamp(Water + amount, 0f, maxWater);
        ResourcesChanged?.Invoke();
    }

    public void AddEnergy(float amount)
    {
        if (amount > 0f) EnergyProduced += amount;
        Energy = Mathf.Clamp(Energy + amount, 0f, maxEnergy);
        ResourcesChanged?.Invoke();
    }

    public bool TrySpendWater(float amount)
    {
        if (Water < amount) return false;
        Water -= amount;
        ResourcesChanged?.Invoke();
        return true;
    }

    public bool TrySpendEnergy(float amount)
    {
        if (Energy < amount) return false;
        Energy -= amount;
        ResourcesChanged?.Invoke();
        return true;
    }

    public void RecordWasteDiverted()
    {
        WasteDiverted++;
        ResourcesChanged?.Invoke();
    }

    public void RecordHarvest()
    {
        CropsHarvested++;
        ResourcesChanged?.Invoke();
    }
}