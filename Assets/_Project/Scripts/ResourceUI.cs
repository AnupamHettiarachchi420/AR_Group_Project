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
        string weather = "";
        if (WeatherManager.Instance != null)
            weather = "\n<color=#A5D8FF>Weather  " + WeatherManager.Instance.Current + "</color>";

        label.text =
              "<color=#8CE99A><b>Fertility</b>  " + rm.Fertility.ToString("0") + "</color>"
            + "\n<color=#74C0FC><b>Water</b>  " + rm.Water.ToString("0") + " / " + rm.MaxWater.ToString("0") + "</color>"
            + "\n<color=#FFD43B><b>Energy</b>  " + rm.Energy.ToString("0") + " / " + rm.MaxEnergy.ToString("0") + "</color>"
            + weather;
    }
}