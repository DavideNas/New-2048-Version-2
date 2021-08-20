using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour{

    //private static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";

    private static string SAVE_FOLDER;

    private void Start() {
        SAVE_FOLDER = Application.persistentDataPath + "/Saves/";    
    }

    public static void Init() {
        // Test if Save Folder exists
        if (!Directory.Exists(SAVE_FOLDER)) {
            // Create Save Foldera
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(string data)
    {
        // create a formatter var to
        BinaryFormatter formatter = new BinaryFormatter();

        string path = SAVE_FOLDER + "save_point.fun";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static string Load()
    {
        string path = SAVE_FOLDER + "save_point.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            string data = formatter.Deserialize(stream) as string;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }

    public static bool DataExists()
    {
        // Check if any data file already exist
        string path = SAVE_FOLDER + "save_point.fun";
        if (File.Exists(path))
            return true;
        else
            return false;
    }
}