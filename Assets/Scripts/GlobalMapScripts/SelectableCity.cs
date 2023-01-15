using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Main managing class for a City.
/// Can be selected by PlayerSelectorGlobalMap and react to its commands.
/// All Selectables usually have one or more ColliderOfSelectable objects that serve as a receiver of raycasts from player input for this class
/// </summary>
[RequireComponent(typeof(City))]
public class SelectableCity : SelectableAbstract
{
    #region Variables
    public City City
    {
        get { return (City)selectablesObject; }
        private set { selectablesObject = value; }
    }
    #endregion
    private void Awake()
    {
        City = GetComponent<City>();
        Faction = City.Faction;
    }
    /// <param name="selectionManager">Selection Manager that will highlight this and change UI</param>
    public override void MakeSelected(SelectionManager selectionManager)
    {
        selectionManager.SetSelectedCity(this);
    }
    public override Sprite GetPortraitSprite()
    {
        if (portraitSprite != null || City.Buildings.Count == 0)
            return base.GetPortraitSprite();
        return City.Buildings[0].sprite;
    }

    public override void StartPlanningAction(SelectionManager selectionManager, InputAction mousePositionInputAction){    }

    public override void EndPlanningAction(SelectionManager selectionManager, InputAction mousePositionInputAction){}

    public override void Act(CommandRecorder commandRecorder, SelectableAbstract anotherSelectable){}

    public override void Move(CommandRecorder commandRecorder, Vector3 target){}
}
