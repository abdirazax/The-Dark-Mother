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
    public List<Troop> Troops { get; private set; }
    [field: SerializeField]
    public Faction Faction { get; private set; }
    private void Awake()
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

}


