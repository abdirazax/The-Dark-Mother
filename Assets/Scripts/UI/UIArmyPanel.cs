using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIArmyPanel : MonoBehaviour
{
    [SerializeField]
    RectTransform troopsPanel;
    List<Image> troopPortraits;
    private void Awake()
    {
        troopPortraits = new List<Image>();
        for (int i = 0; i < troopsPanel.childCount; i++)
        {
            troopPortraits.Add(troopsPanel.GetChild(i).GetComponent<Image>());
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void RenderArmy(SelectableArmy selectedArmy)
    {
        int iStartFrom = 0, j = 0;
        if (selectedArmy.Army.Leader != null)
        {
            Leader selectedLeader = selectedArmy.Army.Leader;
            troopPortraits[0].sprite = selectedLeader.sprite;
            troopPortraits[0].rectTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedLeader.name;
            troopPortraits[0].gameObject.SetActive(true);
            iStartFrom++;
        }
        for (int i = iStartFrom; i < troopPortraits.Count; i++)
        {
            if (selectedArmy.Army.Troops.Count > j)
            {
                troopPortraits[i].sprite = selectedArmy.Army.Troops[j].sprite;
                troopPortraits[i].rectTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedArmy.Army.Troops[j].name;
                troopPortraits[i].gameObject.SetActive(true);
                j++;
                continue;
            }
            troopPortraits[i].gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }
}