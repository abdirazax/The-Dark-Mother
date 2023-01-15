using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSelectorInputManager : MonoBehaviour
{
    #region Variables
    private PlayerInput playerInput;
    private InputAction selectAction, actionAction;
    public InputAction MousePositionAction { get; private set; }
    private PlayerSelectorAbstract playerSelector;
    #endregion
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerSelector = GetComponent<PlayerSelectorAbstract>();
        selectAction = playerInput.actions["MouseSelect"];
        actionAction = playerInput.actions["MouseAction"];
        MousePositionAction = playerInput.actions["MousePosition"];
    }
    private void OnEnable()
    {
        selectAction.started += SelectStart;
        selectAction.canceled += SelectEnd;
        actionAction.started += actionStart;
        actionAction.canceled += actionEnd;
    }
    private void OnDisable()
    {
        selectAction.started -= SelectStart;
        selectAction.canceled -= SelectEnd;
        actionAction.started -= actionStart;
        actionAction.canceled -= actionEnd;
    }

    private void SelectStart(InputAction.CallbackContext context)
    {
        playerSelector.SelectionStart(MousePositionAction);
    }
    private void SelectEnd(InputAction.CallbackContext context)
    {
        playerSelector.SelectionEnd(MousePositionAction);
    }
    private void actionStart(InputAction.CallbackContext obj)
    {
        playerSelector.ActionStart(MousePositionAction);
    }
    private void actionEnd(InputAction.CallbackContext obj)
    {
        playerSelector.ActionEnd(MousePositionAction);
    }


}
