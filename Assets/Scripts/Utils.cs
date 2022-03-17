using UnityEngine;
public static class Utils
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


    public static Vector3 ForwardDirectionAcrossXZ(Vector3 forwardDirection)
    {
        forwardDirection.y = 0;
        forwardDirection.Normalize();
        return forwardDirection;
    }

    public static Vector3 ClampYAxis(Vector3 vector,float min, float max)
    {
        float yAllowed = Mathf.Clamp(vector.y, min, max);
        return new Vector3(vector.x, yAllowed, vector.z);

        
    }
}
