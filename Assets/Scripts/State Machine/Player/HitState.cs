using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : PlayerState
{
    private float stunnedTime, timer;
    public override void OnEnter(PlayerStateMachine machine)
    {
        Debug.Log("Hit");
        stunnedTime = machine.hitStunned;
        timer = Time.time;
    }

    public override void OnExit(PlayerStateMachine machine)
    {
        
    }

    public override void OnUpdate(PlayerStateMachine machine)
    {
        if (Time.time - timer > stunnedTime)
        {
            machine.SetState(machine.NeutralState);
        }
    }
}
