using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class ManualTargetSystem : Singleton<ManualTargetSystem>
{
    [SerializeField] private ArrowView arrowView;
    [SerializeField] private LayerMask targetLayerMask;

    public void StartTargeting(Vector3 startPosition)
    {
        arrowView.gameObject.SetActive(true);
        arrowView.SetupArrow(startPosition);

    }

    //public EnemyView EndTargeting(Vector3 endPosition)
    //{
    //    Debug.Log("ending targeting");
    //    arrowView.gameObject.SetActive(false);
    //    if (Physics.Raycast(endPosition, Vector3.forward, out RaycastHit hit, 10f, targetLayerMask)
    //        && hit.collider != null && hit.transform.TryGetComponent(out EnemyView enemyView))
    //    {
    //        return enemyView;
    //    }
    //    Debug.Log("returning null");
    //    return null;
    //}

    // DEBUG raycast not hitting collider

    public EnemyView EndTargeting(Vector3 endPosition)
    {
        arrowView.gameObject.SetActive(false);

        // Tembak raycast langsung dari kamera menembus kursor mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, targetLayerMask)
            && hit.collider != null && hit.transform.TryGetComponent(out EnemyView enemyView))
        {
            return enemyView;
        }

        return null;
    }

}
