using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockState : PlayerState
{
    private PlayerScript player;
    private float stunnedTime;
    public float timer = 0;
    private bool blockedHit, gotHit, started;
    public override void OnEnter(PlayerStateMachine machine)
    {
        player = machine.playerScript;
        PlayerScript.Hit += HandleHit;
        blockedHit = false;
        gotHit = false;
        //Debug.Log("Locked");
        stunnedTime = machine.blockStunned;
        timer = Time.time;
        started = true;
    }

    public override void OnExit(PlayerStateMachine machine)
    {
        PlayerScript.Hit -= HandleHit;
    }

    public override void OnUpdate(PlayerStateMachine machine)
    {

        if(Time.time - timer> stunnedTime && started)
        {
            machine.SetState(machine.NeutralState);
        }

        if (blockedHit)
        {
            blockedHit = false;
            machine.SetState(machine.LockState);
        }

        if (gotHit)
        {
            gotHit = false;
            machine.SetState(machine.HitState);
        }
    }

    private void HandleHit()
    {
        if (player.blocking)
        {
            blockedHit = true;
        }
        else
        {
            gotHit = true;
        }
    }
}
