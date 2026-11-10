using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    public event Action<float> FertilityChanged;
    public float Fertility { get; private set; }

    void Awake() { Instance = this; }

    public void AddFertility(float amount)
    {
        Fertility = Mathf.Clamp(Fertility + amount, 0f, 100f);
        FertilityChanged?.Invoke(Fertility);
    }
}