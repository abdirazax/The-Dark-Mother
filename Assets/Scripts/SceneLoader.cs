using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum SceneName
    {
        GameScene,
    }
    public static void Load(SceneName sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());

    }
}
