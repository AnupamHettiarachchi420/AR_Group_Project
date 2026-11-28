using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Button[] levelButtons;
    [SerializeField] TMP_Text[] levelLabels;
    [SerializeField] string[] sceneNames = { "Level1_Tutorial", "Level2_DroughtFarm", "Level3_Village" };
    [SerializeField] string[] titles = { "Level 1: Tutorial Plot", "Level 2: Drought Farm", "Level 3: The Village" };

    void Start()
    {
        panel.SetActive(false);
    }

    public void Open()
    {
        panel.SetActive(true);
        Refresh();
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    public void PlayLevel(int index)
    {
        SceneLoader.Load(sceneNames[index]);
    }

    void Refresh()
    {
        var d = SaveSystem.Data;
        for (int i = 0; i < levelButtons.Length; i++)
        {
            bool unlocked = i < d.levelsUnlocked;
            levelButtons[i].interactable = unlocked;

            string info;
            if (!unlocked) info = "Locked";
            else if (d.bestScore[i] > 0) info = "Stars " + d.bestStars[i] + " / 3     Best " + d.bestScore[i];
            else info = "Not played yet";

            levelLabels[i].text = titles[i] + "\n<size=60%>" + info + "</size>";
        }
    }
}