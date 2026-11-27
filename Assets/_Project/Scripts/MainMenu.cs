using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string firstLevelScene = "Level1_Tutorial";
    [SerializeField] TMP_Text bestText;

    void Start()
    {
        RefreshBest();
    }

    public void RefreshBest()
    {
        if (bestText == null) return;
        var d = SaveSystem.Data;
        bestText.text = d.bestScore[0] > 0
            ? "Best score (Level 1): " + d.bestScore[0] + "   Stars: " + d.bestStars[0] + " / 3"
            : "No scores yet. Restore the valley!";
    }

    public void Play()
    {
        SceneLoader.Load(firstLevelScene);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}