using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWinState : AIState
{
    public override void OnEnter(AIStateMachine machine)
    {
        machine.aIScript.Victory();
    }

    public override void OnExit(AIStateMachine machine)
    {
        
    }

    public override void OnUpdate(AIStateMachine machine)
    {
        
    }
}
