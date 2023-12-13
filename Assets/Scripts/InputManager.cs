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
    public static event Action SpecialStopped;
    #endregion
    private InputActions inputActions;
    private bool dashChecker;
    private float lastDirectionX;

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
        inputActions.Player1.Dash.started += ctx => checkForDash(ctx);
        inputActions.Player1.Attack.performed += ctx => startAttack(ctx);
        inputActions.Player1.Special.performed += ctx => startSpecial(ctx);
        inputActions.Player1.Special.canceled += ctx => endSpecial(ctx);

        inputActions.Player1.Debugger.performed += ctx => StartDebug(ctx);
    }

    void movement(InputAction.CallbackContext context)
    {
        movementInput = inputActions.Player1.Movement.ReadValue<Vector2>();
    }

    void checkForDash(InputAction.CallbackContext context)
    {
        StopAllCoroutines();
        if(dashChecker)
        {
            dashChecker = false;
            if(lastDirectionX > 0 && inputActions.Player1.Movement.ReadValue<Vector2>().x > 0)
            {
                DashForward(context);
            } else if(lastDirectionX < 0 && inputActions.Player1.Movement.ReadValue<Vector2>().x < 0)
            {
                DashBackwards(context);
            }
        } else
        {            
            lastDirectionX = inputActions.Player1.Movement.ReadValue<Vector2>().x;
            dashChecker = true;
            StartCoroutine(dashCheckerLife());
        }
    }

    IEnumerator dashCheckerLife()
    {
        yield return new WaitForSeconds(0.3f);
        dashChecker = false;
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

    void endSpecial(InputAction.CallbackContext context)
    {
        SpecialStopped?.Invoke();
    }

    void StartDebug(InputAction.CallbackContext context)
    {
        GetComponent<PlayerScript>().hasBeenHit(0.5f,0.5f);
    }
}
