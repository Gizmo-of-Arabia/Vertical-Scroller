using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Moves missile forward and rotates it when needed.
/// </summary>
public class MissileMovement : MonoBehaviour
{
    [Header("Movement Values")]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;


    [Header("References")]
    [SerializeField] private UpdateTarget frontSensor;

    [Header("Physics Calculations")]
    private Vector3 heading;
    private Vector3 direction;
    private Quaternion currentRotation;
    private Quaternion targetRotation;

    [Header("Components")]
    private Rigidbody2D myRigidbody;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();  
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        FaceTarget(frontSensor.DetectedTargetPosition);
        SetForwardVelocity();


    }



    private void FaceTarget(Vector3 target)
    {
        currentRotation = transform.rotation;

        heading = target - transform.position;
        direction = Vector3.Normalize(heading);

        targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);


    }

    private void SetForwardVelocity()
    {
        myRigidbody.velocity = transform.up * speed;
    }

    // Quaternion targetRotation = Quaternion.LookRotation(direction);
}
