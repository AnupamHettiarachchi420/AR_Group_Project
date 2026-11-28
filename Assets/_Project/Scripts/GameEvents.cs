using System;

public static class GameEvents
{
    public static event Action<WasteItem> ItemPickedUp;
    public static event Action<WasteType> WasteDelivered;
    public static event Action CropPlanted;
    public static event Action CropWatered;
    public static event Action CropHarvested;

    public static void RaiseItemPickedUp(WasteItem i) { ItemPickedUp?.Invoke(i); }
    public static void RaiseWasteDelivered(WasteType t) { WasteDelivered?.Invoke(t); }
    public static void RaiseCropPlanted() { CropPlanted?.Invoke(); }
    public static void RaiseCropWatered() { CropWatered?.Invoke(); }
    public static void RaiseCropHarvested() { CropHarvested?.Invoke(); }
}