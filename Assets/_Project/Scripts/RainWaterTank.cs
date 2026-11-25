using System.Resources;
using UnityEngine;

public class RainwaterTank : MonoBehaviour
{
    [SerializeField] float waterPerSecondInRain = 2f;

    void Update()
    {
        if (WeatherManager.Instance == null) return;
        if (WeatherManager.Instance.Current != Weather.Rainy) return;
        ResourceManager.Instance.AddWater(waterPerSecondInRain * Time.deltaTime);
    }
}