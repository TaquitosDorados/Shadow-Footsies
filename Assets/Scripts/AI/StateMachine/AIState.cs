using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState
{
    public abstract void OnEnter(AIStateMachine machine);
    public abstract void OnUpdate(AIStateMachine machine);
    public abstract void OnExit(AIStateMachine machine);
}
