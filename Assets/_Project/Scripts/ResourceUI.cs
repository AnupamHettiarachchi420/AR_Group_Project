using System.Resources;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] TMP_Text label;

    void Start()
    {
        ResourceManager.Instance.ResourcesChanged += Refresh;
        Refresh();
    }

    void OnDestroy()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.ResourcesChanged -= Refresh;
    }

    void Refresh()
    {
        var rm = ResourceManager.Instance;
        label.text = "Fertility: " + rm.Fertility.ToString("0")
                   + "\nWater: " + rm.Water.ToString("0")
                   + "\nEnergy: " + rm.Energy.ToString("0");
    }
}