using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinaryDataStream : MonoBehaviour
{
    private static string GetPath(string fileName)
    {
        string folder = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(folder);

        string fullPath = folder + fileName + ".dat";
        return fullPath;
    }

    // ================= SAVE =================
    public static void Save<T>(T serializedObject, string fileName)
    {
        string fullPath = GetPath(fileName);

        Debug.Log("SAVE PATH: " + fullPath);

        BinaryFormatter formatter = new BinaryFormatter();

        try
        {
            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
            {
                formatter.Serialize(fileStream, serializedObject);
            }

            Debug.Log("Save SUCCESS");
        }
        catch (SerializationException e)
        {
            Debug.LogError("Save FAILED: " + e.Message);
        }
    }

    // ================= CHECK EXIST =================
    public static bool Exist(string fileName)
    {
        string fullPath = GetPath(fileName);
        return File.Exists(fullPath);
    }

    // ================= LOAD =================
    public static T Read<T>(string fileName)
    {
        string fullPath = GetPath(fileName);

        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("File NOT FOUND: " + fullPath);
            return default(T);
        }

        Debug.Log("LOAD PATH: " + fullPath);

        BinaryFormatter formatter = new BinaryFormatter();

        try
        {
            using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
            {
                T data = (T)formatter.Deserialize(fileStream);
                Debug.Log("Load SUCCESS");
                return data;
            }
        }
        catch (SerializationException e)
        {
            Debug.LogError("Load FAILED: " + e.Message);
            return default(T);
        }
    }

    // ================= DELETE =================
    public static void Delete(string fileName)
    {
        string fullPath = GetPath(fileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            Debug.Log("Deleted file: " + fullPath);
        }
        else
        {
            Debug.LogWarning("Delete FAILED - file not found");
        }
    }

    // ================= OPEN FOLDER =================
    public static void OpenSaveFolder()
    {
        string folder = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(folder);

        Debug.Log("OPEN FOLDER: " + folder);
        Application.OpenURL("file://" + folder);
    }
}