using UnityEngine;

public enum Sfx { Pickup, Plant, Water, Harvest, Deliver, Success, Fail }

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] AudioClip music;

    [Header("Sound effects")]
    [SerializeField] AudioClip pickup;
    [SerializeField] AudioClip plant;
    [SerializeField] AudioClip water;
    [SerializeField] AudioClip harvest;
    [SerializeField] AudioClip deliver;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip fail;

    AudioSource musicSource;
    AudioSource sfxSource;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Boot()
    {
        if (Instance != null) return;
        var prefab = Resources.Load<AudioManager>("AudioManager");
        if (prefab != null) Instantiate(prefab);
        else Debug.LogWarning("AudioManager prefab not found in a Resources folder.");
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        SetMusicVolume(SaveSystem.Data.musicVolume);
        SetSfxVolume(SaveSystem.Data.sfxVolume);

        if (music != null)
        {
            musicSource.clip = music;
            musicSource.Play();
        }
    }

    public void SetMusicVolume(float v)
    {
        v = Mathf.Clamp01(v);
        musicSource.volume = v;
        SaveSystem.Data.musicVolume = v;
    }

    public void SetSfxVolume(float v)
    {
        v = Mathf.Clamp01(v);
        sfxSource.volume = v;
        SaveSystem.Data.sfxVolume = v;
    }

    public static void Play(Sfx id)
    {
        if (Instance == null) return;
        AudioClip clip = Instance.GetClip(id);
        if (clip != null) Instance.sfxSource.PlayOneShot(clip);
    }

    AudioClip GetClip(Sfx id)
    {
        switch (id)
        {
            case Sfx.Pickup: return pickup;
            case Sfx.Plant: return plant;
            case Sfx.Water: return water;
            case Sfx.Harvest: return harvest;
            case Sfx.Deliver: return deliver;
            case Sfx.Success: return success;
            case Sfx.Fail: return fail;
            default: return null;
        }
    }
}