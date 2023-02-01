using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;


public class SaveLoadSystem : MonoBehaviour
{
    public string savePath 
    { 
        get 
        {
            string path = $"{Application.persistentDataPath}/SaveFiles";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return $"{path}/Save.sav";
        } 
    }
    public string saveAsCampaignFilePath
    {
        get
        {
            string path = $"{Application.dataPath}/Campaigns/MainCampaigns";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return $"{path}/MainCampaign.campaign";
        }
    }

    public void SaveToPath(string saveFilePath)
    {
        Dictionary<string, object> state = LoadFile(saveFilePath);
        if (!Directory.Exists(saveFilePath))
        {
            Directory.CreateDirectory(saveFilePath);
        }
        SaveState(state);
        SaveFile(state, savePath);
    }
    public void LoadFromPath(string saveFilePath)
    {
        Dictionary<string, object> state = LoadFile(saveFilePath);
        LoadState(state);
    }

    [ContextMenu("Save As Campaign")]
    void SaveAsCampaign()
    {
        Dictionary<string, object> state = LoadFile(saveAsCampaignFilePath);
        SaveState(state);
        SaveFile(state, saveAsCampaignFilePath);
    }
    [ContextMenu("Load Campaign")]
    void LoadCampaign()
    {
        Dictionary<string, object> state = LoadFile(saveAsCampaignFilePath);
        LoadState(state);
    }


    void SaveFile(object state, string path)
    {
        using (FileStream stream = File.Open(path, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }
    Dictionary<string,object> LoadFile(string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("Save file not found in " + path);
            return new Dictionary<string, object>();
        }
        using (FileStream stream = File.Open(path, FileMode.Open))
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
