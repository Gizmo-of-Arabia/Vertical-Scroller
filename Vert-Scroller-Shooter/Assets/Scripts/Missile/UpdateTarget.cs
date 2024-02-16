using RyanHipplesArchitecture.SO_Variables;
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

    [SerializeField] private IntVariable playerCharacterLayer;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != playerCharacterLayer.Value) return;


        DetectedTargetPosition = collision.transform.position;
        MarkerTransform.position = DetectedTargetPosition;

        Debug.Log(collision.gameObject.name);

    }



}
