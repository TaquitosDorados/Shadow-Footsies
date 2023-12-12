using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    #region Events 
    public static event Action DashF;
    public static event Action DashB;
    public static event Action Attack;
    public static event Action Special;
    #endregion
    private InputActions inputActions;

    public Vector2 movementInput;

    private void Awake()
    {
        inputActions = new InputActions();
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        inputActions.Player1.Movement.performed += ctx => movement(ctx);
        inputActions.Player1.DashForward.performed += ctx => DashForward(ctx);
        inputActions.Player1.DashBackwards.performed += ctx => DashBackwards(ctx);
        inputActions.Player1.Attack.performed += ctx => startAttack(ctx);
        inputActions.Player1.Special.performed += ctx => startSpecial(ctx);

        inputActions.Player1.Debugger.performed += ctx => StartDebug(ctx);
    }

    void movement(InputAction.CallbackContext context)
    {
        movementInput = inputActions.Player1.Movement.ReadValue<Vector2>();
    }

    void DashForward(InputAction.CallbackContext context)
    {
        DashF?.Invoke();
    }

    void DashBackwards(InputAction.CallbackContext context)
    {
        DashB?.Invoke();
    }

    void startAttack(InputAction.CallbackContext context)
    {
        Attack?.Invoke();
    }

    void startSpecial(InputAction.CallbackContext context)
    {
        Special?.Invoke();
    }

    void StartDebug(InputAction.CallbackContext context)
    {
        GetComponent<PlayerScript>().hasBeenHit(1, 2);
    }
}
