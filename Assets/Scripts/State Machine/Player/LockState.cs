using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockState : PlayerState
{
    private float stunnedTime, timer;
    public override void OnEnter(PlayerStateMachine machine)
    {
        Debug.Log("Locked");
        stunnedTime = machine.blockStunned;
        timer = Time.time;
    }

    public override void OnExit(PlayerStateMachine machine)
    {
        
    }

    public override void OnUpdate(PlayerStateMachine machine)
    {
        if(Time.time - timer> stunnedTime)
        {
            machine.SetState(machine.NeutralState);
        }
    }
}
