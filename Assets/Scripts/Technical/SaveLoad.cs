using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public static class SaveLoad
{

    public static void SaveFile<T>(string filePath, T saveObj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + filePath, FileMode.OpenOrCreate);
        bf.Serialize(file, saveObj);
        file.Close();
    }

    public static T LoadFile<T>(string filePath)
    {
        if (File.Exists(Application.persistentDataPath + filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filePath, FileMode.Open);
            T data = default(T);
            object temp = bf.Deserialize(file);
            if (temp.GetType() == typeof(T))
                data = (T)temp;
            file.Close();
            return data;
        }
        return default(T);
    }
}
