using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMapSpawnManager : MonoBehaviour
{

    public SerializableDictionary<Faction, List<Army>> armiesInTheScene = new SerializableDictionary<Faction, List<Army>>();
    public SerializableDictionary<Faction, List<City>> citiesInTheScene = new SerializableDictionary<Faction, List<City>>();
    [SerializeField]
    Transform armiesContainer, citiesContainer;

    public void SetFactionsArmiesAndCitiesInTheScene(List<Faction> factions)
    {
        armiesInTheScene = new SerializableDictionary<Faction, List<Army>>();
        for (int i = 0; i < factions.Count; i++)
        {
            armiesInTheScene.Add(factions[i], new List<Army>());
            citiesInTheScene.Add(factions[i], new List<City>());
        }

        Army[] armiesInContainer = armiesContainer.GetComponentsInChildren<Army>();
        for (int i = 0; i < armiesInContainer.Length; i++)
        {
            armiesInTheScene[armiesInContainer[i].Faction].Add(armiesInContainer[i]);
        }

        City[] citiesInContainer = citiesContainer.GetComponentsInChildren<City>();
        for (int i = 0; i < citiesInContainer.Length; i++)
        {
            citiesInTheScene[citiesInContainer[i].Faction].Add(citiesInContainer[i]);
        }
    }
}
