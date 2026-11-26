using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] LevelManager level;

    bool paused;

    void Start()
    {
        pausePanel.SetActive(false);
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;
        if (level != null && level.IsFinished) return;

        if (kb.escapeKey.wasPressedThisFrame || kb.pKey.wasPressedThisFrame)
            SetPaused(!paused);
    }

    public void SetPaused(bool value)
    {
        paused = value;
        pausePanel.SetActive(value);
        Time.timeScale = value ? 0f : 1f;
    }

    public void Resume() { SetPaused(false); }

    public void Restart()
    {
        SceneLoader.Load(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenu()
    {
        SceneLoader.Load("MainMenu");
    }
}