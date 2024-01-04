using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralState : PlayerState
{
    private PlayerScript player;
    private bool blockedHit, gotHit, dash, specialReady, specialGo, neutralAttack, moveAttack;
    public override void OnEnter(PlayerStateMachine machine)
    {
        //Debug.Log("Neutral");

        player = machine.playerScript;
        blockedHit = false;
        gotHit = false;
        dash = false;
        specialGo = false;
        specialReady = false;
        neutralAttack = false;
        moveAttack = false;

        if (!machine.playerScript.isAI)
        {
           
            InputManager.DashF += DashForward;
            InputManager.DashB += DashBackwards;
            InputManager.Special += startSpecial;
            InputManager.SpecialStopped += LaunchSpecial;
            InputManager.Attack += HandleAttack;
        } 
        PlayerScript.Hit += HandleHit;
    }

    public override void OnExit(PlayerStateMachine machine)
    {
        PlayerScript.Hit -= HandleHit;
        InputManager.DashF -= DashForward;
        InputManager.DashB -= DashBackwards;
        InputManager.Special -= startSpecial;
        InputManager.SpecialStopped -= LaunchSpecial;
        InputManager.Attack -= HandleAttack;
    }

    public override void OnUpdate(PlayerStateMachine machine)
    {
        if (machine.playerScript.win)
        {
            machine.SetState(machine.WinState);
        }

        if (dash)
        {
            dash = false;
            machine.SetState(machine.LockState);
        }

        if (!machine.playerScript.isAI)
            machine.playerScript.Move();

        if(blockedHit)
        {
            blockedHit = false;
            machine.playerScript.BlockAnim();
            machine.SetState(machine.LockState);
        }

        if(gotHit)
        {
            gotHit = false;
            if (player.hitStunned == -1)
            {
                machine.SetState(machine.DeadState);
            }
            else
            {
                machine.SetState(machine.HitState);
            }
        }



        if (specialGo && player.specialCharged || specialGo && player.canSpecial)
        {
            specialGo = false;
            player.specialAttack();
            machine.SetState(machine.LockState);
        }

        if (moveAttack)
        {
            moveAttack = false;
            player.moveAttack();
            machine.SetState(machine.LockState);
        }

        if (neutralAttack)
        {
            neutralAttack = false;
            player.neutralAttack();
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
        player.blockStunned = .5f;
        dash = true;
    }

    private void DashBackwards()
    {
        player.Dash(new Vector3(-1, 0, 0));
        player.blockStunned = .5f;
        dash = true;
    }

    private void startSpecial()
    {
        specialReady = true;
    }

    private void LaunchSpecial()
    {
        specialGo = true;
    }

    private void HandleAttack()
    {
        if (player.canSpecial)
        {
            specialReady = true;
            specialGo = true;
        } else if (player.moveVector.magnitude > 0)
        {
            moveAttack = true;
        } else
        {
            neutralAttack = true;
        }
    }
}
