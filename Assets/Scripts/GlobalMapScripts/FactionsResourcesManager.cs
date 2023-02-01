using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FactionsResourcesManager : MonoBehaviour
{
    [field:SerializeField]
    public SerializableDictionary<Faction, FactionResourceStore> factionResourceStores = new SerializableDictionary<Faction, FactionResourceStore>();
    public static Action<FactionResourceStore, SerializableDictionary<ResourceType, int>, bool> OnPlayerFactionResourceChangeCalculated;
    


    public void CreateResourcesStores(List<Faction> factions, DatabaseScriptableObject database)
    {
        for (int i = 0; i < factions.Count; i++)
            factionResourceStores.Add(factions[i], new FactionResourceStore(database.allResourceTypes));
    }

    public SerializableDictionary<ResourceType, int> CalculatedFactionResourcesChangeInNextTurn(Faction faction, GlobalMapSpawnManager globalMapSpawnManager, bool IsToBeDisplayedOnScreen)
    {
        SerializableDictionary<ResourceType, int> allResourceChangesInThisTurnDictionary = new SerializableDictionary<ResourceType, int>();
        foreach (City city in globalMapSpawnManager.citiesInTheScene[faction])
        {
            foreach (Building building in city.Buildings)
            {
                AddToResourceChanges(building.resourceChangePerTurn);
            }
        }
        foreach (Army army in globalMapSpawnManager.armiesInTheScene[faction])
        {
            AddToResourceChanges(army.Leader.resourceChangePerTurn);
            foreach (Troop troop in army.Troops)
                AddToResourceChanges(troop.resourceChangePerTurn);
        }
        OnPlayerFactionResourceChangeCalculated?.Invoke(factionResourceStores[faction], allResourceChangesInThisTurnDictionary, IsToBeDisplayedOnScreen);
        return allResourceChangesInThisTurnDictionary;
        void AddToResourceChanges(List<Resource> resourceChangePerTurn)
        {
            foreach (Resource incomeOfBuilding in resourceChangePerTurn)
            {
                if (!allResourceChangesInThisTurnDictionary.ContainsKey(incomeOfBuilding.resourceType))
                    allResourceChangesInThisTurnDictionary[incomeOfBuilding.resourceType] = incomeOfBuilding.amount;
                else
                    allResourceChangesInThisTurnDictionary[incomeOfBuilding.resourceType] += incomeOfBuilding.amount;
            }
        }
    }

    public void ActivateResourceChangesPerTurn(Faction faction, GlobalMapSpawnManager globalMapSpawnManager,bool isToBeDisplayedOnScreen)
    {
        UpdateCaps(faction, globalMapSpawnManager);
        foreach (Army army in globalMapSpawnManager.armiesInTheScene[faction])
        {
            army.Navigates.ReplenishMovementPoints();
        }
        List<Resource> allResourceChangesInThisTurn = new List<Resource>();
        foreach (KeyValuePair<ResourceType, int> resourceAsPair in CalculatedFactionResourcesChangeInNextTurn(faction, globalMapSpawnManager,isToBeDisplayedOnScreen))
        {
            allResourceChangesInThisTurn.Add(new Resource(resourceAsPair));
        }
        factionResourceStores[faction].AddResource(allResourceChangesInThisTurn);
    }

    void UpdateCaps(Faction faction, GlobalMapSpawnManager globalMapSpawnManager)
    {
        factionResourceStores[faction].ResetCaps(faction.factionInitialCaps);
        foreach (City city in globalMapSpawnManager.citiesInTheScene[faction])
        {
            foreach (Building building in city.Buildings)
            {
                factionResourceStores[faction].AddCap(building.resourceCapChangeWhenBuilt);
            }
        }
        foreach (Army army in globalMapSpawnManager.armiesInTheScene[faction])
        {
            factionResourceStores[faction].AddCap(army.Leader.resourceCapChangeWhenHired);
            foreach (Troop troop in army.Troops)
                factionResourceStores[faction].AddCap(troop.resourceCapChangeWhenHired);
        }
    }

    public void UpdateCapResourcesUsage(Faction faction, GlobalMapSpawnManager globalMapSpawnManager)
    {
        UpdateCaps(faction, globalMapSpawnManager);
        factionResourceStores[faction].ResetCapResources();
        foreach (City city in globalMapSpawnManager.citiesInTheScene[faction])
        {
            foreach (Building building in city.Buildings)
            {
                factionResourceStores[faction].AddOnlyCapResource(building.resourceChangeWhenBuilt);
            }
        }
        foreach (Army army in globalMapSpawnManager.armiesInTheScene[faction])
        {
            factionResourceStores[faction].AddOnlyCapResource(army.Leader.resourceChangeWhenHired);
            foreach (Troop troop in army.Troops)
                factionResourceStores[faction].AddOnlyCapResource(troop.resourceChangeWhenHired);
        }
    }


}