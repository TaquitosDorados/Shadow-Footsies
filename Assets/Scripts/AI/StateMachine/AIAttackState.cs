using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class AIAttackState : AIState
{
    private float timer, lagTime;
    private bool Hit, gotHit;
    public override void OnEnter(AIStateMachine machine)
    {
        Hit= false;
        gotHit= false;
        Debug.Log("AttackState");
        machine.aIScript.Attack();

        PlayerScript.confirmHit += hitConfirm;

        switch (machine.aIScript.currentAttack)
        {
            case 1:
                lagTime = machine.aIScript.NLagTime;
                break;
            case 2:
                lagTime = machine.aIScript.MLagTime;
                break;
            case 3:
                lagTime = machine.aIScript.SLagTime;
                break;
        }
        Debug.Log(lagTime);
        timer = Time.time;

        AIScript.Hit += getHit;
    }

    public override void OnExit(AIStateMachine machine)
    {
        PlayerScript.confirmHit -= hitConfirm;
        AIScript.Hit -= getHit;
    }

    public override void OnUpdate(AIStateMachine machine)
    {
        if (gotHit)
        {
            machine.SetState(machine.HitState);
        }

        if (Time.time - timer >= lagTime)
        {
            float random = Random.Range(0.0f, 1.0f);
            if (random < 0.1)
            {
                //Special Attack
                machine.aIScript.currentAttack = 3;
                machine.SetState(machine.AttackState);
            }
            else if (random < 0.7f)
            {
                machine.aIScript.MB = true;
            }
            machine.SetState(machine.StateSelector);
            return;
        }
        
        if (Time.time - timer >= lagTime - 0.2f)
        {
            if (Hit && machine.aIScript.currentAttack != 3)
            {
                //Special Attack
                machine.aIScript.currentAttack = 3;
                machine.SetState(machine.AttackState);
            }
        }
    }

    private void hitConfirm()
    {
        Hit= true;
    }

    private void getHit()
    {
        gotHit= true;
    }
}
