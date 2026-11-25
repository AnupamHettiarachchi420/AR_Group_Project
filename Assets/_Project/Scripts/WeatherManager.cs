using System;
using UnityEngine;

public enum Weather { Sunny, Cloudy, Rainy }

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance { get; private set; }
    public event Action<Weather> Changed;

    [SerializeField] Light sun;
    [SerializeField, Range(0f, 1f)] float rainChance = 0.25f;
    [SerializeField, Range(0f, 1f)] float cloudyChance = 0.3f;
    [SerializeField] float minSeconds = 25f;
    [SerializeField] float maxSeconds = 45f;

    public Weather Current { get; private set; } = Weather.Sunny;

    public float SolarMultiplier
    {
        get
        {
            switch (Current)
            {
                case Weather.Cloudy: return 0.4f;
                case Weather.Rainy: return 0.2f;
                default: return 1f;
            }
        }
    }

    float timer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Apply(Weather.Sunny);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) PickNext();
    }

    void PickNext()
    {
        float roll = UnityEngine.Random.value;
        Weather next = Weather.Sunny;
        if (roll < rainChance) next = Weather.Rainy;
        else if (roll < rainChance + cloudyChance) next = Weather.Cloudy;
        Apply(next);
    }

    void Apply(Weather w)
    {
        Current = w;
        timer = UnityEngine.Random.Range(minSeconds, maxSeconds);
        if (sun != null)
            sun.intensity = w == Weather.Sunny ? 1f : (w == Weather.Cloudy ? 0.6f : 0.4f);
        Changed?.Invoke(w);
    }
}