using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : PlayerState
{
    private float stunnedTime;
    public float timer = 0;
    private bool hit, started;
    public override void OnEnter(PlayerStateMachine machine)
    {
        PlayerScript.Hit += handleHit;
        hit = false;
        Debug.Log("Hit" + machine.gameObject.name);
        stunnedTime = machine.hitStunned;
        timer = Time.time;
        started = true;
        machine.playerScript.HitAnim();
    }

    public override void OnExit(PlayerStateMachine machine)
    {
        PlayerScript.Hit -= handleHit;
    }

    public override void OnUpdate(PlayerStateMachine machine)
    {

        if (Time.time - timer > stunnedTime && started)
        {
            machine.SetState(machine.NeutralState);
        }

        if (hit)
        {
            hit = false;
            machine.SetState(machine.DeadState);
        }
    }

    void handleHit()
    {
        hit = true;
    }
}
