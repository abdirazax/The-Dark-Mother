using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSelectionInputManager : MonoBehaviour
{
    #region Variables
    private PlayerInput playerInput;
    private InputAction selectAction, mousePositionAction;
    private PlayerSelection playerSelection;
    #endregion
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerSelection = GetComponent<PlayerSelection>();
        selectAction = playerInput.actions["Select"];
        mousePositionAction = playerInput.actions["MousePosition"];
    }
    private void OnEnable()
    {
        selectAction.started += SelectStart;
        selectAction.canceled += SelectEnd;
    }
    private void OnDisable()
    {
        selectAction.started -= SelectStart;
        selectAction.canceled -= SelectEnd;
    }

    private void SelectStart(InputAction.CallbackContext context)
    {
        playerSelection.SelectObjectStart(mousePositionAction.ReadValue<Vector2>());
    }
    private void SelectEnd(InputAction.CallbackContext context)
    {
        playerSelection.SelectObjectEnd(mousePositionAction.ReadValue<Vector2>());
    }

}
