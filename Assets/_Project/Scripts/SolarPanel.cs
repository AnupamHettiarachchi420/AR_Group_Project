using System.Resources;
using UnityEngine;

public class SolarPanel : MonoBehaviour
{
    [SerializeField] float energyPerSecond = 1f;

    void Update()
    {
        float multiplier = WeatherManager.Instance != null ? WeatherManager.Instance.SolarMultiplier : 1f;
        ResourceManager.Instance.AddEnergy(energyPerSecond * multiplier * Time.deltaTime);
    }
}