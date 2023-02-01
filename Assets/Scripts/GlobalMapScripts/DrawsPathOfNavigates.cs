using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(DoesNavigation))]
[RequireComponent(typeof(LineRenderer))]
public class DrawsPathOfNavigates : MonoBehaviour
{
    #region Variables
    DrawsPathStateAbstract currentState;
    DrawsPathStateDrawing drawingState = new DrawsPathStateDrawing();
    DrawsPathStateNotDrawing notDrawingState = new DrawsPathStateNotDrawing();
    private DoesNavigation doesNavigation;
    public LineRenderer LineRenderer { get; private set; }
    #endregion
    private void OnEnable()
    {
        Initialize();
    }
    private void OnDisable()
    {
        EndDrawingPath();
    }
    private void Awake()
    {
        Initialize();
    }
    void Initialize()
    {
        doesNavigation = GetComponent<DoesNavigation>();
        currentState = notDrawingState;
        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.startWidth = 0.3f;
        LineRenderer.endWidth = 0.3f;
        LineRenderer.positionCount = 0;
    }
    public void ReactToStartMoving()
    {
        StartDrawingPath();
    }
    void StartDrawingPath()
    {
        currentState = drawingState;
    }
    public void ReactToEndMoving()
    {
        EndDrawingPath();
    }
    void EndDrawingPath()
    {
        HideDrawnPath();
        currentState = notDrawingState;
    }
    void LateUpdate()
    {
        currentState.LateUpdateState(this);
    }

    void DrawPotentialPathOfSelectedNavigatorToTarget()
    {
        NavMeshPath globalPath = doesNavigation.getPathToReach(doesNavigation.GlobalDestinationPosition);
        NavMeshPath pathInThisTurn = doesNavigation.getPathToReachInThisTurn(globalPath);
        DrawPotentialPath(globalPath);
    }
    void DrawPotentialPath(NavMeshPath pathToDraw)
    {
        if (pathToDraw.corners.Length < 2)
            return;
        LineRenderer.positionCount = pathToDraw.corners.Length;
        for (int i = 0; i < pathToDraw.corners.Length; i++)
        {
            SetLineRendererPos(i, pathToDraw.corners[i]);
        }
    }
    void HideDrawnPath()
    {
        LineRenderer.positionCount = 0;
    }
    void SetLineRendererPos(int pointIndex, Vector3 position)
    {
        if (pointIndex >= LineRenderer.positionCount)
            return;
        LineRenderer.SetPosition(pointIndex, position + Vector3.up * .05f);
    }
    


    private class DrawsPathStateDrawing : DrawsPathStateAbstract
    {
        private readonly string terrainToMoveOnTag = "TerrainToMoveOn";
        public override void LateUpdateState(DrawsPathOfNavigates drawPathManager)
        {
            drawPathManager.DrawPotentialPathOfSelectedNavigatorToTarget();
        }
    }
    private class DrawsPathStateNotDrawing : DrawsPathStateAbstract
    {
        public override void LateUpdateState(DrawsPathOfNavigates pathDrawer) { }
    }
    private abstract class DrawsPathStateAbstract
    {
        public abstract void LateUpdateState(DrawsPathOfNavigates pathDrawer);
    }
}
