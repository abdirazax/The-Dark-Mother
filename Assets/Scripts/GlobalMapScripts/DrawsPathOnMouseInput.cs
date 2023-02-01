using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
/// <summary>
/// Get commands mainly from SelectionManager.cs.
/// Responsible for drawing path of navigatable objects.
/// Has states that decide whether to draw or not on LateUpdate
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class DrawsPathOnMouseInput : MonoBehaviour
{
    #region Variables
    [SerializeField]
    float maxDistanceToTryFindNavMeshAround = 20f;
    DrawsPathStateAbstract currentState;
    DrawsPathStateDrawing drawingState = new DrawsPathStateDrawing();
    DrawsPathStateNotDrawing notDrawingState = new DrawsPathStateNotDrawing();
    private InputAction mousePositionInputAction;
    private Navigates navigator;
    /*public Vector3 lastMousePositionOnTerrain;*/
    public LineRenderer LineRenderer { get; private set; }
    #endregion
    private void Awake()
    {
        currentState = notDrawingState;
        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.startWidth = 0.3f;
        LineRenderer.endWidth = 0.3f;
        LineRenderer.positionCount = 0;
    }
    public void StartDrawingPathOf(Navigates navigatingObject,InputAction mousePositionInputAction, float maxDistanceToTryFindNavMeshAround)
    {
        this.maxDistanceToTryFindNavMeshAround = maxDistanceToTryFindNavMeshAround;
        navigator = navigatingObject;
        this.mousePositionInputAction = mousePositionInputAction;
        currentState = drawingState;
    }
    public void StopDrawing()
    {
        currentState = notDrawingState;
        HideDrawnPath();
    }
    void LateUpdate()
    {
        currentState.LateUpdateState(this);
    }

    void DrawPotentialPathOfSelectedNavigatorToTarget(Vector3 target)
    {
        NavMeshHit closestNavMeshHit;
        if (NavMesh.SamplePosition(target, out closestNavMeshHit, maxDistanceToTryFindNavMeshAround, NavMesh.AllAreas))
        {
            NavMeshPath globalPath = navigator.DoesNavigation.getPathToReach(closestNavMeshHit.position);
            NavMeshPath pathInThisTurn = navigator.DoesNavigation.getPathToReachInThisTurn(closestNavMeshHit.position);
            DrawPotentialPath(globalPath);
        }
    }

    void SetLineRendererPos(int pointIndex, Vector3 position)
    {
        if (pointIndex >= LineRenderer.positionCount)
            return;
        LineRenderer.SetPosition(pointIndex, position + Vector3.up * .05f);
    }
    public void DrawPotentialPath(NavMeshPath pathToDraw)
    {
        if (pathToDraw.corners.Length < 2)
            return;
        LineRenderer.positionCount = pathToDraw.corners.Length;
        for (int i = 0; i < pathToDraw.corners.Length; i++)
        {
            SetLineRendererPos(i, pathToDraw.corners[i]);
        }
    }
    public void HideDrawnPath()
    {
        LineRenderer.positionCount = 0;
    }

    private class DrawsPathStateDrawing:DrawsPathStateAbstract
    {

        private readonly string terrainToMoveOnTag = "TerrainToMoveOn";
        public override void StartState(DrawsPathOnMouseInput drawPathManager) { }
        public override void LateUpdateState(DrawsPathOnMouseInput drawPathManager)
        {
            Ray ray = Camera.main.ScreenPointToRay(drawPathManager.mousePositionInputAction.ReadValue<Vector2>());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(Utils.IsMouseOverUI(drawPathManager.mousePositionInputAction))
                    return;
                if (!hit.collider.CompareTag(terrainToMoveOnTag))
                    return;
                drawPathManager.DrawPotentialPathOfSelectedNavigatorToTarget(hit.point);
            }
        }
    }
    private abstract class DrawsPathStateAbstract
    {
        public abstract void StartState(DrawsPathOnMouseInput pathDrawer);
        public abstract void LateUpdateState(DrawsPathOnMouseInput pathDrawer);
    }
    private class DrawsPathStateNotDrawing:DrawsPathStateAbstract
    {
        public override void StartState(DrawsPathOnMouseInput drawPathManager) { }
        public override void LateUpdateState(DrawsPathOnMouseInput pathDrawer) { }
    }
}
