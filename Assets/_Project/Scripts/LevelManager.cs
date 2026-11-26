using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Goals")]
    [SerializeField] float timeLimit = 240f;
    [SerializeField] int cropsGoal = 6;
    [SerializeField] int wasteGoal = 10;
    [SerializeField] int maxLooseWaste = 15;

    [Header("UI")]
    [SerializeField] TMP_Text goalLabel;
    [SerializeField] GameObject endPanel;
    [SerializeField] TMP_Text endTitle;
    [SerializeField] TMP_Text endDetails;

    float timeLeft;
    bool finished;

    void Start()
    {
        Time.timeScale = 1f;
        timeLeft = timeLimit;
        endPanel.SetActive(false);
    }

    void Update()
    {
        if (finished) return;

        timeLeft -= Time.deltaTime;
        var rm = ResourceManager.Instance;

        int minutes = Mathf.Max(0, (int)(timeLeft / 60f));
        int seconds = Mathf.Max(0, (int)(timeLeft % 60f));
        goalLabel.text = "Time: " + minutes + ":" + seconds.ToString("00")
            + "\nCrops harvested: " + rm.CropsHarvested + " / " + cropsGoal
            + "\nWaste diverted: " + rm.WasteDiverted + " / " + wasteGoal
            + "\nEnergy produced: " + rm.EnergyProduced.ToString("0")
            + "\nLoose waste: " + WasteItem.Count + " / " + maxLooseWaste;

        if (rm.CropsHarvested >= cropsGoal && rm.WasteDiverted >= wasteGoal)
            End(true, "Every waste stream became a resource.");
        else if (WasteItem.Count > maxLooseWaste)
            End(false, "Waste piled up and polluted the valley.");
        else if (timeLeft <= 0f)
            End(false, "Time ran out before the loops closed.");
    }

    void End(bool won, string reason)
    {
        finished = true;
        var rm = ResourceManager.Instance;
        endPanel.SetActive(true);
        endTitle.text = won ? "Valley restored!" : "Try again";
        endDetails.text = reason
            + "\nCrops harvested: " + rm.CropsHarvested
            + "\nWaste diverted: " + rm.WasteDiverted
            + "\nEnergy produced: " + rm.EnergyProduced.ToString("0");
        Time.timeScale = 0f;
    }

    public bool IsFinished => finished;

    public void Retry()
    {
        SceneLoader.Load(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        SceneLoader.Load("MainMenu");
    }
}