using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Tool { Seeds, WateringCan, Hands }

public class ToolController : MonoBehaviour
{
    [SerializeField] float range = 2.5f;
    [SerializeField] TMP_Text toolLabel;

    Tool current = Tool.Seeds;

    void Start()
    {
        RefreshLabel();
    }

    void Update()
    {
        var kb = Keyboard.current;
        var mouse = Mouse.current;
        if (kb == null || mouse == null) return;

        if (kb.digit1Key.wasPressedThisFrame) SetTool(Tool.Seeds);
        if (kb.digit2Key.wasPressedThisFrame) SetTool(Tool.WateringCan);
        if (kb.digit3Key.wasPressedThisFrame) SetTool(Tool.Hands);

        if (mouse.leftButton.wasPressedThisFrame) UseTool();
    }

    void SetTool(Tool t)
    {
        current = t;
        RefreshLabel();
    }

    void UseTool()
    {
        CropPlot plot = FindNearestPlot();
        if (plot == null) return;

        switch (current)
        {
            case Tool.Seeds: plot.TryPlant(); break;
            case Tool.WateringCan: plot.TryWater(); break;
            case Tool.Hands: plot.TryHarvest(); break;
        }
    }

    CropPlot FindNearestPlot()
    {
        CropPlot best = null;
        float bestDist = float.MaxValue;

        foreach (var hit in Physics.OverlapSphere(transform.position, range))
        {
            var plot = hit.GetComponentInParent<CropPlot>();
            if (plot == null) continue;

            float d = (plot.transform.position - transform.position).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
                best = plot;
            }
        }
        return best;
    }

    void RefreshLabel()
    {
        if (toolLabel == null) return;

        toolLabel.text = "<color=#8CE99A><b>TOOL</b></color>      "
                       + ToolItem(Tool.Seeds, "1", "Seeds") + "      "
                       + ToolItem(Tool.WateringCan, "2", "Watering can") + "      "
                       + ToolItem(Tool.Hands, "3", "Harvest");
    }

    string ToolItem(Tool tool, string key, string label)
    {
        if (tool == current)
            return "<color=#FFD43B><b>[" + key + "] " + label + "</b></color>";
        return "<color=#8A9690>[" + key + "] " + label + "</color>";
    }
}