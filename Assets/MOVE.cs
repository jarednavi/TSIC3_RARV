using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MOVE : MonoBehaviour
{
    public GameObject model;
    public ObserverBehaviour[] ImageTargets;
    public float speed = 1.0f;
    private bool isMoving = false;
    private int targetIndex = 0;
    public void MoveToNextMarker()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveToTarget());
        }
    }
    private IEnumerator MoveToTarget()
    {
        isMoving = true;
        ObserverBehaviour target = GetNextDetectedTarget();
        if (target == null)
        {
            isMoving = false;
            yield break;
        }
        Vector3 startPosition = model.transform.position;
        Vector3 endPosition = target.transform.position;
        float journey = 0;
        while (journey <= 1f)
        {
            journey += Time.deltaTime * speed;
            model.transform.position = Vector3.Lerp(startPosition, endPosition, journey);
            yield return null;
        }
        targetIndex = (targetIndex + 1) % ImageTargets.Length;
        isMoving = false;
    }
    private ObserverBehaviour GetNextDetectedTarget()
    {
        int attempts = 0;
        while (attempts < ImageTargets.Length)
        {
            ObserverBehaviour target = ImageTargets[targetIndex];
            if (target != null && (target.TargetStatus.Status == Status.TRACKED || target.TargetStatus.Status == Status.EXTENDED_TRACKED))
            {
                return target;
            }
            targetIndex = (targetIndex + 1) % ImageTargets.Length;
            attempts++;
        }
        return null;
    }
}


