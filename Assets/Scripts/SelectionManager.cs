using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

[RequireComponent(typeof(DrawsPathOnMouseInput))]
[RequireComponent(typeof(CommandRecorder))]
public class SelectionManager : MonoBehaviour
{

    #region Variables
    [SerializeField]
    UIPortrait portrait;
    [SerializeField]
    UICityPanel cityPanel;
    [SerializeField] 
    UIArmyPanel armyPanel;
    [SerializeField]
    Highlight selectionHighlight;
    [field:SerializeField]
    public SelectableAbstract Selected { get; private set; }
    public DrawsPathOnMouseInput DrawsPathOfSelectionManager { get; private set; }
    public CommandRecorder CommandRecorder { get; private set; }
    private readonly string terrainToMoveOnTag = "TerrainToMoveOn";

    #endregion
    private void OnEnable()
    {
        PlayerSelectorGlobalMap.OnSelectEnd += TrySelecting;
        PlayerSelectorGlobalMap.OnActionStart += StartPlanningPotentialActionOfSelected;
        PlayerSelectorGlobalMap.OnActionEnd += EndPlanningPotentialActionOfSelected;
    }
    private void OnDisable()
    {
        PlayerSelectorGlobalMap.OnSelectEnd -= TrySelecting;
        PlayerSelectorGlobalMap.OnActionStart -= StartPlanningPotentialActionOfSelected;
        PlayerSelectorGlobalMap.OnActionEnd -= EndPlanningPotentialActionOfSelected;
    }



    private void Awake()
    {
        //armyPanel = UIDocument.GetComponent<UIArmyPanel>();
        DrawsPathOfSelectionManager = GetComponent<DrawsPathOnMouseInput>();
        CommandRecorder = GetComponent<CommandRecorder>();
    }

    void TrySelecting(InputAction mousePositionInputAction)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePositionInputAction.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Utils.IsMouseOverUI(mousePositionInputAction))
                return;
            if (hit.transform.GetComponent<ColliderOfSelectable>() != null)
            {
                //Selectable should invoke its MakeSelected function because selectable can be army or city or something else.
                //And their MakeSelected functions will call different SetSelected functions
                hit.transform.GetComponent<ColliderOfSelectable>().selectable.MakeSelected(this);
                return;
            }
            ResetSelected();
        }
    }

    public void SetSelectedArmy(SelectableArmy selectableArmy)
    {
        ResetSelected();
        if (selectableArmy == null)
            return;
        Selected = selectableArmy;
        portrait.SetPortrait(selectableArmy.GetPortraitSprite());
        armyPanel.RenderArmy(selectableArmy);
        selectionHighlight.Follow(selectableArmy.transform, 1);
    }
    public void SetSelectedCity(SelectableCity selectableCity)
    {
        ResetSelected();
        if (selectableCity == null)
            return;
        Selected = selectableCity;
        portrait.SetPortrait(selectableCity.GetPortraitSprite());
        cityPanel.RenderCity(selectableCity);
        selectionHighlight.Follow(selectableCity.transform, 3);
    }
    public void ResetSelected()
    {
        Selected = null;
        portrait.Hide();
        armyPanel.Hide();
        cityPanel.Hide();
        selectionHighlight.Remove();
    }

    

    public void StartPlanningPotentialActionOfSelected(InputAction mousePositionInputAction)
    {
        if (Selected == null) return;
        Selected.StartPlanningAction(this, mousePositionInputAction);
    }

    public void EndPlanningPotentialActionOfSelected(InputAction mousePositionInputAction)
    {
        DrawsPathOfSelectionManager.StopDrawing();
        if (Selected == null)
            return;
        Selected.EndPlanningAction(this, mousePositionInputAction);
    }


}
