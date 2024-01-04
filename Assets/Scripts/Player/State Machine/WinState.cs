using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : PlayerState
{
    public override void OnEnter(PlayerStateMachine machine)
    {
        Debug.Log("win " + machine.gameObject.name);
    }

    public override void OnExit(PlayerStateMachine machine)
    {
        
    }

    public override void OnUpdate(PlayerStateMachine machine)
    {
        
    }
}
