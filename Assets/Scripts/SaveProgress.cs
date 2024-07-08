using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveProgress
{
    private static string GetSaveFilePath(int level)
    {
        return Application.persistentDataPath + "/playerlvl" + level + ".json";
    }

    public static void SavePlayer(PlayerMovement Player, SpawnPoint Spawn, int level)
    {
        string path = GetSaveFilePath(level);
        SaveSystem data = new SaveSystem(Player, Spawn);
        string jsonData = JsonUtility.ToJson(data, true); // true for pretty print
        File.WriteAllText(path, jsonData);
    }

    public static SaveSystem LoadData(int level)
    {
        string path = GetSaveFilePath(level);
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveSystem>(jsonData);
        }
        else
        {
            Debug.Log("Save file not found at " + path);
            return null;
        }
    }

    public static void DeleteSaveFile(int level)
    {
        string path = Application.persistentDataPath + "/playerlvl" + level + ".json";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted.");
        }
        else
        {
            Debug.Log("No save file to delete.");
        }
    }
}
