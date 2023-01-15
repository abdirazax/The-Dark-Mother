using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;


public class SaveLoadSystem :MonoBehaviour
{
    public string savePath => $"{Application.persistentDataPath}/save.sav";

    [ContextMenu("Save")]
    void Save()
    {
        Dictionary<string, object> state = LoadFile();
        SaveState(state);
        SaveFile(state);
    }

    [ContextMenu("Load")]
    void Load()
    {
        Dictionary<string, object> state = LoadFile();
        LoadState(state);
    }

    
    void SaveFile(object state)
    {
        using (FileStream stream = File.Open(savePath, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }
    Dictionary<string,object> LoadFile()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("Save file not found in " + savePath);
            return new Dictionary<string, object>();
        }
        using (FileStream stream = File.Open(savePath,FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    void SaveState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.Id] = saveable.SaveState();
        }
    }
    void LoadState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            if (state.TryGetValue(saveable.Id, out object saveState)) 
            {
                saveable.LoadState(saveState);
            }
        }
    }

}
