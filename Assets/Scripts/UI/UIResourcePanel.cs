using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIResourcePanel : MonoBehaviour
{
    [SerializeField]
    RectTransform resourcesLayout;
    List<TextMeshProUGUI> resourcesTexts;
    private void Awake()
    {
        resourcesTexts = new List<TextMeshProUGUI>();
        for (int i = 0; i < resourcesLayout.childCount; i++)
        {
            resourcesTexts.Add(resourcesLayout.GetChild(i).GetComponent<TextMeshProUGUI>());
        }
    }
    private void OnEnable()
    {
        FactionsResourcesManager.OnPlayerFactionResourceChangeCalculated += RenderResources;
    }
    private void OnDisable()
    {
        FactionsResourcesManager.OnPlayerFactionResourceChangeCalculated -= RenderResources;
    }
    public void RenderResources(FactionResourceStore factionResourceStore, SerializableDictionary<ResourceType, int> factionResourceChangeInNextTurn, bool isThisPlayerFaction)
    {
        if (!isThisPlayerFaction) return;
        int i = 0;
        foreach (KeyValuePair<ResourceType,int> resource in factionResourceStore.FactionResources)
        {
            string changeInNextTurn = "";
            if (factionResourceChangeInNextTurn.ContainsKey(resource.Key))
            {
                changeInNextTurn += "(";
                if (factionResourceChangeInNextTurn[resource.Key] > 0)
                    changeInNextTurn += "+";
                changeInNextTurn += factionResourceChangeInNextTurn[resource.Key] + ")";
            }
            if (resource.Value == 0 && factionResourceStore.FactionCaps[resource.Key] == 0) continue;
            resourcesTexts[i].text = $"{resource.Value}/{factionResourceStore.FactionCaps[resource.Key]}{changeInNextTurn} {resource.Key.name}";
            resourcesTexts[i].gameObject.SetActive(true);
            i++;
        }
        for(int j = i; j < resourcesTexts.Count; j++)
        {
            resourcesTexts[j].gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }
}