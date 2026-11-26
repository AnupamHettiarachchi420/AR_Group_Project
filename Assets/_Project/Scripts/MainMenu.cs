using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string firstLevelScene = "Level1_Tutorial";

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