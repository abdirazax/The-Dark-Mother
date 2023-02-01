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
    /*[SerializeField]
    UICityPanel cityPanel;
    [SerializeField] 
    UIArmyPanel armyPanel;*/
    public float maxDistanceToTryFindNavMeshAround = 20f;
    [SerializeField]
    Highlight selectionHighlight;
    [field:SerializeField]
    public SelectableAbstract Selected { get; private set; }
    public DrawsPathOnMouseInput DrawsPathOfSelectionManager { get; private set; }
    public CommandRecorder CommandRecorder { get; private set; }
    private readonly string terrainToMoveOnTag = "TerrainToMoveOn";
    public static event Action<Sprite> OnPortraitHaverSelected;
    public static event Action OnSomethingDeselected;
    public static event Action<SelectableCity> OnCitySelected;
    public static event Action<SelectableArmy> OnArmySelected;
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
        OnPortraitHaverSelected?.Invoke(selectableArmy.GetPortraitSprite());
        OnArmySelected?.Invoke(selectableArmy);
        selectionHighlight.Follow(selectableArmy.transform, 1);
    }
    public void SetSelectedCity(SelectableCity selectableCity)
    {
        ResetSelected();
        if (selectableCity == null)
            return;
        Selected = selectableCity;
        OnPortraitHaverSelected?.Invoke(selectableCity.GetPortraitSprite());
        OnCitySelected?.Invoke(selectableCity);
        selectionHighlight.Follow(selectableCity.transform, 3);
    }
    public void ResetSelected()
    {
        Selected = null;
        OnSomethingDeselected?.Invoke();
        selectionHighlight.Disable();
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
