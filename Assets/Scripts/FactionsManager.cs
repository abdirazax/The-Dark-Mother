using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(GlobalMapSpawnManager))]
[RequireComponent(typeof(FactionsResourcesManager))]
/// <summary>
/// if some faction is not in the factions array, then it is a hostile neutral faction
/// if the faction is at index 0, then it is a non-hostile neutral faction
/// player faction is at index 1
/// </summary>
public class FactionsManager : MonoBehaviour,ISaveable
{
    #region Variables
    [SerializeField]
    DatabaseScriptableObject database;

    [field:SerializeField]
    public List<Faction> Factions { get; private set; }
    
    GlobalMapSpawnManager globalMapSpawnManager;
    FactionsResourcesManager factionsResourcesManager;

    /// <summary>
    /// faction opinions can't exist in and of itself. That's why they are represented by 2D Array;
    /// </summary>
    public int[,] FactionsCurrentOpinions { get; private set; }
    /// <summary>
    /// faction opinions can't exist in and of itself. That's why they are represented by 2D Array;
    /// </summary>
    public int[,] FactionsOpinionsChangeNextTurn { get; private set; }
    /// <summary>
    /// faction relations can't exist in and of itself. That's why they are represented by 2D Array;
    /// </summary>
    public Relationship[,] FactionsRelationships { get; private set; }

    
    #endregion

    
    private void CreateFactionsOpinions()
    {
        FactionsCurrentOpinions = new int[Factions.Count, Factions.Count ];
        FactionsOpinionsChangeNextTurn = new int[Factions.Count , Factions.Count ];
        FactionsRelationships = new Relationship[Factions.Count , Factions.Count ];
        for (int i = 0; i < FactionsCurrentOpinions.GetLength(0); i++)
        {
            for (int j = 0; j < FactionsCurrentOpinions.GetLength(1); j++)
            {
                FactionsCurrentOpinions[i, j] = 0;
                FactionsOpinionsChangeNextTurn[i, j] = 0;
                FactionsRelationships[i, j] = Relationship.Neutral;
            }
        }
    }

    private void Awake()
    {
        globalMapSpawnManager = GetComponent<GlobalMapSpawnManager>();
        factionsResourcesManager = GetComponent<FactionsResourcesManager>();
        CreateFactionsOpinions();
        factionsResourcesManager.CreateResourcesStores(Factions,database);
        globalMapSpawnManager.SetFactionsArmiesAndCitiesInTheScene(Factions);

    }


    public void ActivateFactionChangesPerTurn(Faction faction)
    {
        factionsResourcesManager.ActivateResourceChangesPerTurn(faction, globalMapSpawnManager);
    }
    public void UpdateFactionResources(Faction faction)
    {
        factionsResourcesManager.UpdateCapResourcesUsage(faction,globalMapSpawnManager);
        factionsResourcesManager.CalculatedFactionResourcesChangeInNextTurn(faction,globalMapSpawnManager);//this will render it on screen if it's player
    }




    public Relationship RelationshipBetween(Faction faction1, Faction faction2)
    {
        int faction1Index = Factions.IndexOf(faction1);
        int faction2Index = Factions.IndexOf(faction2);
        //if some faction is not in the factions array, then it is a hostile neutral faction
        if (faction1Index == -1 || faction2Index == -1)
            return Relationship.War;
        return FactionsRelationships[faction1Index, faction2Index];
    }
    public int CurrentOpinionOf(Faction faction1, Faction faction2)
    {
        int faction1Index = Factions.IndexOf(faction1);
        int faction2Index = Factions.IndexOf(faction2);
        //if some faction is not in the factions array, then it is a hostile neutral faction
        if (faction1Index == -1 || faction2Index == -1)
            return -100;
        return FactionsCurrentOpinions[faction1Index, faction2Index];
    }
    public int OpinionsChangeNextTurnOf(Faction faction1, Faction faction2)
    {
        int faction1Index = Factions.IndexOf(faction1);
        int faction2Index = Factions.IndexOf(faction2);
        //if some faction is not in the factions array, then it is a hostile neutral faction
        if (faction1Index == -1 || faction2Index == -1)
            return 0;
        return FactionsOpinionsChangeNextTurn[faction1Index, faction2Index];
    }

    public object SaveState()
    {
        return new SaveData(this);
    }
    public void LoadState(object state)
    {
        SaveData saveData = (SaveData)state;
        FactionsCurrentOpinions = saveData.factionsCurrentOpinions;
        FactionsOpinionsChangeNextTurn = saveData.factionsOpinionsChangeNextTurn;
        FactionsRelationships = new Relationship[FactionsCurrentOpinions.Length, FactionsCurrentOpinions.Length];
        Factions = new List<Faction>();
        for (int i = 0; i < FactionsCurrentOpinions.GetLength(0); i++)
        {
            Factions.Add(database.allFactions[saveData.factionsIds[i]]);
            for (int j = 0; j < FactionsCurrentOpinions.GetLength(1); j++)
            {
                FactionsRelationships[i, j] = (Relationship)saveData.factionsRelationships[i, j];
            }
        }
    }
    [Serializable]
    struct SaveData
    {
        public string[] factionsIds;
        public int[,] factionsCurrentOpinions;
        public int[,] factionsOpinionsChangeNextTurn;
        public int[,] factionsRelationships;


        public SaveData(FactionsManager factionsManager)
        {
            factionsIds = new string[factionsManager.Factions.Count];
            factionsCurrentOpinions = factionsManager.FactionsCurrentOpinions;
            factionsOpinionsChangeNextTurn = factionsManager.FactionsOpinionsChangeNextTurn;
            factionsRelationships = new int[factionsCurrentOpinions.Length, factionsCurrentOpinions.Length];
            for (int i = 0; i < factionsManager.Factions.Count; i++)
            {

                factionsIds[i] = factionsManager.Factions[i].Id;
                for (int j = 0; j < factionsCurrentOpinions.GetLength(1); j++)
                {
                    factionsRelationships[i, j] = (int)factionsManager.FactionsRelationships[i, j];
                }
            }

        }
    }
}

[Serializable]
public enum Relationship
{
    Neutral = 0,
    Alliance = 1,
    War = -1,
}
