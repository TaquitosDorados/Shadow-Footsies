using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static event Action Hit; 

    private InputManager input;
    private CharacterController controller;

    public bool isAI;
    public float moveSpeed = 5f;
    public float DashPower = 10f;
    [HideInInspector] public Vector3 moveVector;
    public bool blocking;
    public float hitStunned;
    public float blockStunned;

    private void Awake()
    {
        input = GetComponent<InputManager>();
        controller = GetComponent<CharacterController>();
        moveVector = Vector3.zero;
    }
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        if(!isAI)
            moveVector = new Vector3(input.movementInput.x, 0, 0);
    }

    public void Move()
    {
        controller.Move(moveVector * Time.deltaTime * moveSpeed);
        if (controller.velocity.x < 0 && !isAI)
        {
            blocking = true;
        } else if(controller.velocity.x > 0 && isAI)
        {
            blocking = true;
        } else
        {
            blocking = false;
        }
    }

    public void Dash(Vector3 direction)
    {
        controller.Move(direction * DashPower);
    }

    public void hasBeenHit(float _blockStun, float _hitStun)
    {
        blockStunned = _blockStun;
        hitStunned = _hitStun;
        Hit?.Invoke();
    }
}
