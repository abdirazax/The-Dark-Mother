using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Responsible for Army's navigation on World Map's Navigation Map
/// </summary>
[RequireComponent(typeof(DoesNavigation))]
public class Navigates : MonoBehaviour
{
    #region Variables

    public DoesNavigation DoesNavigation { get; private set; }
    [field: SerializeField]
    float movementPoints;
    public float MovementPoints
    {
        get { return movementPoints; }
        private set{
            movementPoints = value;
            if (movementPoints < 0) movementPoints = 0;
        }
    }
    [field: SerializeField]
    public float MovementPointsMax { get; private set; }
    /*[SerializeField]
    Transform startPositionHighlight, targetPositionHighlight;*/
    public float RotationY
    {
        get { return transform.rotation.eulerAngles.y; }
        set
        {
            Vector3 v = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(v.x, value, v.z);
        }
    }
    [SerializeField]
    public Vector3  globalDestinationPosition;
    #endregion 
    private void Awake()
    {
        Initialize();
    }
    private void OnEnable()
    {
        Initialize();
    }
    void Initialize()
    {
        DoesNavigation = GetComponent<DoesNavigation>();
        DoesNavigation.Initialize();
    }
    public void SetNewDestination(Vector3 target)
    {
        globalDestinationPosition = target;
        DoesNavigation.ReactToNewDestinationSet(target);

    }
    public void CancelDestination()
    {
        globalDestinationPosition = transform.position;
        DoesNavigation.FinishMovement();
    }
    public void ReplenishMovementPoints()
    {
        MovementPoints = MovementPointsMax;
    }
    public void AddMovementPoints(float changeValue)
    {
        MovementPoints += changeValue;
    }
    public void LoadState(NavigatesSaveData newNavigatesData)
    {
        MovementPointsMax = newNavigatesData.movementPointsMax;
        MovementPoints = newNavigatesData.movementPoints;
        RotationY = newNavigatesData.rotationY;
        transform.position = new Vector3(newNavigatesData.transformPosition[0], newNavigatesData.transformPosition[1], newNavigatesData.transformPosition[2]);
        globalDestinationPosition = new Vector3(newNavigatesData.globalDestinationPosition[0], newNavigatesData.globalDestinationPosition[1], newNavigatesData.globalDestinationPosition[2]);
        Initialize();
    }
}



[Serializable]
public struct NavigatesSaveData
{
    public float movementPoints;
    public float movementPointsMax;
    public float rotationY;
    public float[] transformPosition;
    public float[] globalDestinationPosition;
    public NavigatesSaveData(Navigates navigates)
    {
        movementPoints = navigates.MovementPoints;
        movementPointsMax = navigates.MovementPointsMax;
        rotationY = navigates.RotationY;
        transformPosition = new float[3] { navigates.transform.position.x, navigates.transform.position.y, navigates.transform.position.z };
        globalDestinationPosition = new float[3] { navigates.globalDestinationPosition.x, navigates.globalDestinationPosition.y, navigates.globalDestinationPosition.z };
    }
}



