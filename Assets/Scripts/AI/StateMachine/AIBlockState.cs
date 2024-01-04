using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBlockState : AIState
{
    private float timer,timer2, stunTime;
    private float waitTime = 0.3f;
    private bool blockAgain;
    public override void OnEnter(AIStateMachine machine)
    {
        Debug.Log("BlockState");
        machine.aIScript.BlockAnim();
        blockAgain= false;
        machine.aIScript.MB = false;
        machine.aIScript.Blocking = true;
        stunTime = machine.aIScript.stunnedTime;
        timer = Time.time;

        AIScript.Block += BlockedAgain;
    }

    public override void OnExit(AIStateMachine machine)
    {
        AIScript.Block -= BlockedAgain;
    }

    public override void OnUpdate(AIStateMachine machine)
    {
        if (Time.time - timer >= stunTime + waitTime)
        {
            if(blockAgain)
            {
                Debug.Log("BlockedAgain");
                stunTime = machine.aIScript.stunnedTime;
                if (Time.time-timer2>=stunTime)
                {
                    float random = Random.Range(0.0f, 1.0f);
                    if (random <= 0.5f)
                    {
                        machine.aIScript.currentAttack = 1;
                        machine.SetState(machine.AttackState);
                    } else
                    {
                        machine.aIScript.currentAttack = 2;
                        machine.SetState(machine.AttackState);
                    }
                }
            } else
            {
                machine.aIScript.Blocking = false;
                machine.SetState(machine.StateSelector);
            }
        }
    }

    private void BlockedAgain()
    {
        timer2= Time.time;
        blockAgain = true;
    }
}
