using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/Troop")]
public class Troop : ScriptableObject
{
    [SerializeField]
    string _id;
    public string Id { get { return _id; } }
    public new string name;
    public string description;
    [field: SerializeField]
    public Sprite sprite;

    public int soldierAmount = 1;
    public int soldierHealth = 100;

    [field: SerializeField]
    public List<Resource> resourceChangeWhenHired = new List<Resource>();
    [field: SerializeField]
    public List<Resource> resourceChangePerTurn = new List<Resource>();
    [field: SerializeField]
    public List<Resource> resourceCapChangeWhenHired = new List<Resource>();

}