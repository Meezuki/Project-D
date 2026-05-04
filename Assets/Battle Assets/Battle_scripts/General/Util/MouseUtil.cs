
using UnityEngine;

public static class MouseUtil  
{
    private static Camera camera = Camera.main;
    public static Vector3 GetMousePositionInWorldSpace(float zValue = 0f)
    {
        Plane dragPlane = new(Camera.main.transform.forward, new Vector3 (0, 0, zValue));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
            
        }
        return Vector3.zero;
    }
    
}