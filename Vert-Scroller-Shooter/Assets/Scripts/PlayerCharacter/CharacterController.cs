using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

/// <summary>
/// Controls movement of the player character based on input data.
/// </summary>
public class CharacterController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D playerRigidbody2D;
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animator afterburnerAnimator;



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

    [Header("Teleportation")]
    [SerializeField] private bool canTeleport;
    [SerializeField] private Vector2 teleportDestination;


    /// <summary>
    /// [0] - base state collider
    /// [1] - moving left collider
    /// [2] - moving right collider
    /// </summary>
    [Header("Colliders")]
    [SerializeField] private List<PolygonCollider2D> Colliders;
    [SerializeField] private ColliderState currentColliderState;




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

    /// <summary>
    /// Helps decide which collider variant to enable.
    /// </summary>
    private enum ColliderState
    {
        Normal,
        LeanLeft,
        LeanRight,
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
        SetHorizontalAnimatorBools();
        SetVerticalAnimatorBools();
        ToggleProperCollder();
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

    /// <summary>
    /// Sets animation conditions based on Inputs for movement along the X axis.
    /// </summary>
    private void SetHorizontalAnimatorBools()
    {
        if (targetRawVelocity.x < 0f)
        {
            bodyAnimator.SetBool("IsMovingLeft", true);
        }
        else
        {
            bodyAnimator.SetBool("IsMovingLeft", false);
        }
        if (targetRawVelocity.x > 0f)
        {
            bodyAnimator.SetBool("IsMovingRight", true);
        }
        else
        {
            bodyAnimator.SetBool("IsMovingRight", false);
        }
    }

    /// <summary>
    /// Sets animation conditions based on Inputs for movement along the Y axis.
    /// </summary>
    private void SetVerticalAnimatorBools()
    {
        if (targetRawVelocity.y < 0f)
        {
            afterburnerAnimator.SetBool("IsMovingDown", true);
        }
        else
        {
            afterburnerAnimator.SetBool("IsMovingDown", false);
        }
        if (targetRawVelocity.y > 0f)
        {
            afterburnerAnimator.SetBool("IsMovingUp", true);
        }
        else
        {
            afterburnerAnimator.SetBool("IsMovingUp", false);
        }
    }

    /// <summary>
    /// Ensures that only the appropriate collider is enabled depending on which way we're leaning.
    /// </summary>
    private void ToggleProperCollder()
    {
        // setting the serialized enum for easier debugging
        if (targetRawVelocity.x < 0f)
        {
            currentColliderState = ColliderState.LeanLeft;
        }
        else if (targetRawVelocity.x > 0f)
        {
            currentColliderState = ColliderState.LeanRight;
        }
        else
        {
            currentColliderState = ColliderState.Normal;
        }




        // enabling proper collider, disabling others
        if (currentColliderState == ColliderState.Normal)
        {
            Colliders[0].enabled = true;

            Colliders[1].enabled = false;
            Colliders[2].enabled = false;
        } 
        else if (currentColliderState == ColliderState.LeanLeft)
        {
            Colliders[1].enabled = true;

            Colliders[0].enabled = false;
            Colliders[2].enabled = false;
        }
        else if (currentColliderState == ColliderState.LeanRight)
        {
            Colliders[2].enabled = true;

            Colliders[0].enabled = false;
            Colliders[1].enabled = false;
        }



    }
    

    /// <summary>
    /// Sets canTeleport to the opposite of isInBounds. There's no point teleporting if PC is already in bounds.
    /// </summary>
    /// <param name="isInBounds">Sets canTeleport to the opposite of this.</param>
    public void DoSetCanTeleport(bool isInBounds)
    {
        canTeleport = !isInBounds;
        //Debug.Log(canTeleport);
    }

    /// <summary>
    /// Teleports the object to teleportDestination (Vec2), but only if the canTeleport flag is set to true.
    /// </summary>
    public void DoTeleportToCenter()
    {
        if (!canTeleport) return;
        transform.position = teleportDestination;
    }

}