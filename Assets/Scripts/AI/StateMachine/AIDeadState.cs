using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeadState : AIState
{
    public override void OnEnter(AIStateMachine machine)
    {
        Debug.Log("Dead");
        machine.aIScript.Defeat();
    }

    public override void OnExit(AIStateMachine machine)
    {
        
    }

    public override void OnUpdate(AIStateMachine machine)
    {
        
    }
}
