using System.Resources;
using TMPro;
using UnityEngine;

public class FertilityUI : MonoBehaviour
{
    [SerializeField] TMP_Text label;

    void Start()
    {
        ResourceManager.Instance.FertilityChanged += Refresh;
        Refresh(ResourceManager.Instance.Fertility);
    }

    void OnDestroy()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.FertilityChanged -= Refresh;
    }

    void Refresh(float value) { label.text = "Soil fertility: " + value.ToString("0"); }
}