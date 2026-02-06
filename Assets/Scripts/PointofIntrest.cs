using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    [Header("Connections")]
    public List<PointOfInterest> NextPointsOfInterest = new List<PointOfInterest>();
    
    [Header("Point Settings")]
    public string pointName = "Point";
    public PointType pointType = PointType.Normal;
    
    [Header("Visuals")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material specialMaterial;
    
    public enum PointType
    {
        Normal,
        Special,
        Event,
        Loot,
        Start,
        End
    }
    
 public void UpdateVisuals()
{
    if (meshRenderer == null)
        meshRenderer = GetComponent<MeshRenderer>();
        
    if (meshRenderer != null)
    {
        switch (pointType)
        {
            case PointType.Special:
                if (specialMaterial != null)
                    meshRenderer.material = specialMaterial;
                break;
            case PointType.Start:
                meshRenderer.material.color = Color.green;
                break;
            case PointType.End:
                meshRenderer.material.color = Color.red;
                break;
            case PointType.Loot:        // Kasus baru untuk Loot
                meshRenderer.material.color = Color.yellow;  // Kuning untuk loot
                break;
            case PointType.Event:       // Kasus baru untuk Event
                meshRenderer.material.color = Color.cyan;    // Cyan untuk event
                break;
            default:  // Normal
                if (normalMaterial != null)
                    meshRenderer.material = normalMaterial;
                break;
        }
    }
}
    
    private void OnDrawGizmos()
    {
        // Draw connections in Scene view
        foreach (var nextPoint in NextPointsOfInterest)
        {
            if (nextPoint != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, nextPoint.transform.position);
                
                // Draw arrow
                Vector3 dir = (nextPoint.transform.position - transform.position).normalized;
                float arrowSize = 0.2f;
                Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 160, 0) * Vector3.forward;
                Vector3 left = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 200, 0) * Vector3.forward;
                Gizmos.DrawRay(nextPoint.transform.position - dir * 0.5f, right * arrowSize);
                Gizmos.DrawRay(nextPoint.transform.position - dir * 0.5f, left * arrowSize);
            }
        }
        
        // Draw point sphere
        Gizmos.color = GetGizmoColor();
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
    
    private Color GetGizmoColor()
    {
        switch (pointType)
        {
            case PointType.Special: return Color.magenta;
            case PointType.Start: return Color.green;
            case PointType.End: return Color.red;
            case PointType.Loot: return Color.yellow;   
            case PointType.Event: return Color.cyan;     
            default: return Color.blue;
        }
    }
}