using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class City : MonoBehaviour, IFactionPawn, ISelectablesObject 
{
    public new string name;
    [field: SerializeField]
    public Army SettledArmy { get; private set; }
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
}
