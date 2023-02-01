using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICityPanel : MonoBehaviour
{
    
    [SerializeField]
    RectTransform buildingsPanel;
    List<Image> buildingPortraits;
    private void Awake()
    {
        buildingPortraits = new List<Image>();
        for (int i = 0; i < buildingsPanel.childCount; i++)
        {
            buildingPortraits.Add(buildingsPanel.GetChild(i).GetComponent<Image>());
        }
    }

    private void OnEnable()
    {
        SelectionManager.OnCitySelected += RenderCity;
        SelectionManager.OnSomethingDeselected += Hide;
    }
    private void OnDisable()
    {
        SelectionManager.OnCitySelected -= RenderCity;
        SelectionManager.OnSomethingDeselected -= Hide;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void RenderCity(SelectableCity selectedCity )
    {
        for (int i = 0; i < buildingPortraits.Count; i++)
        {
            if (selectedCity.City.Buildings.Count > i)
            {
                buildingPortraits[i].sprite = selectedCity.City.Buildings[i].sprite;
                buildingPortraits[i].rectTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedCity.City.Buildings[i].name;
                buildingPortraits[i].gameObject.SetActive(true);
                continue;
            }
            buildingPortraits[i].gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }
}
