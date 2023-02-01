using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using CodeMonkey;
public class CameraPan : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float panSpeedSide = 0.03f;
    [SerializeField]
    private float panSpeedForward = 0.02f;
    [SerializeField]
    private float inputPanSpeedInfluence = 1f;
    [SerializeField]
    private float cameraMinHeight = 1f;
    [SerializeField]
    private float cameraMaxHeight = 10f;
    private CinemachineVirtualCamera virtualCamera;
    private Transform cameraTransformParent, cameraTransform;
    [SerializeField]
    private float cameraMinX, cameraMaxX, cameraMinZ, cameraMaxZ;
    #endregion
    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cameraTransform = virtualCamera.VirtualCameraGameObject.transform;
        cameraTransformParent = cameraTransform.parent;
    }
    
    public void PanScreen(float inputX, float inputY, float inputPanSpeed)
    {
        inputPanSpeed = Utils.Remap(inputPanSpeed, 0, 1, 1, 1 + inputPanSpeedInfluence);
        Vector3 bothCameraPositionsCombined = new Vector3(cameraTransformParent.position.x, 
                                                            cameraTransform.position.y, 
                                                            cameraTransformParent.position.z);
        
        bothCameraPositionsCombined = Vector3.Lerp(bothCameraPositionsCombined,
            bothCameraPositionsCombined +
            (cameraTransform.forward * inputY * panSpeedForward +
            cameraTransformParent.right * inputX * panSpeedSide)
            * inputPanSpeed * (1 + Mathf.Log(cameraTransform.position.y)) * virtualCamera.m_Lens.FieldOfView, Time.deltaTime);

        bothCameraPositionsCombined = Utils.ClampYAxis(bothCameraPositionsCombined, cameraMinHeight, cameraMaxHeight);
        bothCameraPositionsCombined = CamPositionLimited(bothCameraPositionsCombined);
        cameraTransformParent.position = new Vector3(bothCameraPositionsCombined.x, 
                                                    cameraTransformParent.position.y,  
                                                    bothCameraPositionsCombined.z);
        cameraTransform.position = new Vector3(cameraTransform.position.x, 
                                                bothCameraPositionsCombined.y,
                                                cameraTransform.position.z);
    }


    public void PanScreenAcrossXZ(float inputX, float inputY, float inputPanSpeed)
    {
        inputPanSpeed = Utils.Remap(inputPanSpeed, 0, 1, 1, 1 + inputPanSpeedInfluence);
        Vector3 positionToLerpTo =
            cameraTransformParent.position +
            (cameraTransformParent.forward * inputY * panSpeedForward +
            cameraTransformParent.right * inputX * panSpeedSide) *
            inputPanSpeed * 
            (1 + Mathf.Log(cameraTransform.position.y)) * 
            virtualCamera.m_Lens.FieldOfView;
        positionToLerpTo = CamPositionLimited(positionToLerpTo);
        cameraTransformParent.position = Vector3.Lerp(cameraTransformParent.position, positionToLerpTo, Time.deltaTime);
    }

    Vector3 CamPositionLimited(Vector3 initialPosition)
    {
        float newX = initialPosition.x;
        float newZ = initialPosition.z;
        if (newX > cameraMaxX) newX = cameraMaxX;
        else if (newX < cameraMinX) newX = cameraMinX;
        if (newZ > cameraMaxZ) newZ = cameraMaxZ;
        else if (newZ < cameraMinZ) newZ = cameraMinZ;
        return new Vector3(newX, initialPosition.y, newZ);
    }

}
