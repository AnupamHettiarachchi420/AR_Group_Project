using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] MainMenu mainMenu;

    void Start()
    {
        panel.SetActive(false);
    }

    public void Open()
    {
        panel.SetActive(true);
        musicSlider.SetValueWithoutNotify(SaveSystem.Data.musicVolume);
        sfxSlider.SetValueWithoutNotify(SaveSystem.Data.sfxVolume);
    }

    public void Close()
    {
        SaveSystem.Save();
        panel.SetActive(false);
    }

    public void OnMusicChanged(float value)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SetMusicVolume(value);
    }

    public void OnSfxChanged(float value)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SetSfxVolume(value);
    }

    public void ResetProgress()
    {
        SaveSystem.ResetProgress();
        if (mainMenu != null) mainMenu.RefreshBest();
    }
}