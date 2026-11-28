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
    [SerializeField] int levelNumber = 1;
    [SerializeField] string nextSceneName;

    [Header("UI")]
    [SerializeField] TMP_Text goalLabel;
    [SerializeField] GameObject endPanel;
    [SerializeField] TMP_Text endTitle;
    [SerializeField] TMP_Text endDetails;
    [SerializeField] GameObject nextButton;

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
        string cropsColor = rm.CropsHarvested >= cropsGoal ? "#8CE99A" : "#FFFFFF";
        string wasteColor = rm.WasteDiverted >= wasteGoal ? "#8CE99A" : "#FFFFFF";
        string looseColor = WasteItem.Count > maxLooseWaste * 0.7f ? "#FF8787" : "#FFFFFF";

        goalLabel.text =
              "<b>TIME  " + minutes + ":" + seconds.ToString("00") + "</b>"
            + "\n<color=" + cropsColor + ">Crops  " + rm.CropsHarvested + " / " + cropsGoal + "</color>"
            + "\n<color=" + wasteColor + ">Waste diverted  " + rm.WasteDiverted + " / " + wasteGoal + "</color>"
            + "\n<color=#FFD43B>Energy made  " + rm.EnergyProduced.ToString("0") + "</color>"
            + "\n<color=" + looseColor + ">Loose waste  " + WasteItem.Count + " / " + maxLooseWaste + "</color>";

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
        if (nextButton != null) nextButton.SetActive(won && !string.IsNullOrEmpty(nextSceneName));
        endTitle.text = won ? "Valley restored!" : "Try again";
        endTitle.color = won ? new Color(0.55f, 0.91f, 0.6f) : new Color(1f, 0.53f, 0.53f);

        string extra = "";
        if (won)
        {
            int score = rm.CropsHarvested * 100 + rm.WasteDiverted * 50
                      + Mathf.RoundToInt(rm.EnergyProduced)
                      + Mathf.RoundToInt(Mathf.Max(0f, timeLeft)) * 5;
            float fraction = timeLeft / timeLimit;
            int stars = fraction > 0.6f ? 3 : (fraction > 0.3f ? 2 : 1);
            bool newBest = SaveSystem.RecordResult(levelNumber, score, stars);
            extra = "\nScore: " + score + "   Stars: " + stars + " / 3" + (newBest ? "   NEW BEST!" : "");
        }

        endDetails.text = reason
            + "\nCrops harvested: " + rm.CropsHarvested
            + "\nWaste diverted: " + rm.WasteDiverted
            + "\nEnergy produced: " + rm.EnergyProduced.ToString("0")
            + extra;

        AudioManager.Play(won ? Sfx.Success : Sfx.Fail);
        Time.timeScale = 0f;
    }

    public bool IsFinished => finished;

    public void Retry()
    {
        SceneLoader.Load(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        SceneLoader.Load(nextSceneName);
    }

    public void GoToMenu()
    {
        SceneLoader.Load("MainMenu");
    }
}