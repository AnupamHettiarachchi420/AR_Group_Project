using UnityEngine;

public enum WasteType { CropResidue, Manure, Plastic }

public class WasteItem : MonoBehaviour
{
    public WasteType type;

    public static int Count { get; private set; }

    void OnEnable() { Count++; }
    void OnDisable() { Count--; }
}