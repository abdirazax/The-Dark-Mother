using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(menuName = "Scriptable Objects/Faction")]
public class Faction : ScriptableObject
{
    [SerializeField]
    string _id;
    public string Id { get { return _id; } }
    public new string name;
    public string description;
    public bool isPlayerFaction;


    [field:SerializeField]
    public SerializableDictionary<ResourceType, int> factionInitialResources;
    [field: SerializeField]
    public SerializableDictionary<ResourceType, int> factionInitialCaps;
    
}