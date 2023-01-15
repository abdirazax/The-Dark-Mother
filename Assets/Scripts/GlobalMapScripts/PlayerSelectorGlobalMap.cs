using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SelectionManager))]
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
    public bool PlayerActionAllowed { get; set; }

    public static event Action<InputAction> OnSelectStart, OnSelectEnd, OnActionStart, OnActionEnd;
    #endregion
    private void Awake()
    {
        selectionManager = GetComponent<SelectionManager>();
        PlayerActionAllowed = true;
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
        if (!PlayerActionAllowed) return;
        if (selectionManager.Selected == null) return;
        if (!selectionManager.Selected.Faction.isPlayerFaction) return;
        OnActionStart?.Invoke(mousePositionInputAction);
    }
    public override void ActionEnd(InputAction mousePositionInputAction)
    {
        if (!PlayerActionAllowed) return;
        if (selectionManager.Selected == null) return;
        if (!selectionManager.Selected.Faction.isPlayerFaction) return;
        OnActionEnd?.Invoke(mousePositionInputAction);
    }

    public void DisablePlayerAction()
    {
        PlayerActionAllowed = false;
    }
    public void EnablePlayerAction()
    {
        PlayerActionAllowed = true;
    }
}