using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float musicVolume = 0.6f;
    public float sfxVolume = 0.8f;
    public int levelsUnlocked = 1;
    public int[] bestScore = new int[3];
    public int[] bestStars = new int[3];
}

public static class SaveSystem
{
    static SaveData data;

    static string FilePath
    {
        get { return Path.Combine(Application.persistentDataPath, "loopvalley_save.json"); }
    }

    public static SaveData Data
    {
        get
        {
            if (data == null) Load();
            return data;
        }
    }

    public static void Load()
    {
        try
        {
            if (File.Exists(FilePath))
                data = JsonUtility.FromJson<SaveData>(File.ReadAllText(FilePath));
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Could not read save file: " + e.Message);
        }

        if (data == null) data = new SaveData();
        if (data.bestScore == null || data.bestScore.Length < 3) data.bestScore = new int[3];
        if (data.bestStars == null || data.bestStars.Length < 3) data.bestStars = new int[3];
    }

    public static void Save()
    {
        try
        {
            File.WriteAllText(FilePath, JsonUtility.ToJson(Data, true));
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Could not write save file: " + e.Message);
        }
    }

    // Returns true if this is a new best score for the level.
    public static bool RecordResult(int level, int score, int stars)
    {
        int i = Mathf.Clamp(level - 1, 0, 2);
        bool newBest = score > Data.bestScore[i];
        if (newBest) Data.bestScore[i] = score;
        if (stars > Data.bestStars[i]) Data.bestStars[i] = stars;
        if (level >= Data.levelsUnlocked && level < 3) Data.levelsUnlocked = level + 1;
        Save();
        return newBest;
    }

    public static void ResetProgress()
    {
        Data.levelsUnlocked = 1;
        Data.bestScore = new int[3];
        Data.bestStars = new int[3];
        Save();
    }
}