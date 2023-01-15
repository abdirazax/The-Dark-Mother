using System;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Anything that can be selected by player (Army, City).
/// All interactive things on the map are usually Selectables.
/// Can be selected by PlayerSelectorGlobalMap and react to its commands.
/// All Selectables usually have one or more ColliderOfSelectable objects that serve as a receiver of raycasts from player input for this class
/// </summary>
public abstract class SelectableAbstract:MonoBehaviour,IHavePortrait
{
    [SerializeField]
    protected Sprite portraitSprite;
    public Faction Faction { get; protected set; }
    protected ISelectablesObject selectablesObject;
    public ISelectablesObject SelectablesObject => selectablesObject;

    /// <summary>
    /// Called when the seleactable is selected by player's selection key input
    /// </summary>
    /// <param name="selectionHighlight">An object that will visiually highlight the selectable when selected</param>
    public abstract void MakeSelected(SelectionManager selectionManager);

    public abstract void StartPlanningAction(SelectionManager selectionManager, InputAction mousePositionInputAction);
    public abstract void EndPlanningAction(SelectionManager selectionManager, InputAction mousePositionInputAction);





    /// <summary>
    /// Called when the selectable is commanded to act.
    /// Ex: If this is an Army and target is hostile army they will fight. If this is Army and target is friendly city Army will garrison in this City
    /// </summary>
    /// <param name="hit">This class is not responsible to implement anything, it just sends info about raycast hit</param>
    public abstract void Act(CommandRecorder commandRecorder, SelectableAbstract anotherSelectable);
    public abstract void Move(CommandRecorder commandRecorder, Vector3 target);
    public virtual Sprite GetPortraitSprite()
    {
        return portraitSprite;
    }

    
}

