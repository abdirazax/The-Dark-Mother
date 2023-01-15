using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneButton : MonoBehaviour
{
    public enum SceneName
    {
        GlobalMap,
    }
    public void ChangeToGlobalMapScene()
    {
        LevelManager.Instance.LoadScene(SceneName.GlobalMap.ToString());
    }
}
