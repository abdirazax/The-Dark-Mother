using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class City : MonoBehaviour, IFactionPawn, ISelectablesObject 
{
    public new string name;
    [field: SerializeField]
    public List<Building> Buildings { get; private set; }
    [field: SerializeField]
    public Faction Faction { get; private set; }
    private void Awake()
    {
        
    }
    public void Act(CommandRecorder commandRecorder, SelectableAbstract target)
    {

    }
    public void LoadState(CitySaveData loadedCityData, DatabaseScriptableObject database)
    {
        name = loadedCityData.name;
        Buildings = new List<Building>();
        foreach (string loadedBuildingId in loadedCityData.buildingsIds)
            Buildings.Add(database.allBuildings[loadedBuildingId]);
        Faction = database.allFactions[loadedCityData.factionId];
        transform.position = new Vector3(loadedCityData.transformPosition[0], loadedCityData.transformPosition[1], loadedCityData.transformPosition[2]);
    }

    public Faction GetFaction()
    {
        return Faction;
    }
}

[Serializable]
public struct CitySaveData
{
    public string name;
    public string[] buildingsIds;
    public string factionId;
    public float[] transformPosition;
    public CitySaveData(City city)
    {
        name = city.name;
        buildingsIds = new string[city.Buildings.Count];
        for (int i = 0; i < buildingsIds.Length; i++)
        {
            buildingsIds[i] = city.Buildings[i].Id;
        }
        factionId = city.Faction.Id;
        transformPosition = new float[3] { city.transform.position.x, city.transform.position.y, city.transform.position.z };
    }
}