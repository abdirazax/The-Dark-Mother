using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInputManager : MonoBehaviour
{
    #region Variables
    private CameraPan cameraPan;
    private AbstractCameraZoom cameraZoom;
    private CameraRotate cameraRotate;
    private PlayerInput playerInput;
    private InputAction moveAction, mousePositionAction, mouseMiddleAction, lookAction, zoomAction, rotateAction, shiftAction;

    #endregion
    private void Awake()
    {
        cameraPan = GetComponent<CameraPan>();
        cameraZoom = GetComponent<AbstractCameraZoom>();
        cameraRotate = GetComponent<CameraRotate>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        mousePositionAction = playerInput.actions["MousePosition"];
        mouseMiddleAction = playerInput.actions["MouseMiddle"];
        lookAction = playerInput.actions["Look"];
        zoomAction = playerInput.actions["Zoom"];
        rotateAction = playerInput.actions["Rotate"];
        shiftAction = playerInput.actions["Shift"];
    }



    void Update()
    {
        HandleZoomInput();
        HandleCursorOnEdgeInput();
        HandleMoveInput();
        HandleRotateInput();
        HandleMouseRotateInput();
    }

    private void HandleMouseRotateInput()
    {
        if (GetPlayerMouseMiddle() == 0)
            return;
        Vector2 deltaPointer = GetPlayerLook();
        cameraRotate.RotateScreen(deltaPointer.x, deltaPointer.y);
    }

    private void HandleRotateInput()
    {
        float playerRotateInput = GetPlayerRotate();
        if (playerRotateInput == 0)
            return;
        cameraRotate.RotateScreen(playerRotateInput,0);
        return;
    }

    private void HandleMoveInput()
    {
        Vector2 playerMoveInput = GetPlayerMove();
        if (playerMoveInput == Vector2.zero)
            return;
        if (GetPlayerMouseMiddle() == 0)
        {
            cameraPan.PanScreenAcrossXZ(playerMoveInput.x, playerMoveInput.y, shiftAction.ReadValue<float>());
            return;
        }
        cameraPan.PanScreen(playerMoveInput.x, playerMoveInput.y, shiftAction.ReadValue<float>());
    }
    private void HandleZoomInput()
    {
        float playerZoomInput = GetPlayerZoom();
        if (playerZoomInput==0)
            return;
        cameraZoom.ZoomScreen(playerZoomInput);
    }
    private void HandleCursorOnEdgeInput()
    {
        if (GetPlayerMouseMiddle() != 0)
            return;
        float inputX = CursorOnScreenEdgeDirectionX(GetPlayerMousePosition().x);
        float inputY = CursorOnScreenEdgeDirectionY(GetPlayerMousePosition().y);
        if (inputX == 0 && inputY == 0) 
            return;
        cameraPan.PanScreenAcrossXZ(inputX, inputY, 0);
    }
    private int CursorOnScreenEdgeDirectionX(float inputX)
    {
        if (inputX >= Screen.width * .98f && inputX <= Screen.width)
            return 1;
        if (inputX <= Screen.width * .02f && inputX >= 0)
            return -1;
        return 0;
    }
    private int CursorOnScreenEdgeDirectionY(float inputY)
    {
        if (inputY >= Screen.height * .98f && inputY <= Screen.height)
            return 1;
        if (inputY <= Screen.height * .02f && inputY >= 0)
            return -1;
        return 0;
    }

    private Vector2 GetPlayerLook()
    {
        if (lookAction == null)
            return Vector2.zero;
        return lookAction.ReadValue<Vector2>();
    }
    private float GetPlayerRotate()
    {
        if (rotateAction == null)
            return 0;
        return rotateAction.ReadValue<float>();
    }
    private float GetPlayerMouseMiddle()
    {
        if (mouseMiddleAction == null)
            return 0;
        return mouseMiddleAction.ReadValue<float>();
    }
    private Vector2 GetPlayerMousePosition()
    {
        if (mousePositionAction.ReadValue<Vector2>() == null)
            return Vector2.zero;
        return mousePositionAction.ReadValue<Vector2>();
    }
    private Vector2 GetPlayerMove()
    {
        if (moveAction.ReadValue<Vector2>() == null)
            return Vector2.zero;
        return moveAction.ReadValue<Vector2>();
    }
    private float GetPlayerZoom()
    {
        if (zoomAction == null)
            return 0;
        return zoomAction.ReadValue<float>();
    }

}
