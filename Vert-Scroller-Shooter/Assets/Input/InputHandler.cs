using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;


/// <summary>
/// Event type that effectively holds a 2D vector, for the purpose of manipulating 2D movement inputs.
/// </summary>
[Serializable] public class MoveInputEvent : UnityEvent<float, float> { };


/// <summary>
/// Raises appropriate events for each input.
/// </summary>
public class InputHandler : MonoBehaviour
{
    private Controls _controls;
    public MoveInputEvent OnMoveInputPerformed;
    //public MoveInputEvent OnMoveInputCanceled;






    private void Awake()
    {
        _controls = new Controls();
    }


    private void OnEnable()
    {
        _controls.Movement.Enable();

        _controls.Movement.Move.performed += InvokeOnMoveInputPerformed;
        _controls.Movement.Move.canceled += InvokeOnMoveInputPerformed;



    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {

    }

    private void OnDisable()
    {
        _controls.Movement.Disable();

        _controls.Movement.Move.performed -= InvokeOnMoveInputPerformed;
        _controls.Movement.Move.canceled -= InvokeOnMoveInputPerformed;


    }


    private void InvokeOnMoveInputPerformed(InputAction.CallbackContext context)
    {
        OnMoveInputPerformed.Invoke(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y);
    }

}
