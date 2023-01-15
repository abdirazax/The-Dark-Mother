
using UnityEngine;
[CreateAssetMenu(menuName ="Scriptable Objects/Resource Type")]
public class ResourceType : ScriptableObject
{
    [SerializeField]
    string _id;
    public string Id { get { return _id; } }
    public new string name;
    public float resourcePaybackWhenUnbought;
    public bool canBePastMinCap, havePenaltyIfPastMinCap, canBePastMaxCap, havePenaltyIfPastMaxCap, isDesignedToBeCapResource;
}
