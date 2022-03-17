using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFoVZoom : CameraZoom
{
    #region Variables
    [SerializeField]
    private float zoomSpeed = 30f;
    [SerializeField]
    private float zoomInMax = 40f;
    [SerializeField]
    private float zoomOutMax = 90f;

    private CinemachineVirtualCamera virtualCamera;
    #endregion

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    public override void ZoomScreen(float inputZ)
    {
        float fieldOfView = virtualCamera.m_Lens.FieldOfView;
        float target = Mathf.Clamp(fieldOfView - inputZ, zoomInMax, zoomOutMax);
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(fieldOfView, target, zoomSpeed * Time.deltaTime);
    }
}
