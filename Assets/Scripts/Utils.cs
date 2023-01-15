using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public static class Utils
{
    /// <summary>
    /// Re-map a float from one range to another
    /// </summary>
    /// <param name="value">float that will be remapped</param>
    /// <param name="from1">Range 1 begin</param>
    /// <param name="to1">Range 1 end</param>
    /// <param name="from2">Range 2 begin</param>
    /// <param name="to2">Range 2 end</param>
    /// <returns>Returns remapped float</returns>
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    /// <summary>
    /// Gets rid of Y-axis and returns 2D normalized direction across XZ-surface
    /// </summary>
    /// <returns>Normalized 2D direction across XZ-surface</returns>
    public static Vector3 ForwardDirectionAcrossXZ(Vector3 forwardDirection)
    {
        forwardDirection.y = 0;
        forwardDirection.Normalize();
        return forwardDirection;
    }

    /// <summary>
    /// Limits Y-axis in a vector to some range
    /// </summary>
    /// <returns>Vector with limited Y-axis value</returns>
    public static Vector3 ClampYAxis(Vector3 vector,float min, float max)
    {
        float yAllowed = Mathf.Clamp(vector.y, min, max);
        return new Vector3(vector.x, yAllowed, vector.z);

        
    }

    public static float CalculateNavMeshPathLength(NavMeshAgent navMeshAgent,Vector3 startPosition,Vector3 targetPosition)
    {
        // Create a path and set it based on a target position.
        NavMeshPath path = new NavMeshPath();
        if (navMeshAgent.enabled)
            navMeshAgent.CalculatePath(targetPosition, path);

        // Create an array of points which is the length of the number of corners in the path + 2.
        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        // The first point is the enemy's position.
        allWayPoints[0] = startPosition ;

        // The last point is the target position.
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        // The points inbetween are the corners of the path.
        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        // Create a float to store the path length that is by default 0.
        float pathLength = 0;

        // Increment the path length by an amount equal to the distance between each waypoint and the next.
        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }
    public static Vector3 LerpByDistance(Vector3 A, Vector3 B, float x)
    {
        Vector3 P = x * Vector3.Normalize(B - A) + A;
        return P;
    }

    public static bool IsMouseOverUI(InputAction mousePositionInputAction)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = mousePositionInputAction.ReadValue<Vector2>();
        List<RaycastResult> raycastResultsList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);
        bool mouseOverUI = false;
        for (int i = 0; i < raycastResultsList.Count; i++)
        {
            if (raycastResultsList[i].gameObject.GetType() == typeof(GameObject))
            {
                mouseOverUI = true;
                break;
            }
        }
        return mouseOverUI;
    }

    public static string DebugArray(int[,] array)
    {
        string s = "";
        for(int i = 0; i < array.GetLength(0); i++)
        {
            for(int j = 0; j < array.GetLength(1); j++)
            {
                s += array[i, j]+" ";
            }
            s += "\n";
        }
        return s;
    }

}
