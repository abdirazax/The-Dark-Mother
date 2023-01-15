using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(FactionsManager))]
[RequireComponent(typeof(PlayerSelectorGlobalMap))]
[RequireComponent(typeof(CommandRecorder))]
public class TurnManager : MonoBehaviour, ISaveable 
{
    CommandRecorder commandRecorder;
    FactionsManager factionsManager;
    PlayerSelectorGlobalMap playerSelectorGlobalMap;
    [SerializeField]
    int currentFactionIndex;
    public int CurrentFactionIndex => currentFactionIndex;
    public TurnStateAbstract CurrentTurnState { get; private set; }
    TurnStateAbstract temporaryPausedTurnStateToGoBackToWhenUnpaused;
    TurnStateFocusingOnUI focusingOnUI = new TurnStateFocusingOnUI();
    TurnStateWaitingForCurrentFactionActions waitingForCurrentFactionActions = new TurnStateWaitingForCurrentFactionActions();
    TurnStateWaitingForAllActionsToEndToFinishThisTurn waitingForAllActionsToEndToFinishThisTurn = new TurnStateWaitingForAllActionsToEndToFinishThisTurn();
    TurnStateApplyingFactionChangesInThisTurn applyingFactionChangesInThisTurn = new TurnStateApplyingFactionChangesInThisTurn();
    [SerializeField]
    int turnNumber;
    public int TurnNumber => turnNumber;
    Faction CurrentFaction { get { return factionsManager.Factions[CurrentFactionIndex]; } }
    


    private void Awake()
    {
        currentFactionIndex = 0;
        factionsManager = GetComponent<FactionsManager>();
        playerSelectorGlobalMap = GetComponent<PlayerSelectorGlobalMap>();
        commandRecorder = GetComponent<CommandRecorder>();
    }
    private void Start()
    {
        factionsManager.UpdateFactionResources(CurrentFaction);
        CurrentTurnState = waitingForCurrentFactionActions;
        CurrentTurnState.StartState(this);
    }
    private void Update()
    {
        CurrentTurnState.UpdateState(this);
    }
    private void LateUpdate()
    {
        CurrentTurnState.LateUpdateState(this);
    }
    void PauseStateToFocusOnUI()
    {
        if (CurrentTurnState == waitingForCurrentFactionActions ||
            CurrentTurnState == waitingForAllActionsToEndToFinishThisTurn)
        {
            temporaryPausedTurnStateToGoBackToWhenUnpaused = CurrentTurnState;
            CurrentTurnState = focusingOnUI;
            CurrentTurnState.StartState(this);
        }
    }
    void UnpauseStateFromFocusOnUI()
    {
        CurrentTurnState.EndState(this);
        CurrentTurnState = temporaryPausedTurnStateToGoBackToWhenUnpaused;
    }
    public void EndTurnButtonForPlayer()
    {
        if (CurrentTurnState == waitingForCurrentFactionActions)
            CurrentTurnState.EndState(this);
    }
    void IncreaseTurn()
    {
        turnNumber++;
    }
    void SwitchToNextFaction()
    {
        currentFactionIndex++;
        if (CurrentFactionIndex >= factionsManager.Factions.Count)
        {
            currentFactionIndex = 0;
            IncreaseTurn();
        }
    }
    bool IsCurrentFactionPlayer()
    {
        return CurrentFaction.isPlayerFaction;
    }
    void EnablePlayerActionAllowedIfCurrentFactionIsPlayer()
    {
        if (IsCurrentFactionPlayer())
            playerSelectorGlobalMap.PlayerActionAllowed = true;
    }
    void DisablePlayerActionAllowed()
    {
        playerSelectorGlobalMap.PlayerActionAllowed = false;
    }
    void ExecuteAndRecordAllCommandsOfCurrentFaction()
    {
        //Debug.Log("AI executes its own commands");
    }


    public class TurnStateFocusingOnUI : TurnStateAbstract
    {
        public override void StartState(TurnManager turnManager)
        {
            turnManager.DisablePlayerActionAllowed();
        }
        public override void EndState(TurnManager turnManager)
        {
            turnManager.EnablePlayerActionAllowedIfCurrentFactionIsPlayer();
        }
        public override void LateUpdateState(TurnManager turnManager) { }
        public override void UpdateState(TurnManager turnManager) { }
    }
    public class TurnStateWaitingForCurrentFactionActions : TurnStateAbstract
    {
        public override void StartState(TurnManager turnManager)
        {
            turnManager.factionsManager.UpdateFactionResources(turnManager.CurrentFaction);
            turnManager.EnablePlayerActionAllowedIfCurrentFactionIsPlayer();
            if (!turnManager.IsCurrentFactionPlayer())
            {
                turnManager.ExecuteAndRecordAllCommandsOfCurrentFaction();
                EndState(turnManager);
                return;
            }
        }
        public override void EndState(TurnManager turnManager)
        {
            turnManager.DisablePlayerActionAllowed();
            turnManager.CurrentTurnState = turnManager.waitingForAllActionsToEndToFinishThisTurn;
            turnManager.CurrentTurnState.StartState(turnManager);
        }
        public override void LateUpdateState(TurnManager turnManager) { }
        public override void UpdateState(TurnManager turnManager) { }
    }
    public class TurnStateWaitingForAllActionsToEndToFinishThisTurn : TurnStateAbstract
    {
        public override void StartState(TurnManager turnManager) { }
        public override void EndState(TurnManager turnManager)
        {
            turnManager.SwitchToNextFaction();
            turnManager.CurrentTurnState = turnManager.applyingFactionChangesInThisTurn;
            turnManager.CurrentTurnState.StartState(turnManager);
        }
        public override void LateUpdateState(TurnManager turnManager)
        {
            if (!turnManager.commandRecorder.IsThereAnyUnfinishedCmdsLeft())
            {
                EndState(turnManager);
            }
        }
        public override void UpdateState(TurnManager turnManager) { }
    }
    public class TurnStateApplyingFactionChangesInThisTurn : TurnStateAbstract
    {
        public override void StartState(TurnManager turnManager)
        {
            turnManager.factionsManager.ActivateFactionChangesPerTurn(turnManager.CurrentFaction);
            EndState(turnManager);
        }
        public override void EndState(TurnManager turnManager)
        {
            turnManager.CurrentTurnState = turnManager.waitingForCurrentFactionActions;
            turnManager.CurrentTurnState.StartState(turnManager);
        }
        public override void LateUpdateState(TurnManager turnManager) { }
        public override void UpdateState(TurnManager turnManager) { }
    }
    public abstract class TurnStateAbstract
    {
        public abstract void StartState(TurnManager turnManager);
        public abstract void UpdateState(TurnManager turnManager);
        public abstract void LateUpdateState(TurnManager turnManager);
        public abstract void EndState(TurnManager turnManager);
    }

    public object SaveState()
    {
        return new SaveData(this);
    }
    public void LoadState(object state)
    {
        SaveData saveData = (SaveData)state;
        currentFactionIndex = saveData.currentFactionIndex;
        turnNumber = saveData.turnNumber;
    }
    [Serializable]
    struct SaveData
    {
        public int currentFactionIndex;
        public int turnNumber;
        public SaveData(TurnManager turnManager)
        {
            currentFactionIndex = turnManager.CurrentFactionIndex;
            turnNumber = turnManager.TurnNumber;
        }
    }
}
