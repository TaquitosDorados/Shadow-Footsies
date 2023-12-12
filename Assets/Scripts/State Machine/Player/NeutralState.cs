using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralState : PlayerState
{
    private PlayerScript player;
    private bool blockedHit, gotHit, dash;
    public override void OnEnter(PlayerStateMachine machine)
    {
        Debug.Log("Neutral");

        player = machine.playerScript;
        blockedHit = false;
        gotHit = false;
        dash = false;

        PlayerScript.Hit += HandleHit;
        InputManager.DashF += DashForward;
        InputManager.DashB += DashBackwards;
    }

    public override void OnExit(PlayerStateMachine machine)
    {
        PlayerScript.Hit -= HandleHit;
        InputManager.DashF -= DashForward;
        InputManager.DashB -= DashBackwards;
    }

    public override void OnUpdate(PlayerStateMachine machine)
    {
        machine.playerScript.Move();

        if(blockedHit)
        {
            blockedHit = false;
            machine.SetState(machine.LockState);
        }

        if(gotHit)
        {
            gotHit = false;
            machine.SetState(machine.HitState);
        }

        if(dash)
        {
            dash = false;
            machine.SetState(machine.LockState);
        }
    }

    private void HandleHit()
    {
        if(player.blocking)
        {
            blockedHit = true;
        } else
        {
            gotHit = true;
        }
    }

    private void DashForward()
    {
        player.Dash(new Vector3(1, 0, 0));
        player.blockStunned = 0.2f;
        dash = true;
    }

    private void DashBackwards()
    {
        player.Dash(new Vector3(-1, 0, 0));
        player.blockStunned = 0.2f;
        dash = true;
    }
}
