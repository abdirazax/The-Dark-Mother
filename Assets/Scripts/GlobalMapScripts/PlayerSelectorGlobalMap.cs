using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SelectionManager))]
[RequireComponent(typeof(FactionsManager))]
/// <summary>
/// Manages Selectables that the player interacts with.
/// Reacts to commands from PlayerSelectorInputManager class on the Global Map.
/// Shoots Raycasts based on mouse position on screen.
/// Contains SelectionHighlight object info and sends it to Selectables when player selects them.
/// </summary>
public class PlayerSelectorGlobalMap : PlayerSelectorAbstract
{

    #region Variables
    SelectionManager selectionManager;
    FactionsManager factionsManager;
    bool playerActionAllowed;

    public static event Action<InputAction> OnSelectStart, OnSelectEnd, OnActionStart, OnActionEnd;
    #endregion
    private void Awake()
    {
        selectionManager = GetComponent<SelectionManager>();
        factionsManager = GetComponent<FactionsManager>();
        playerActionAllowed = false;
    }
    public override void SelectionStart(InputAction mousePositionInputAction) 
    {
        OnSelectStart?.Invoke(mousePositionInputAction);
    }
    public override void SelectionEnd(InputAction mousePositionInputAction)
    {
        OnSelectEnd?.Invoke(mousePositionInputAction);
    }
    public override void ActionStart(InputAction mousePositionInputAction)
    {
        if (!playerActionAllowed) return;
        if (selectionManager.Selected == null) return;
        if (!factionsManager.IsFactionPlayer(selectionManager.Selected.GetObjectFaction())) return;
        OnActionStart?.Invoke(mousePositionInputAction);
    }
    public override void ActionEnd(InputAction mousePositionInputAction)
    {
        if (!playerActionAllowed) return;
        if (selectionManager.Selected == null) return;
        if (!factionsManager.IsFactionPlayer(selectionManager.Selected.GetObjectFaction())) return;
        OnActionEnd?.Invoke(mousePositionInputAction);
    }

    public void DisablePlayerAction()
    {
        playerActionAllowed = false;
    }
    public void EnablePlayerAction()
    {
        playerActionAllowed = true;
    }
}