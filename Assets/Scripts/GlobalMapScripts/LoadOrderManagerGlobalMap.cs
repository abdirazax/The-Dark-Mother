using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SaveLoadSystem))]
public class LoadOrderManagerGlobalMap : MonoBehaviour
{
    SaveLoadSystem saveLoadSystem;
    private void Awake()
    {
        saveLoadSystem = GetComponent<SaveLoadSystem>();
    }
    private void OnEnable()
    {
        LevelManager.OnSceneLoadedWithSaveFilePathIndication += LoadThisScene;
    }
    private void OnDisable()
    {
        LevelManager.OnSceneLoadedWithSaveFilePathIndication -= LoadThisScene;
    }
    void LoadThisScene(string chosenSaveFilePath)
    {
        saveLoadSystem.LoadFromPath(chosenSaveFilePath);
    }
}

