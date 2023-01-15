using UnityEngine;
using Cinemachine;

public class CameraRotate : MonoBehaviour
{
    #region Variables
    [SerializeField]
    [Range(0.0f, 3.0f)]
    private float rotateSpeed = 1f;
    
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
        cameraTransformParent.rotation *= Quaternion.Euler(new Vector3(0, inputRotateX * rotateSpeed, 0));
        cameraTransform.rotation *= Quaternion.Euler(new Vector3(-inputRotateY * rotateSpeed, 0, 0));
    }
}
