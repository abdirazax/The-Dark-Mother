using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
/// <summary>
/// Main managing class for an Army.
/// Can be selected by PlayerSelectorGlobalMap and react to its commands.
/// All Selectables usually have one or more ColliderOfSelectable objects that serve as a receiver of raycasts from player input for this class
/// </summary>
[RequireComponent(typeof(Army))]
public class SelectableArmy : SelectableAbstract
{
    #region Variables
    public Army Army { 
        get { return (Army)selectablesObject; } 
        private set { selectablesObject = value; } 
    }
    #endregion
    private void Awake()
    {
        Army = GetComponent<Army>();
        Faction = Army.Faction;
    }

    public override void MakeSelected(SelectionManager selectionManager)
    {
        selectionManager.SetSelectedArmy(this);
    }

    public override void Act(CommandRecorder commandRecorder, SelectableAbstract anotherSelectable)
    {
        if (anotherSelectable == null) return;
        Army.Act(commandRecorder, anotherSelectable);
    }

    public override void Move(CommandRecorder commandRecorder, Vector3 target)
    {
        if (target == null) return;
        Army.Move(commandRecorder, target);
    }


    public override Sprite GetPortraitSprite()
    {
        if (portraitSprite != null)
            return base.GetPortraitSprite();
        return Army.Leader.sprite;
    }

    public override void StartPlanningAction(SelectionManager selectionManager, InputAction mousePositionInputAction)
    {
        Navigates navigator = GetComponent<Navigates>();
        if (navigator == null) return;
        selectionManager.DrawsPathOfSelectionManager.StartDrawingPathOf(navigator, mousePositionInputAction);
    }

    public override void EndPlanningAction(SelectionManager selectionManager, InputAction mousePositionInputAction)
    {
        //handle action when mouse button released on another Selectable
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePositionInputAction.ReadValue<Vector2>());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (Utils.IsMouseOverUI(mousePositionInputAction))
                    return;
                ColliderOfSelectable hitColliderOfSelectable = hit.transform.GetComponent<ColliderOfSelectable>();
                if (hitColliderOfSelectable != null)
                {
                    Act(selectionManager.CommandRecorder, hitColliderOfSelectable.selectable);
                    return;
                }
            }
        }

        // if mouse action did not land on another selectable then
        // handle action like move
        {
            Move(selectionManager.CommandRecorder, selectionManager.DrawsPathOfSelectionManager.lastMousePositionOnTerrain);
            //drawsPathOfSelectionManager.HideDrawnPath();
        }
    }
}
