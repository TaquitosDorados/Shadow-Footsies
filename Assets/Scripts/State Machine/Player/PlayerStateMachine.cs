using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private PlayerScript m_playerScript;

    private PlayerState currentState;

    private PlayerState neutralState = new NeutralState();
    private PlayerState lockState = new LockState();
    private PlayerState hitState = new HitState();
    private PlayerState deadState = new DeadState();

    public PlayerScript playerScript => m_playerScript;

    public PlayerState NeutralState => neutralState;
    public PlayerState LockState => lockState;
    public PlayerState HitState => hitState;
    public PlayerState DeadState => deadState;

    public float blockStunned, hitStunned;

    private void Awake()
    {
        SetState(neutralState);
    }

    public void SetState(PlayerState state)
    {
        currentState?.OnExit(this);
        currentState = state;
        currentState?.OnEnter(this);
    }

    private void Update()
    {
        currentState?.OnUpdate(this);
        blockStunned = playerScript.blockStunned;
        hitStunned = playerScript.hitStunned;

    }

}
