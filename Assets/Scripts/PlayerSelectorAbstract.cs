using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Manages Selectables that the player interacts with.
/// Reacts to commands from PlayerSelectorInputManager class.
/// </summary>
public abstract class PlayerSelectorAbstract : MonoBehaviour
{
    /// <summary>
    /// When Player clicks and holds selection key
    /// </summary>
    /// <param name="mousePositionAction">2D mouse position on Screen</param>
    public virtual void SelectionStart(InputAction mousePositionAction)
    {

    }
    /// <summary>
    /// When Player releases selection key
    /// </summary>
    /// <param name="mousePositionAction">2D mouse position on Screen</param>
    public virtual void SelectionEnd(InputAction mousePositionAction)
    {

    }
    /// <summary>
    /// When Player clicks and holds action key
    /// </summary>
    /// <param name="mousePositionAction">2D mouse position on Screen</param>
    public virtual void ActionStart(InputAction mousePositionAction)
    {

    }
    /// <summary>
    /// When Player releases action key
    /// </summary>
    /// <param name="mousePositionAction">2D mouse position on Screen</param>
    public virtual void ActionEnd(InputAction mousePositionAction)
    {

    }

}
