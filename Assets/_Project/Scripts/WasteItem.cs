using System.Collections.Generic;
using UnityEngine;

public enum WasteType { CropResidue, Manure, Plastic }

public class WasteItem : MonoBehaviour
{
    public WasteType type;

    // Held: someone is carrying it. Claimed: the robot is heading to it.
    public bool Held { get; set; }
    public bool Claimed { get; set; }

    public static readonly List<WasteItem> All = new List<WasteItem>();
    public static int Count => All.Count;

    void OnEnable() { All.Add(this); }
    void OnDisable() { All.Remove(this); }
}