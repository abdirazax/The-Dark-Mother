using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Navigates))]
public class Army : MonoBehaviour, IFactionPawn, ISelectablesObject
{

    public Navigates Navigates { get; private set; }
    public new string name;
    [field: SerializeField]
    public Leader Leader { get; private set; }
    [field: SerializeField]
    public List<Agent> Agents { get; private set; }
    [field: SerializeField]
    public List<Troop> Troops { get; private set; }
    [field: SerializeField]
    public Faction Faction { get; private set; }
    private void Awake()
    {
        Initialize();
    }
    void Initialize()
    {
        Navigates = GetComponent<Navigates>();
    }
    public void Act(CommandRecorder commandRecorder, SelectableAbstract target)
    {
    }
    public void Move(CommandRecorder commandRecorder, Vector3 target)
    {
        if (target == null)
            return;
        CmdArmyMove cmdArmyMove = new CmdArmyMove(Faction, this, target);
        commandRecorder.DoThisCmd(cmdArmyMove);
    }

    public void LoadState(ArmySaveData loadedArmyData,DatabaseScriptableObject database)
    {
        Navigates.LoadState(loadedArmyData.navigatesSaveData);
        name = loadedArmyData.name;
        Leader = database.allLeaders[loadedArmyData.leaderId];
        Agents = new List<Agent>();
        foreach (string loadedAgentId in loadedArmyData.agentsIds)
            Agents.Add(database.allAgents[loadedAgentId]);
        Troops = new List<Troop>();
        foreach (string loadedTroopId in loadedArmyData.troopsIds)
            Troops.Add(database.allTroops[loadedTroopId]);
        Faction = database.allFactions[loadedArmyData.factionId];
        Initialize();
    }

    public Faction GetFaction()
    {
        return Faction;
    }
}


[Serializable]
public struct ArmySaveData
{
    public string name;
    public string leaderId;
    public string[] troopsIds;
    public string[] agentsIds;
    public string factionId;
    public NavigatesSaveData navigatesSaveData;
    public ArmySaveData(Army army)
    {
        name = army.name;
        leaderId = army.Leader.Id;

        agentsIds = new string[army.Agents.Count];
        for (int i = 0; i < agentsIds.Length; i++)
            agentsIds[i] = army.Agents[i].Id;
        troopsIds = new string[army.Troops.Count];
        for (int i = 0; i < troopsIds.Length; i++)
            troopsIds[i] = army.Troops[i].Id;
        
        factionId = army.Faction.Id;
        navigatesSaveData = new NavigatesSaveData(army.Navigates);
    }
}


