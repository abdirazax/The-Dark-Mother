using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] GameObject loaderCanvas;
    [SerializeField] Image progressBar;
    public static event Action<string> OnSceneLoadedWithSaveFilePathIndication;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public async void LoadScene(string sceneName, string saveFileToLoadPath)
    {
        progressBar.fillAmount = 0;
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        loaderCanvas.SetActive(true);
        do {
            await Task.Delay(100);
            progressBar.fillAmount = scene.progress / .9f;
        } while (scene.progress < .9f);
        scene.allowSceneActivation = true;
        OnSceneLoadedWithSaveFilePathIndication?.Invoke(saveFileToLoadPath);
        loaderCanvas.SetActive(false);
    }
}
