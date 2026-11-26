using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] GameObject loadingPanel;
    [SerializeField] TMP_Text loadingText;
    [SerializeField] float minimumSeconds = 0.8f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        loadingPanel.SetActive(false);
    }

    public static void Load(string sceneName)
    {
        Time.timeScale = 1f;
        if (Instance != null) Instance.StartCoroutine(Instance.LoadRoutine(sceneName));
        else SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadRoutine(string sceneName)
    {
        loadingPanel.SetActive(true);
        loadingText.text = "Loading... 0%";

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (op.progress < 0.9f || timer < minimumSeconds)
        {
            timer += Time.unscaledDeltaTime;
            float shown = Mathf.Clamp01(op.progress / 0.9f);
            loadingText.text = "Loading... " + Mathf.RoundToInt(shown * 100f) + "%";
            yield return null;
        }

        loadingText.text = "Loading... 100%";
        op.allowSceneActivation = true;
        while (!op.isDone) yield return null;

        loadingPanel.SetActive(false);
    }
}