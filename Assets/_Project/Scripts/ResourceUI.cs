using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] TMP_Text label;

    void Start()
    {
        ResourceManager.Instance.ResourcesChanged += Refresh;
        if (WeatherManager.Instance != null)
            WeatherManager.Instance.Changed += OnWeatherChanged;
        Refresh();
    }

    void OnDestroy()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.ResourcesChanged -= Refresh;
        if (WeatherManager.Instance != null)
            WeatherManager.Instance.Changed -= OnWeatherChanged;
    }

    void OnWeatherChanged(Weather w)
    {
        Refresh();
    }

    void Refresh()
    {
        var rm = ResourceManager.Instance;
        string weather = WeatherManager.Instance != null
            ? "\nWeather: " + WeatherManager.Instance.Current
            : "";

        label.text = "Fertility: " + rm.Fertility.ToString("0")
                   + "\nWater: " + rm.Water.ToString("0")
                   + "\nEnergy: " + rm.Energy.ToString("0")
                   + weather;
    }
}