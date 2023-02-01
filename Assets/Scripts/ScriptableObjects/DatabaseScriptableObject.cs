using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Database", menuName = "Persistent Data/Database")]
public class DatabaseScriptableObject : ScriptableObject
{
    public SerializableDictionary<string, Faction> allFactions = new SerializableDictionary<string, Faction>();
    public SerializableDictionary<string, Troop> allTroops = new SerializableDictionary<string, Troop>();
    public SerializableDictionary<string, Agent> allAgents = new SerializableDictionary<string, Agent>();
    public SerializableDictionary<string, Leader> allLeaders = new SerializableDictionary<string, Leader>();
    public SerializableDictionary<string, Building> allBuildings = new SerializableDictionary<string, Building>();
    public SerializableDictionary<string, ResourceType> allResourceTypes = new SerializableDictionary<string, ResourceType>();

    private void OnValidate()
    {
        SetAllDictionaries();
    }
    private void OnEnable()
    {
        SetAllDictionaries();
    }
    void SetAllDictionaries()
    {
        foreach (Faction faction in Resources.LoadAll<Faction>("ScriptableObjects/Factions"))
        {
            allFactions[faction.Id] = faction;
        }
        foreach (Troop troop in Resources.LoadAll<Troop>("ScriptableObjects/Troops"))
        {
            allTroops[troop.Id] = troop;
        }
        foreach (Agent agent in Resources.LoadAll<Agent>("ScriptableObjects/Agents"))
        {
            allAgents[agent.Id] = agent;
        }
        foreach (Leader leader in Resources.LoadAll<Leader>("ScriptableObjects/Leaders"))
        {
            allLeaders[leader.Id] = leader;
        }
        foreach (Building building in Resources.LoadAll<Building>("ScriptableObjects/Buildings"))
        {
            allBuildings[building.Id] = building;
        }
        foreach (ResourceType resourceType in Resources.LoadAll<ResourceType>("ScriptableObjects/ResourceTypes"))
        {
            allResourceTypes[resourceType.Id] = resourceType;
        }
        
    }

}
