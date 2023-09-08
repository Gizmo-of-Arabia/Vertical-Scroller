using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holds and updates position info about the projectile's target (the player).
/// </summary>
public class UpdateTarget : MonoBehaviour
{
    public Vector3 DetectedTargetPosition;

    // MARKER FOR DEBUGGING ONLY
    public Transform MarkerTransform;

    private void OnTriggerStay2D(Collider2D collision)
    {
        MarkerTransform.position = DetectedTargetPosition;
        DetectedTargetPosition = collision.transform.position;

    }



}
