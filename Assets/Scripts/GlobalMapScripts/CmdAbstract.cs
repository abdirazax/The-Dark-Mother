using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public abstract class CmdAbstract
{
    public static event Action<CmdAbstract> OnExecutionStart,OnExecutionEnd;
    public readonly Faction faction;
    public readonly IFactionPawn factionPawn;

    public delegate void ExecuteOnUpdateDelegate();
    public ExecuteOnUpdateDelegate OnUpdateDelegate;
    protected CmdAbstract(Faction faction,IFactionPawn factionPawn)
    {
        this.faction = faction;
        this.factionPawn = factionPawn;
        OnUpdateDelegate = DoOnFirstFrameOfExecuting;
    }
    /// <summary>
    /// function that should be performed The First Frame On Update in CommandRecorder object. After Performed it switches the delegate that called this function to another one designed for each frame execution
    /// </summary>
    protected void DoOnFirstFrameOfExecuting()
    {
        DoWhenStartedExecuting();
        OnExecutionStart?.Invoke(this); 
        OnUpdateDelegate = DoOnEachFrameOfExecuting;
    }
    /// <summary>
    /// function that should be performed Each Frame On Update in CommandRecorder object
    /// </summary>
    protected void DoOnEachFrameOfExecuting()
    {
        if (IsExecutionEndConditionMet())
        {
            OnUpdateDelegate = null;
            DoWhenEndedExecuting();
            OnExecutionEnd?.Invoke(this);
            return;
        }
        DoWhileExecuting();
    }
    public void Interrupt()
    {
        OnUpdateDelegate = null;
        DoWhenInterrupted();
        DoWhenEndedExecuting();
        OnExecutionEnd?.Invoke(this);
    }
    public bool IsFromSameFactionPawn(CmdAbstract anotherCmd)
    {
        return factionPawn == anotherCmd.factionPawn;
    }
    public abstract void DoWhenStartedExecuting();
    public abstract void DoWhileExecuting();
    public abstract void DoWhenEndedExecuting();
    public abstract void DoWhenInterrupted();
    public abstract bool IsExecutionEndConditionMet();

}


public class CmdArmyMove : CmdAbstract
{
    Army PawnArmy { get { return (Army)factionPawn; } }
    Vector3 targetDestination;
    Vector3 initialPosition;
    public CmdArmyMove(Faction faction, Army pawnArmy, Vector3 targetDestination) : base(faction,pawnArmy)
    {
        initialPosition = pawnArmy.transform.position;
        this.targetDestination = targetDestination;
    }

    public override void DoWhenStartedExecuting()
    {
        //Debug.Log(PawnArmy.name + " Started Moving \nFrom " + initialPosition + " to " + targetDestination);
        PawnArmy.Navigates.SetNewDestination(targetDestination);
    }
    public override void DoWhileExecuting()
    {
    }
    public override bool IsExecutionEndConditionMet()
    {
        return PawnArmy.Navigates.DoesNavigation.IsIdle;
    }
    public override void DoWhenEndedExecuting()
    {
        //Debug.Log(PawnArmy.name + " Ended Moving\nFrom " + initialPosition + " to " + targetDestination);
        PawnArmy.Navigates.DoesNavigation.FinishMovement();
    }

    public override void DoWhenInterrupted()
    {
        //Debug.Log("Interrupting");
        targetDestination = PawnArmy.transform.position;
        PawnArmy.Navigates.SetNewDestination(targetDestination);
    }
}
