using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneButton : MonoBehaviour
{
    public void LoadGlobalMapScene(string saveFilePathToLoadInGlobalMapScene)
    {
        LevelManager.Instance.LoadScene(SceneName.GlobalMap.ToString(),saveFilePathToLoadInGlobalMapScene);
    }
    public enum SceneName
    {
        GlobalMap,
    }
}
