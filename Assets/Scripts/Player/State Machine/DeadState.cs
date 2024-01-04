using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : PlayerState
{
    public override void OnEnter(PlayerStateMachine machine)
    {
        Debug.Log("muerto " + machine.gameObject.name);
        machine.playerScript.Defeat();
    }

    public override void OnExit(PlayerStateMachine machine)
    {
        
    }

    public override void OnUpdate(PlayerStateMachine machine)
    {

    }
}
