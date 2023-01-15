using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/Building")]
public class Building : ScriptableObject
{
    [SerializeField]
    string _id;
    public string Id { get { return _id; } }
    public readonly string id;
    public new string name;
    public string description;
    [field: SerializeField]
    public Sprite sprite;

    [field: SerializeField]
    public List<Troop> garrisonAdd;
    [field: SerializeField]
    public List<Troop> canRecruit;

    [field: SerializeField]
    public List<Resource> resourceChangeWhenBuilt = new List<Resource>();
    [field: SerializeField]
    public List<Resource> resourceChangePerTurn = new List<Resource>();
    [field: SerializeField]
    public List<Resource> resourceCapChangeWhenBuilt = new List<Resource>();
}