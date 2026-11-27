using System.Resources;
using UnityEngine;

public enum PlotState { Empty, Growing, Ripe, Withered }

public class CropPlot : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] Transform cropVisual;
    [SerializeField] Renderer soilRenderer;
    [SerializeField] Color dryColor = new Color(0.55f, 0.40f, 0.25f);
    [SerializeField] Color wetColor = new Color(0.25f, 0.15f, 0.08f);
    [SerializeField] Color growingColor = new Color(0.2f, 0.7f, 0.2f);
    [SerializeField] Color ripeColor = new Color(0.95f, 0.75f, 0.15f);
    [SerializeField] Color witheredColor = new Color(0.4f, 0.35f, 0.3f);

    [Header("Growth")]
    [SerializeField] float secondsToGrow = 20f;
    [SerializeField] float secondsWaterLasts = 15f;
    [SerializeField] float witherAfterDrySeconds = 15f;
    [SerializeField] float fertilityCostPerHarvest = 3f;
    [SerializeField] float waterCostPerWatering = 10f;
    [SerializeField] GameObject residuePrefab;

    public PlotState State { get; private set; } = PlotState.Empty;

    Renderer cropRenderer;
    float growth;      // 0 to 1
    float water;       // seconds of water left
    float drySeconds;

    void Awake()
    {
        cropRenderer = cropVisual.GetComponent<Renderer>();
    }

    void Start()
    {
        UpdateVisuals();
    }

    void Update()
    {
        if (State != PlotState.Growing) return;

        if (water > 0f)
        {
            water -= Time.deltaTime;
            drySeconds = 0f;

            float fertility01 = ResourceManager.Instance.Fertility / 100f;
            float speed = Mathf.Lerp(0.5f, 1.5f, fertility01);
            growth += Time.deltaTime / secondsToGrow * speed;

            if (growth >= 1f)
            {
                growth = 1f;
                State = PlotState.Ripe;
            }
        }
        else
        {
            drySeconds += Time.deltaTime;
            if (drySeconds >= witherAfterDrySeconds) State = PlotState.Withered;
        }

        UpdateVisuals();
    }

    public bool TryPlant()
    {
        if (State != PlotState.Empty) return false;
        State = PlotState.Growing;
        growth = 0f;
        water = 0f;
        drySeconds = 0f;
        UpdateVisuals();
        AudioManager.Play(Sfx.Plant);
        return true;
    }

    public bool TryWater()
    {
        if (State != PlotState.Growing) return false;
        if (!ResourceManager.Instance.TrySpendWater(waterCostPerWatering)) return false;
        water = secondsWaterLasts;
        drySeconds = 0f;
        UpdateVisuals();
        AudioManager.Play(Sfx.Water);
        return true;
    }

    public bool TryHarvest()
    {
        if (State != PlotState.Ripe && State != PlotState.Withered) return false;

        if (State == PlotState.Ripe)
        {
            ResourceManager.Instance.AddFertility(-fertilityCostPerHarvest);
            ResourceManager.Instance.RecordHarvest();
        }

        if (residuePrefab != null)
        {
            Vector3 offset = new Vector3(Random.Range(-0.6f, 0.6f), 1f, Random.Range(-0.6f, 0.6f));
            Instantiate(residuePrefab, transform.position + offset, Quaternion.identity);
        }

        State = PlotState.Empty;
        growth = 0f;
        water = 0f;
        UpdateVisuals();
        AudioManager.Play(Sfx.Harvest);
        return true;
    }

    void UpdateVisuals()
    {
        bool hasCrop = State != PlotState.Empty;
        cropVisual.gameObject.SetActive(hasCrop);
        soilRenderer.material.color = water > 0f ? wetColor : dryColor;
        if (!hasCrop) return;

        float h = Mathf.Max(0.3f, growth);
        cropVisual.localScale = new Vector3(0.5f, h, 0.5f);
        cropVisual.localPosition = new Vector3(0f, 0.2f + h * 0.5f, 0f);

        Color c = growingColor;
        if (State == PlotState.Ripe) c = ripeColor;
        else if (State == PlotState.Withered) c = witheredColor;
        cropRenderer.material.color = c;
    }
}