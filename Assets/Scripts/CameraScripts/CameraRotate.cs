using UnityEngine;
using Cinemachine;

public class CameraRotate : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float rotateSpeed = 30f;
    
    private CinemachineVirtualCamera virtualCamera;
    private Transform cameraTransform, cameraTransformParent;
    #endregion

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cameraTransform = virtualCamera.VirtualCameraGameObject.transform;
        cameraTransformParent = cameraTransform.parent;
    }
    public void RotateScreen(float inputRotateX, float inputRotateY)
    {
        Quaternion camCurrentRotation = cameraTransformParent.rotation;
        cameraTransformParent.rotation *= Quaternion.Euler(new Vector3(0, inputRotateX, 0));
        cameraTransform.rotation *= Quaternion.Euler(new Vector3(-inputRotateY, 0, 0));
    }
}
