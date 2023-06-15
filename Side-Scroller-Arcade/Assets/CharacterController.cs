using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor.Rendering.LookDev;

/// <summary>
/// Controls movement of the player character based on input data.
/// </summary>
public class CharacterController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D playerRigidbody2D;

    [Header("Velocity Calculations")]
    [SerializeField] private Vector2 currentRawVelocity;
    [SerializeField] private Vector2 targetRawVelocity;
    [SerializeField] private float speedMultiplier;


    [Header("Movement States for each Axis")]
    [SerializeField] private MovementState movementState_x;
    [SerializeField] private MovementState movementState_y;

    [Header("State-dependent Speed Modifiers")]
    [SerializeField] private float accelerationRate;
    [SerializeField] private float decelerationRate;
    [SerializeField] private float turnRate;





    /* */

    /// <summary>
    /// Helps label/code movement based on limited possibilities.
    /// </summary>
    private enum MovementState
    {
        Constant,

        Accelerating,
        Decelerating,

        Turning,
    }



    /* */


    private void Awake()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {



    }

    private void Start()
    {

    }


    private void Update()
    {

    }

    private void FixedUpdate()
    {
        movementState_x = ChosenMovementStatePerAxis("x", targetRawVelocity.x);
        movementState_y = ChosenMovementStatePerAxis("y", targetRawVelocity.y);

        currentRawVelocity.x = RawVelocityBasedOnStatePerAxis(currentRawVelocity.x, movementState_x, targetRawVelocity.x);
        currentRawVelocity.y = RawVelocityBasedOnStatePerAxis(currentRawVelocity.y, movementState_y, targetRawVelocity.y);



        playerRigidbody2D.velocity = currentRawVelocity * speedMultiplier;

    }

    private void OnDisable()
    {


    }

    /// <summary>
    /// Sets targetRawVelocity based on Vector2 data. Should be called outside this file, e.g. by an event.
    /// </summary>
    /// <param name="x">x value in a Vector2 "input"</param>
    /// <param name="y">y value in a Vector2 "input"</param>
    public void DoSetVelocityTarget(float x, float y)
    {
        targetRawVelocity.x = x; 
        targetRawVelocity.y = y;
    }


    /// <summary>
    /// Calculates the new raw velocity for a specific axis, based on which state the movement is in.
    /// Constant: no need to change velocity.
    /// Accelerating: Velocity changes depending on the set accelerationRate.
    /// Decelerating: Velocity changes depending on the set decelerationRate.
    /// Turning:      Velocity changes depending on the set turnRate.
    /// </summary>
    /// <param name="currentRawVelocityValue">Current raw velocity. Raw means: without the speed multiplier applied. </param>
    /// <param name="state">Current movement state.</param>
    /// <param name="targetRawVelocityValue">Velocity target coming from the outside, e.g. player inputs.</param>
    /// <returns>Modified raw velocity value for a specific axis.</returns>
    private float RawVelocityBasedOnStatePerAxis(float currentRawVelocityValue, MovementState state, float targetRawVelocityValue)
    {
        float _currentRawVelocity = currentRawVelocityValue;

        switch (state)
        {
            case MovementState.Constant:
                break;
            case MovementState.Accelerating:
                _currentRawVelocity = Mathf.MoveTowards(_currentRawVelocity, targetRawVelocityValue, accelerationRate * Time.deltaTime);
                break;
            case MovementState.Decelerating:
                _currentRawVelocity = Mathf.MoveTowards(_currentRawVelocity, targetRawVelocityValue, decelerationRate * Time.deltaTime);
                break;
            case MovementState.Turning:
                _currentRawVelocity = Mathf.MoveTowards(_currentRawVelocity, targetRawVelocityValue, turnRate * Time.deltaTime);
                break;
        }

        return _currentRawVelocity;

    }

    /// <summary>
    /// First, decides the axis along which the analyzed movement is happening.
    /// Then, determines and returns the appropriate movement state along that axis, based on inputs and current movement.
    /// </summary>
    /// <param name="movementAxisName">Name of movement axis, must be x or y.</param>
    /// <param name="targetRawVelocityValue">Velocity target coming from the outside, e.g. player inputs.</param>
    /// <returns>Appropriate movement state.</returns>
    private MovementState ChosenMovementStatePerAxis(string movementAxisName, float targetRawVelocityValue)
    {
        float _currentRawVelocity;

        switch (movementAxisName)
        {
            case "x":
                _currentRawVelocity = currentRawVelocity.x;
                break;
            case "y":
                _currentRawVelocity = currentRawVelocity.y;
                break;
            default:
                Debug.LogError($"{movementAxisName} is not a valid movement axis name!");
                return MovementState.Constant;
        }



        if (_currentRawVelocity == targetRawVelocityValue)
        {
            return MovementState.Constant;
        }
        else if (System.Math.Sign(_currentRawVelocity) == System.Math.Sign(targetRawVelocityValue))
        {
            return MovementState.Accelerating;
        }
        else if (_currentRawVelocity == 0f)
        {
            return MovementState.Accelerating;
        }
        else if (targetRawVelocityValue == 0f)
        {
            return MovementState.Decelerating;
        }
        else
        {
            return MovementState.Turning;
        }
    }

    /* NOTES ON ANIMATING MOVEMENT:
     * Vertical movement states:
     * 1. Speeding up from 0 -> Jet boost (up) or jet cut (down).
     * 2. Turning Around from "+" to "-" -> Jet cut + air brakes.
     * 3. Turning around from "-" to "+" -> Jet blast / bigger boost.
     */

    /*
     * Some Horizontal movement states:
     * 1. Speeding up from 0 -> 45 degree roll.
     * 2. Turning Around -> 90 degree roll.
     * 
    */


}