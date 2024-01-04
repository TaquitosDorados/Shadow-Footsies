using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    public static event Action Hit;
    public static event Action Block;
    public static event Action Win;

    [Header("General")]
    public float moveSpeed = 5f;
    public float DashPower = 1.5f;
    public Collider2D[] attackBoxes;

    [HideInInspector] public float distanceToPlayer;
    [HideInInspector] public float stunnedTime;
    [HideInInspector] public bool MF, MB, Blocking, win;
    [HideInInspector] public int currentAttack; //1-NA 2-MA 3-SA

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

    [HideInInspector] public PlayerScript player;

    private CharacterController controller;
    private Animator animator;

    private void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        controller= GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);

        if(MB)
        {
            MF = false;
            Vector3 moveVector = new Vector3(1, 0, 0);
            controller.Move(moveVector * Time.deltaTime * moveSpeed);
            Blocking = true;
            animator.SetBool("Moving", true);
            animator.Play("WalkBack");
        } else if (MF)
        {
            MB = false;
            Vector3 moveVector = new Vector3(-1, 0, 0);
            controller.Move(moveVector * Time.deltaTime * moveSpeed);
            Blocking = false;
            animator.SetBool("Moving", true);
            animator.Play("WalkFront");
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }

    public void Dash(Vector3 direction)
    {
        Blocking = false;
        MB = false;
        MF = false;
        controller.Move(direction * DashPower);
        if (direction.x < 0)
        {
            animator.Play("DashFront");
        }
        else
        {
            animator.Play("DashBack");
        }
    }

    public void Attack()
    {
        MB = false;
        MF = false;
        Blocking = false;
        switch (currentAttack)
        {
            case 1:
                animator.Play("NAttack");
                StartCoroutine(AttackCoroutine(attackBoxes[0], NStartTime, NLagTime, NBlockStun, NHitStun));
                break;
            case 2:
                animator.Play("MAttack");
                StartCoroutine(AttackCoroutine(attackBoxes[1], MStartTime, MLagTime, MBlockStun, MHitStun));
                break;
            case 3:
                animator.Play("SAttack");
                StartCoroutine(AttackCoroutine(attackBoxes[2], SStartTime, SLagTime, SBlockStun, SHitStun));
                break;
        }
    }

    IEnumerator AttackCoroutine(Collider2D _attackbox, float _startTime, float _LagTime, float _BlockStun, float _HitStun)
    {
        yield return new WaitForSeconds(_startTime);
        float angle = _attackbox.transform.rotation.eulerAngles.z;
        Collider2D[] cols = Physics2D.OverlapBoxAll(_attackbox.bounds.center, _attackbox.bounds.extents, angle, LayerMask.GetMask("Hurtbox"));
        foreach (Collider2D col in cols)
        {
            if (col.transform.root == transform)
                continue;
            col.transform.root.GetComponent<PlayerScript>().hasBeenHit(_BlockStun, _HitStun);
            break;
        }
    }

    public void handleHit(float _BlockStun, float _HitStun)
    {
        StopAllCoroutines();
        if (Blocking)
        {
            stunnedTime = _BlockStun;
            Block?.Invoke();
        } else
        {
            stunnedTime = _HitStun;
            Hit?.Invoke();
        }
        MB = false;
        MF= false;
    }
    public void HitAnim()
    {
        animator.Play("Hit");
    }

    public void BlockAnim()
    {
        animator.Play("Block");
    }

    public void Victory()
    {
        Win?.Invoke();
        win = true;
        animator.Play("Win");
    }

    public void Defeat()
    {
        animator.Play("KnockDown");
        player.Victory();
    }
}
