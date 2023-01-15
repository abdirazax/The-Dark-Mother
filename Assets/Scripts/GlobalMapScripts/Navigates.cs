using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(DrawsPathOfNavigates))]
/// <summary>
/// Responsible for Army's navigation on World Map's Navigation Map
/// </summary>
public class Navigates : MonoBehaviour, ISaveable 
{
    public DrawsPathOfNavigates DrawsPath { get; private set; }
    NavigatingStateIdle navigatingStateIdle = new NavigatingStateIdle();
    NavigatingStateMoving navigatingStateMoving = new NavigatingStateMoving();
    NavigatingStateAbstract currentNavigatingState;
    public event Action OnMovingStart, OnMovingEnd;
    public bool IsIdle { get { return currentNavigatingState == navigatingStateIdle; } }
    public bool IsMoving { get { return currentNavigatingState == navigatingStateMoving; } }
    #region Variables
    [field: SerializeField]
    float movementPoints;
    public float MovementPoints
    {
        get { return movementPoints; }
        private set
        {
            movementPoints = value;
            if (movementPoints < 0) movementPoints = 0;
        }
    }
    [field: SerializeField]
    public float MovementPointsMax { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }
    [SerializeField]
    Transform startHL, targetHL;
    public float RotationY
    {
        get { return transform.rotation.eulerAngles.y; }
        private set
        {
            Vector3 v = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(v.x, value, v.z);
        }
    }
    [SerializeField]
    public Vector3 initialPosition, finalDestination;
    public NavMeshPath SelectedPotentialPath { get; private set; }

    #endregion 
    private void Awake()
    {
        DrawsPath = GetComponent<DrawsPathOfNavigates>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        SelectedPotentialPath = new NavMeshPath();
        currentNavigatingState = navigatingStateIdle;
        TurnOffDefaultAutoRotationByNavMeshAgent();
        initialPosition = transform.position;
    }
    private void LateUpdate()
    {
        currentNavigatingState.LateUpdateState(this);
    }
    public void SetDestinationAndMove(Vector3 target)
    {
        finalDestination = target;
        SelectedPotentialPath = getPathToReachInThisTurn(target);
        if (MovementPoints == 0) return;
        MoveInPath(SelectedPotentialPath);
    }
    public void FinishMovement()
    {
        transform.position = NavMeshAgent.destination;
        MovementPoints-=GetPathLengthBetweenTwoPos(transform.position, initialPosition);
        initialPosition = transform.position;
        SwitchStateToIdleIfNotAlready();
    }
    public void StopRightThere()
    {
        NavMeshAgent.destination = transform.position;
        FinishMovement();
    }
    void SwitchStateToMovingIfNotAlready()
    {
        if (currentNavigatingState == navigatingStateMoving)
            return;
        currentNavigatingState.EndState(this);
        currentNavigatingState = navigatingStateMoving;
        currentNavigatingState.StartState(this);
    }
    void SwitchStateToIdleIfNotAlready()
    {
        if (currentNavigatingState == navigatingStateIdle)
            return;
        currentNavigatingState.EndState(this);
        currentNavigatingState = navigatingStateIdle;
        currentNavigatingState.StartState(this);
    }
    /// <summary>
    /// Moves Navigatable in ANY path, even teleports if the start of path is in another place
    /// </summary>
    /// <param name="path">Any calculated path</param>
    public void MoveInPath(NavMeshPath path)
    {
        if (path.corners.Length > 0)
        {
            transform.position = path.corners[0];
            NavMeshAgent.destination = path.corners[path.corners.Length - 1];
            SwitchStateToMovingIfNotAlready();
        }
    }
    public NavMeshPath getPathToReach(Vector3 targetPosition)
    {
        if (targetPosition == null) return new NavMeshPath();
        NavMeshPath localNavMeshPath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position,
            targetPosition, NavMesh.AllAreas, localNavMeshPath);
        return localNavMeshPath;
    }
    public NavMeshPath getPathToReachInThisTurn(Vector3 targetPosition)
    {
        if (targetPosition == null) return new NavMeshPath();
        NavMeshPath localNavMeshPath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position,
            targetPosition, NavMesh.AllAreas, localNavMeshPath);
        if (localNavMeshPath.corners.Length < 2)
            return localNavMeshPath;
        Vector3 lastPosition = localNavMeshPath.corners[0];
        Vector3 pointPosition;
        float localMovementPoints = MovementPoints;
        for (int i = 1; i < localNavMeshPath.corners.Length; i++)
        {
            pointPosition = new Vector3(
                localNavMeshPath.corners[i].x,
                localNavMeshPath.corners[i].y,
                localNavMeshPath.corners[i].z);
            if (localMovementPoints - Vector3.Distance(lastPosition, pointPosition) > 0)
            {
                localMovementPoints -= Vector3.Distance(lastPosition, pointPosition);
                lastPosition = pointPosition;
            }
            else
            {
                pointPosition = Utils.LerpByDistance(lastPosition, pointPosition, localMovementPoints);
                localMovementPoints = 0;
                lastPosition = pointPosition;
            }
        }
        NavMesh.CalculatePath(transform.position,
            lastPosition, NavMesh.AllAreas, localNavMeshPath);
        return localNavMeshPath;
    }
    public NavMeshPath getPathToReachInThisTurn(NavMeshPath globalPath)
    {
        if (globalPath.corners.Length < 2)
            return globalPath;
        NavMeshPath localNavMeshPath = new NavMeshPath();
        Vector3 lastPosition = globalPath.corners[0];
        Vector3 pointPosition;
        float localMovementPoints = MovementPoints;
        for (int i = 1; i < globalPath.corners.Length; i++)
        {
            pointPosition = new Vector3(
                globalPath.corners[i].x,
                globalPath.corners[i].y,
                globalPath.corners[i].z);
            if (localMovementPoints - Vector3.Distance(lastPosition, pointPosition) > 0)
            {
                localMovementPoints -= Vector3.Distance(lastPosition, pointPosition);
                lastPosition = pointPosition;
            }
            else
            {
                pointPosition = Utils.LerpByDistance(lastPosition, pointPosition, localMovementPoints);
                localMovementPoints = 0;
                lastPosition = pointPosition;
            }
        }
        NavMesh.CalculatePath(transform.position,
            lastPosition, NavMesh.AllAreas, localNavMeshPath);
        return localNavMeshPath;
    }
    
    float GetPathLengthBetweenTwoPos(Vector3 position1, Vector3 position2)
    {
        NavMeshPath tempPath = new NavMeshPath();
        NavMesh.CalculatePath(position1, position2, NavMesh.AllAreas, tempPath);
        return GetPathLength(tempPath);
    }
    float GetPathLength(NavMeshPath path)
    {
        float lengthTotal = 0;
        for(int i = 1; i < path.corners.Length; i++)
        {
            lengthTotal += Vector3.Distance(path.corners[i], path.corners[i - 1]);
        }
        return lengthTotal;
    }
    public void DebugPath(NavMeshPath path)
    {
        string s = path.corners.Length + ": ";
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            s += path.corners[i] + " | ";
        }
        Debug.Log(s);
    }
    private void TurnOffDefaultAutoRotationByNavMeshAgent()
    {
        NavMeshAgent.updateRotation = false;
    }
    private void AdjustRotationByVelocity()
    {

        if (NavMeshAgent.velocity.sqrMagnitude <= 0.05f)
            return;
        RotationY = Quaternion.LookRotation(NavMeshAgent.velocity.normalized).eulerAngles.y;
    }
    bool IsDestinationReached()
    {
        if (NavMeshAgent.pathPending)
            return false;
        if (NavMeshAgent.remainingDistance > NavMeshAgent.stoppingDistance)
            return false;
        if (NavMeshAgent.hasPath && NavMeshAgent.velocity.sqrMagnitude > 0f)
            return false;
        return true;
    }
    public void ReplenishMovementPoints()
    {
        MovementPoints = MovementPointsMax;
    }

    

    public class NavigatingStateMoving : NavigatingStateAbstract
    {
        public override void StartState(Navigates navigates) 
        {
            navigates.OnMovingStart?.Invoke();
        }
        public override void EndState(Navigates navigates)
        {
            navigates.OnMovingEnd?.Invoke();
        }
        public override void LateUpdateState(Navigates navigates)
        {
            if (navigates.IsDestinationReached())
            {
                navigates.FinishMovement();
                return;
            }
            
            navigates.AdjustRotationByVelocity();
        }
        public override void UpdateState(Navigates navigates) { }
    }
    public class NavigatingStateIdle : NavigatingStateAbstract
    {
        public override void StartState(Navigates navigates) { }
        public override void EndState(Navigates navigates) { }
        public override void LateUpdateState(Navigates navigates) { }
        public override void UpdateState(Navigates navigates) { }
    }
    public abstract class NavigatingStateAbstract
    {
        public abstract void StartState(Navigates navigates);
        public abstract void UpdateState(Navigates navigates);
        public abstract void LateUpdateState(Navigates navigates);
        public abstract void EndState(Navigates navigates);
    }
    
    public object SaveState()
    {
        return new SaveData(this);
    }
    public void LoadState(object state)
    {
        SaveData saveData = (SaveData)state;
        MovementPoints = saveData.movementPoints;
        MovementPointsMax = saveData.movementPointsMax;
        RotationY = saveData.rotationY;
        transform.position = new Vector3(saveData.transformPosition[0], saveData.transformPosition[1], saveData.transformPosition[2]);
        initialPosition = new Vector3(saveData.initialPosition[0], saveData.initialPosition[1], saveData.initialPosition[2]);
        finalDestination = new Vector3(saveData.finalDestination[0], saveData.finalDestination[1], saveData.finalDestination[2]);
    }
    [Serializable]
    struct SaveData
    {
        public float movementPoints;
        public float movementPointsMax;
        public float rotationY;
        public float[] transformPosition;
        public float[] initialPosition;
        public float[] finalDestination;
        public SaveData(Navigates navigates)
        {
            movementPoints = navigates.MovementPoints;
            movementPointsMax = navigates.MovementPointsMax;
            rotationY = navigates.RotationY;
            transformPosition = new float[3] { navigates.transform.position.x, navigates.transform.position.y, navigates.transform.position.z };
            initialPosition = new float[3] { navigates.initialPosition.x, navigates.initialPosition.y, navigates.initialPosition.z };
            finalDestination = new float[3] { navigates.finalDestination.x, navigates.finalDestination.y, navigates.finalDestination.z };
        }
    }
}



