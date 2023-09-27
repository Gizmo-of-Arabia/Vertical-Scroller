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
[Serializable] public class ResetInputEvent : UnityEvent { };



/// <summary>
/// Raises appropriate events for each input.
/// </summary>
public class InputHandler : MonoBehaviour
{
    private Controls _controls;
    public MoveInputEvent OnMoveInputPerformed;
    public ResetInputEvent OnResetInputPerformed;






    private void Awake()
    {
        _controls = new Controls();
    }


    private void OnEnable()
    {
        _controls.Movement.Enable();
        _controls.Special.Enable();

        _controls.Movement.Move.performed += InvokeOnMoveInputPerformed;
        _controls.Movement.Move.canceled += InvokeOnMoveInputPerformed;

        _controls.Special.Reset.performed += InvokeOnResetInputPerformed;



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
        _controls.Special.Disable();

        _controls.Movement.Move.performed -= InvokeOnMoveInputPerformed;
        _controls.Movement.Move.canceled -= InvokeOnMoveInputPerformed;

        _controls.Special.Reset.performed -= InvokeOnResetInputPerformed;



    }


    private void InvokeOnMoveInputPerformed(InputAction.CallbackContext context)
    {
        OnMoveInputPerformed.Invoke(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y);
    }

    private void InvokeOnResetInputPerformed(InputAction.CallbackContext context)
    {
        OnResetInputPerformed.Invoke();
    }

}
