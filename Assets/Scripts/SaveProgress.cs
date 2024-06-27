using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveProgress
{
    public static void SavePlayer (PlayerMovement Player, SpawnPoint Spawn, int level)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerlvl" + level + ".sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveSystem data = new SaveSystem(Player, Spawn);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveSystem LoadData(int level)
    {
        string path = Application.persistentDataPath + "/playerlvl" + level + ".sav";
        if (File.Exists(path)) 
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream (path, FileMode.Open);

            SaveSystem data = binaryFormatter.Deserialize(stream) as SaveSystem;

            stream.Close();
            return data;
        }else
        {
            Debug.Log("Save file not found at " +  path);
            return null;
        }
    }

    public static void DeleteSaveFile(int level)
    {
        string path = Application.persistentDataPath + "/playerlvl" + level + ".sav";
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
