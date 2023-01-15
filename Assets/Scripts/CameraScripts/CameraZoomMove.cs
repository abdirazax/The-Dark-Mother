using UnityEngine;
using Cinemachine;

public class CameraZoomMove : AbstractCameraZoom
{
    #region Variables
    [SerializeField]
    private float zoomSpeed = 30f;
    [SerializeField]
    private float zoomInMax = 40f;
    [SerializeField]
    private float zoomOutMax = 90f;

    private CinemachineVirtualCamera virtualCamera;
    private Transform cameraTransform;
    #endregion

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cameraTransform = virtualCamera.VirtualCameraGameObject.transform;
    }
    public override void ZoomScreen(float inputZ)
    {
        float camCurrentYPos = cameraTransform.position.y;
        float target = Mathf.Clamp(camCurrentYPos - inputZ, zoomInMax, zoomOutMax);

        cameraTransform.position = new Vector3(cameraTransform.position.x,
            Mathf.Lerp(camCurrentYPos, target, zoomSpeed * Time.deltaTime),
                                                cameraTransform.position.z);
    }


}
