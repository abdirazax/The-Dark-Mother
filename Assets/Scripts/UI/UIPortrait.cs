using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPortrait : MonoBehaviour
{
    Sprite defaultSprite;
    Image portrait;
    private void Awake()
    {
        portrait = GetComponent<Image>();
        defaultSprite = portrait.sprite;
    }
    private void OnEnable()
    {
        SelectionManager.OnPortraitHaverSelected += SetPortrait;
        SelectionManager.OnSomethingDeselected += Hide;
    }
    private void OnDisable()
    {
        SelectionManager.OnPortraitHaverSelected -= SetPortrait;
        SelectionManager.OnSomethingDeselected -= Hide;
    }

    /// <summary>
    /// Change Portrait in the UI to selected selectable.
    /// </summary>
    /// <param name="portraitSprite">If null, then sets a default sprite that it had on Awake method</param>
    public void SetPortrait(Sprite portraitSprite)
    {
        if (portraitSprite == null)
        {
            SetDefault();
            return;
        }
        portrait.sprite = portraitSprite;
        gameObject.SetActive(true);
    }

    void SetDefault()
    {
        portrait.sprite = defaultSprite;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
