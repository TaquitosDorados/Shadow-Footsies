using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static event Action Hit; 
    public static event Action confirmHit;

    private InputManager input;
    private CharacterController controller;
    private Animator animator;

    [Header("General")]
    public float moveSpeed = 5f;
    public float DashPower = 10f;
    public bool isAI;
    [HideInInspector] public Vector3 moveVector;
    [HideInInspector] public bool blocking;
    [HideInInspector] public float hitStunned;
    [HideInInspector] public float blockStunned;
    [HideInInspector] public bool canSpecial;
    [HideInInspector] public bool specialCharged;
    [HideInInspector] public bool win;
    public Collider2D[] attackBoxes;
    
    [Header("Neutral Attack")]
    public float NStartTime;
    public float NLagTime;
    public float NBlockStun;
    public float NHitStun;
    [Header("Moving Attack")]
    public float MStartTime;
    public float MLagTime;
    public float MBlockStun;
    public float MHitStun;
    [Header("Special Attack")]
    public float SStartTime;
    public float SLagTime;
    public float SBlockStun;
    public float SHitStun;

    private void Awake()
    {
        input = GetComponent<InputManager>();
        controller = GetComponent<CharacterController>();
        moveVector = Vector3.zero;
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        InputManager.Special += chargeSpecial;
    }

    private void OnDisable()
    {
        InputManager.Special -= chargeSpecial;
    }

    private void Update()
    {
            moveVector = new Vector3(input.movementInput.x, 0, 0);
    }

    public void Move()
    {
        controller.Move(moveVector * Time.deltaTime * moveSpeed);
        if (controller.velocity.x < 0)
        {
            blocking = true;
            animator.Play("WalkBack");
            animator.SetBool("Moving", true);
        } else if(controller.velocity.x > 0)
        {
            blocking = false;
            animator.Play("WalkFront");
            animator.SetBool("Moving", true);
        } else
        {
            blocking = false;
            animator.SetBool("Moving", false);
        }
    }

    public void Dash(Vector3 direction)
    {
        blocking= false;
        controller.Move(direction * DashPower);
        //controller.SimpleMove(direction * DashPower);
        if(direction.x< 0)
        {
            animator.Play("DashBack");
        } else
        {
            animator.Play("DashFront");
        }
    }

    public void hasBeenHit(float _blockStun, float _hitStun)
    {
        StopAllCoroutines();
        canSpecial = false;
        specialCharged = false;
        blockStunned = _blockStun;
        hitStunned = _hitStun;
        Hit?.Invoke();
    }

    public void HitAnim()
    {
        confirmHit?.Invoke();
        animator.Play("Hit");
    }
    public void BlockAnim()
    {
        animator.Play("Block");
    }
    public void neutralAttack()
    {
        animator.Play("NAttack");
        StartCoroutine(AttackCoroutine(attackBoxes[0], NStartTime, NLagTime, NBlockStun, NHitStun));
        //Debug.Log("Neutral Attack");
    }

    public void moveAttack()
    {
        animator.Play("MAttack");
        StartCoroutine(AttackCoroutine(attackBoxes[1], MStartTime, MLagTime, MBlockStun, MHitStun));
        //Debug.Log("Move Attack");
    }

    public void specialAttack()
    {
        animator.Play("SAttack");
        StartCoroutine(AttackCoroutine(attackBoxes[2], SStartTime, SLagTime, SBlockStun, SHitStun));
        //Debug.Log("Special Attack");
    }

    IEnumerator AttackCoroutine(Collider2D _attackbox, float _startTime, float _LagTime, float _BlockStun, float _HitStun)
    {
        canSpecial = false;
        blockStunned = _LagTime;
        yield return new WaitForSeconds(_startTime);
        float angle = _attackbox.transform.rotation.eulerAngles.z;
        Collider2D[] cols = Physics2D.OverlapBoxAll(_attackbox.bounds.center, _attackbox.bounds.extents, angle, LayerMask.GetMask("Hurtbox"));
        foreach(Collider2D col in cols)
        {
            if (col.transform.root == transform)
                continue;
            col.transform.root.GetComponent<AIScript>().handleHit(_BlockStun, _HitStun);
            break;
        }
        yield return new WaitForSeconds(_LagTime - _startTime);
        canSpecial = true;
        yield return new WaitForSeconds(0.3f);
        canSpecial = false;
    }

    private void chargeSpecial()
    {
        specialCharged = true;
    }

    public void Defeat()
    {
        animator.Play("KnockDown");
        AIScript ai = FindObjectOfType<AIScript>();
        ai.Victory();
    }

    public void Victory()
    {
        win = true;
        animator.Play("Win");
    }
}
