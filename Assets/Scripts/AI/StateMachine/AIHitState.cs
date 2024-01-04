using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHitState : AIState
{
    private float stunTime;
    private float timer;
    private bool hit;
    public override void OnEnter(AIStateMachine machine)
    {
        Debug.Log("HitState");
        hit = false;
        stunTime = machine.aIScript.stunnedTime;
        machine.aIScript.HitAnim();
        timer = Time.time;
        machine.aIScript.MB = false;
        machine.aIScript.MF = false;
        AIScript.Hit += HitAgain;
    }

    public override void OnExit(AIStateMachine machine)
    {
        AIScript.Hit -= HitAgain;
    }

    public override void OnUpdate(AIStateMachine machine)
    {
        if (hit)
        {
            machine.SetState(machine.DeadState);
        }

        if (Time.time - timer < stunTime)
        {
            return;
        }

        machine.aIScript.MB = true;

        if(Time.time - timer <= stunTime + 0.7f)
        {
            machine.SetState(machine.StateSelector);
        }
    }

    private void HitAgain()
    {
        hit= true;
    }
}
