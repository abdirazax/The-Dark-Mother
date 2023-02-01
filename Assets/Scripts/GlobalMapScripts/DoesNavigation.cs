using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Navigates))]
[RequireComponent(typeof(DrawsPathOfNavigates))]
[RequireComponent(typeof(NavMeshAgent))]
public class DoesNavigation : MonoBehaviour
{

    public NavMeshPath SelectedPotentialPath { get; private set; }
    public Navigates Navigates { get; private set; }
    public DrawsPathOfNavigates DrawsPath { get; private set; }
    public NavMeshAgent NavAgent { get; private set; }

    NavigatingStateAbstract currentNavigatingState;
    #region NavigatingStates
    NavigatingStateIdle navigatingStateIdle = new NavigatingStateIdle();
    NavigatingStateMoving navigatingStateMoving = new NavigatingStateMoving();
    public bool IsIdle { get { return currentNavigatingState == navigatingStateIdle; } }
    public bool IsMoving { get { return currentNavigatingState == navigatingStateMoving; } }
    #endregion



    Vector3 initialPosition, destinationPositionInThisTurn;
    public Vector3 GlobalDestinationPosition { get {
            if (Navigates == null) return new Vector3();
            return Navigates.globalDestinationPosition; } }
    
    private void Awake()
    {
        Initialize();
    }
    private void OnEnable()
    {
        Initialize();
    }
    public void Initialize()
    {
        Navigates = GetComponent<Navigates>();
        DrawsPath = GetComponent<DrawsPathOfNavigates>();
        NavAgent = GetComponent<NavMeshAgent>();
        currentNavigatingState = navigatingStateIdle;
        TurnOffDefaultAutoRotationByNavMeshAgent();
        Stop();
    }

    private void LateUpdate()
    {
        currentNavigatingState.LateUpdateState(this);
    }
    public void ReactToNewDestinationSet(Vector3 target)
    {
        Stop();
        StartMovingTo(target);
    }
    void StartMovingTo(Vector3 target)
    {
        initialPosition = transform.position;
        SelectedPotentialPath = getPathToReachInThisTurn(target);
        destinationPositionInThisTurn = SelectedPotentialPath.corners[SelectedPotentialPath.corners.Length - 1];
        if (Navigates.MovementPoints == 0) return;
        MoveInPath(SelectedPotentialPath);
    }
    public void Stop()
    {
        destinationPositionInThisTurn = transform.position;
        FinishMovement();
    }
    public void FinishMovement()
    {
        Navigates.AddMovementPoints(-GetPathLengthBetweenTwoPos(destinationPositionInThisTurn, initialPosition));
        destinationPositionInThisTurn = new Vector3();
        NavAgent.isStopped = true;
        NavAgent.ResetPath();
        SwitchStateToIdleIfNotAlready();
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
            NavAgent.destination = path.corners[path.corners.Length - 1];
            SwitchStateToMovingIfNotAlready();
        }
    }

    public NavMeshPath getPathToReach(Vector3 targetPosition)
    {
        if (targetPosition == null) return new NavMeshPath();
        NavMeshPath temporaryNavMeshPath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position,
            targetPosition, NavMesh.AllAreas, temporaryNavMeshPath);
        return temporaryNavMeshPath;
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
        float localMovementPoints = Navigates.MovementPoints;
        for (int i = 1; i < localNavMeshPath.corners.Length; i++)
        {
            pointPosition = new Vector3(localNavMeshPath.corners[i].x, localNavMeshPath.corners[i].y, localNavMeshPath.corners[i].z);
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
        float localMovementPoints = Navigates.MovementPoints;
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
        for (int i = 1; i < path.corners.Length; i++)
        {
            lengthTotal += Vector3.Distance(path.corners[i], path.corners[i - 1]);
        }
        return lengthTotal;
    }
    


    private void TurnOffDefaultAutoRotationByNavMeshAgent()
    {
        NavAgent.updateRotation = false;
    }
    private void AdjustRotationByVelocity()
    {
        if (NavAgent.velocity.sqrMagnitude <= 0.05f)
            return;
        Navigates.RotationY = Quaternion.LookRotation(NavAgent.velocity.normalized).eulerAngles.y;
    }
    bool IsDestinationReached()
    {
        if (NavAgent.pathPending)
            return false;
        if (NavAgent.remainingDistance > NavAgent.stoppingDistance)
            return false;
        if (NavAgent.hasPath && NavAgent.velocity.sqrMagnitude > 0f)
            return false;
        return true;
    }

    public class NavigatingStateMoving : NavigatingStateAbstract
    {
        public override void StartState(DoesNavigation doesNavigation)
        {
            doesNavigation.DrawsPath?.ReactToStartMoving();
        }
        public override void EndState(DoesNavigation doesNavigation)
        {
            doesNavigation.DrawsPath?.ReactToEndMoving();
        }
        public override void LateUpdateState(DoesNavigation doesNavigation)
        {
                if (doesNavigation.IsDestinationReached())
                {
                    doesNavigation.FinishMovement();
                    return;
                }
            doesNavigation.AdjustRotationByVelocity();
        }
        public override void UpdateState(DoesNavigation doesNavigation) { }
    }
    public class NavigatingStateIdle : NavigatingStateAbstract
    {
        public override void StartState(DoesNavigation doesNavigation) { }
        public override void EndState(DoesNavigation doesNavigation) { }
        public override void LateUpdateState(DoesNavigation doesNavigation) { }
        public override void UpdateState(DoesNavigation doesNavigation) { }
    }
    public abstract class NavigatingStateAbstract
    {
        public abstract void StartState(DoesNavigation doesNavigation);
        public abstract void EndState(DoesNavigation doesNavigation);
        public abstract void UpdateState(DoesNavigation doesNavigation);
        public abstract void LateUpdateState(DoesNavigation doesNavigation);
    }

}



