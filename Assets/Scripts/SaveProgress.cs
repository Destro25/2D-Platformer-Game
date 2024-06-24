using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveProgress
{
    public static void SavePlayer1 (PlayerMovement Player, SpawnPoint Spawn)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerlvl1.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveSystem data = new SaveSystem(Player, Spawn);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveSystem LoadData1()
    {
        string path = Application.persistentDataPath + "/playerlvl1.sav";
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

    public static void DeleteSaveFile1()
    {
        string path = Application.persistentDataPath + "/playerlvl1.sav";
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

    public static void SavePlayer2(PlayerMovement Player, SpawnPoint Spawn)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerlvl2.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveSystem data = new SaveSystem(Player, Spawn);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveSystem LoadData2()
    {
        string path = Application.persistentDataPath + "/playerlvl2.sav";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveSystem data = binaryFormatter.Deserialize(stream) as SaveSystem;

            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Save file not found at " + path);
            return null;
        }
    }

    public static void DeleteSaveFile2()
    {
        string path = Application.persistentDataPath + "/playerlvl2.sav";
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

    public static void SavePlayer3(PlayerMovement Player, SpawnPoint Spawn)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerlvl3.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveSystem data = new SaveSystem(Player, Spawn);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveSystem LoadData3()
    {
        string path = Application.persistentDataPath + "/playerlvl3.sav";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveSystem data = binaryFormatter.Deserialize(stream) as SaveSystem;

            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Save file not found at " + path);
            return null;
        }
    }

    public static void DeleteSaveFile3()
    {
        string path = Application.persistentDataPath + "/playerlvl3.sav";
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
