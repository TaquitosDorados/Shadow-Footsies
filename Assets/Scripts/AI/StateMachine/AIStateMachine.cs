using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    [SerializeField] private AIScript m_aIScript;

    private AIState currentState;

    private AIState stateSelector = new StateSelector();
    private AIState hitState = new AIHitState();
    private AIState blockState = new AIBlockState();
    private AIState attackState = new AIAttackState();
    private AIState deadState = new AIDeadState();
    private AIState winState = new AIWinState();

    public AIScript aIScript => m_aIScript;

    public AIState StateSelector => stateSelector;
    public AIState HitState => hitState;
    public AIState BlockState => blockState;
    public AIState AttackState => attackState;
    public AIState DeadState => deadState;
    public AIState WinState => winState;

    private void Awake()
    {
        SetState(stateSelector);
    }

    public void SetState(AIState state)
    {
        currentState?.OnExit(this);
        currentState = state;
        currentState?.OnEnter(this);
    }

    private void Update()
    {
        currentState?.OnUpdate(this);
    }
}
