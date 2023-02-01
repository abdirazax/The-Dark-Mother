using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMapSpawnManager : MonoBehaviour,ISaveable
{
    [SerializeField]
    DatabaseScriptableObject database;
    public SerializableDictionary<Faction, List<Army>> armiesInTheScene = new SerializableDictionary<Faction, List<Army>>();
    public SerializableDictionary<Faction, List<City>> citiesInTheScene = new SerializableDictionary<Faction, List<City>>();
    [SerializeField]
    Transform armiesContainer, citiesContainer;
    [SerializeField]
    GameObject armyPrototype, cityPrototype;
    public static event Action OnStartedDestroyingGameObjects;
    private void Awake()
    {
        FindFactionsArmiesInTheScene();
        FindFactionsCitiesInTheScene();
    }
    public void FindFactionsArmiesInTheScene()
    {
        armiesInTheScene = new SerializableDictionary<Faction, List<Army>>();
        foreach (Faction foundFaction in database.allFactions.Values)
        {
            armiesInTheScene.Add(foundFaction, new List<Army>());
        }
        Army[] armiesInContainer = armiesContainer.GetComponentsInChildren<Army>();
        for (int i = 0; i < armiesInContainer.Length; i++)
        {
            armiesInTheScene[armiesInContainer[i].Faction].Add(armiesInContainer[i]);
        }
    }
    public void FindFactionsCitiesInTheScene()
    {
        citiesInTheScene = new SerializableDictionary<Faction, List<City>>();
        foreach (Faction foundFaction in database.allFactions.Values)
        {
            citiesInTheScene.Add(foundFaction, new List<City>());
        }
        City[] citiesInContainer = citiesContainer.GetComponentsInChildren<City>();
        for (int i = 0; i < citiesInContainer.Length; i++)
        {
            citiesInTheScene[citiesInContainer[i].Faction].Add(citiesInContainer[i]);
        }
    }

    public void InstantiateArmy(ArmySaveData armyData)
    {
        GameObject armyObject = Instantiate(armyPrototype, armiesContainer);
        armyObject.name = armyData.name;
        armyObject.GetComponent<Army>().LoadState(armyData, database);

    }
    public void InstantiateCity(CitySaveData cityData)
    {
        GameObject cityObject = Instantiate(cityPrototype, citiesContainer);
        cityObject.name = cityData.name;
        cityObject.GetComponent<City>().LoadState(cityData, database);
    }

    public void DestroyArmiesInTheScene()
    {
        OnStartedDestroyingGameObjects?.Invoke();
        foreach(KeyValuePair<Faction,List<Army>> factionListArmyPair in armiesInTheScene)
        {
            for(int i = 0; i < factionListArmyPair.Value.Count; i++)
            {
                Destroy(factionListArmyPair.Value[i].gameObject);
            }
        }
        FindFactionsArmiesInTheScene();
    }
    public void DestroyCitiesInTheScene()
    {
        OnStartedDestroyingGameObjects?.Invoke();
        foreach (KeyValuePair<Faction, List<City>> factionListCityPair in citiesInTheScene)
        {
            for (int i = 0; i < factionListCityPair.Value.Count; i++)
            {
                Destroy(factionListCityPair.Value[i].gameObject);
            }
        }
        FindFactionsCitiesInTheScene();
    }

    public object SaveState()
    {
        return new SaveData(this);
    }
    public void LoadState(object state)
    {
        DestroyArmiesInTheScene();
        DestroyCitiesInTheScene();
        SaveData saveData = (SaveData)state;
        armiesInTheScene = new SerializableDictionary<Faction, List<Army>>();
        citiesInTheScene = new SerializableDictionary<Faction, List<City>>();

        foreach (KeyValuePair<string, ArmySaveData[]> factionIdArmyDataPair in saveData.armiesInTheScene)
        {
            for (int j = 0; j < factionIdArmyDataPair.Value.Length; j++)
                InstantiateArmy(factionIdArmyDataPair.Value[j]);
        }
        foreach (KeyValuePair<string, CitySaveData[]> factionIdCityDataPair in saveData.citiesInTheScene)
        {
            for (int j = 0; j < factionIdCityDataPair.Value.Length; j++)
                InstantiateCity(factionIdCityDataPair.Value[j]);
        }
        FindFactionsArmiesInTheScene();
        FindFactionsCitiesInTheScene();
    }
    [Serializable]
    struct SaveData
    {
        public SerializableDictionary<string, ArmySaveData[]> armiesInTheScene;
        public SerializableDictionary<string, CitySaveData[]> citiesInTheScene;

        public SaveData(GlobalMapSpawnManager globalMapSpawnManager)
        {
            armiesInTheScene = new SerializableDictionary<string, ArmySaveData[]>();
            citiesInTheScene = new SerializableDictionary<string, CitySaveData[]>();
            globalMapSpawnManager.FindFactionsArmiesInTheScene();
            globalMapSpawnManager.FindFactionsCitiesInTheScene();
            foreach (KeyValuePair<Faction, List<Army>> factionArmyPair in globalMapSpawnManager.armiesInTheScene)
            {
                ArmySaveData[] tempArmySaveData = new ArmySaveData[factionArmyPair.Value.Count];
                for (int j = 0; j < tempArmySaveData.Length; j++)
                    tempArmySaveData[j] = new ArmySaveData(factionArmyPair.Value[j]);
                armiesInTheScene.Add(factionArmyPair.Key.Id, tempArmySaveData);
            }
            foreach (KeyValuePair<Faction, List<City>> factionCityPair in globalMapSpawnManager.citiesInTheScene)
            {
                CitySaveData[] tempCitySaveData = new CitySaveData[factionCityPair.Value.Count];
                for (int j = 0; j < tempCitySaveData.Length; j++)
                    tempCitySaveData[j] = new CitySaveData(factionCityPair.Value[j]);
                citiesInTheScene.Add(factionCityPair.Key.Id, tempCitySaveData);
            }
        }

    }
}