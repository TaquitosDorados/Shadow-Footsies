
using UnityEngine;

public class StateSelector : AIState
{
    private float tickTime = 0.5f;
    private float timer;
    private bool selection, backdown, dashAttack, hit, block, win;
    public override void OnEnter(AIStateMachine machine)
    {
        selection = false; backdown = false; dashAttack = false; hit = false; block = false; win= false;
        timer = Time.time;
        Debug.Log("Selecting");

        AIScript.Block += BlockStart;
        AIScript.Hit += getHit;
    }

    public override void OnExit(AIStateMachine machine)
    {
        AIScript.Block -= BlockStart;
        AIScript.Hit -= getHit;
    }

    public override void OnUpdate(AIStateMachine machine)
    {
        if (win || machine.aIScript.win)
        {
            machine.SetState(machine.WinState);
        }

        if (block)
        {
            machine.SetState(machine.BlockState);
        }

        if(hit)
        {
            machine.SetState(machine.HitState);
        }

        if(Time.time - timer < tickTime)
        {
            return;
        }

        if (dashAttack)
        {
            dashAttack = false;
            timer = Time.time;
            machine.SetState(machine.AttackState);
            return;
        }

        if (backdown)
        {
            backdown = false;
            tickTime = 0.5f;
            timer = Time.time;
            //Move Back
            Debug.Log("MB from MF");
            machine.aIScript.MB = true;
            return;
        }

        if (!selection)
        {
            selection = true;
            Select(machine);
        }

    }

    private void Select(AIStateMachine machine)
    {
        if (machine.aIScript.distanceToPlayer <= 2.5f)
        {
            Debug.Log("Cerca");
            //CERCA
            float random = Random.Range(0.0f, 1.0f);

            if(random < 0.3f)
            {
                //Neutral Attack
                Debug.Log("NAttack");
                machine.aIScript.currentAttack = 1;
                machine.SetState(machine.AttackState);
            } else if (random < 0.6f)
            {
                //Moving Attack
                Debug.Log("MAttack");
                machine.aIScript.currentAttack = 2;
                machine.SetState(machine.AttackState);
            } else if (random < 0.75f)
            {
                //Special Attack
                Debug.Log("SAttack");
                machine.aIScript.currentAttack = 3;
                machine.SetState(machine.AttackState);
            } else if(random < 0.925f)
            {
                //Move Back
                Debug.Log("MB");
                machine.aIScript.MB = true;
            } else if (random < 0.975f)
            {
                //Dash Back
                Debug.Log("DB");
                machine.aIScript.Dash(new Vector3(1, 0, 0));
            } else
            {
                //Move Forward
                Debug.Log("MF");
                machine.aIScript.MB = false;
                machine.aIScript.MF = true;
            }
        }
        else
        {
            //LEJOS
            float random = Random.Range(0.0f, 1.0f);
            if (random < 0.5625f)
            {
                //Move Forward
                Debug.Log("MF");

                machine.aIScript.MB = false;
                machine.aIScript.MF = true;

                random = Random.Range(0.0f, 1.0f);
                if (random >= 0.8f)
                {
                    tickTime = 0.2f;
                    backdown = true;
                }
            }
            else if (random < 0.75f)
            {
                //Move Back
                Debug.Log("MB");
                machine.aIScript.MB = true;
                machine.aIScript.MF = false;
            }
            else if (random < 0.9375f)
            {
                //Dash Forward
                Debug.Log("DF");

                machine.aIScript.Dash(new Vector3(-1, 0, 0));

                random = Random.Range(0.0f, 1.0f);
                if (random <= .5f)
                {
                    random = Random.Range(0.0f, 1.0f);
                    dashAttack = true;
                    switch(random)
                    {
                        case <= 0.4f:
                            Debug.Log("NAttack From DF");
                            machine.aIScript.currentAttack = 1;
                            machine.SetState(machine.AttackState);
                            break;
                        case <= 0.8f:
                            Debug.Log("MAttack From DF");
                            machine.aIScript.currentAttack = 2;
                            machine.SetState(machine.AttackState);
                            break;
                        default:
                            Debug.Log("SAttack From DF");
                            machine.aIScript.currentAttack = 3;
                            machine.SetState(machine.AttackState);
                            break;
                    }
                }
            }
            else
            {
                //Dash Back
                Debug.Log("DB");
                machine.aIScript.Dash(new Vector3(1, 0, 0));
            }
        }

        timer = Time.time;
        selection = false;
    }

    private void getHit()
    {
        hit = true;
    }

    private void BlockStart()
    {
        block = true;
    }

    private void hasWon()
    {
        win= true;
    }
}
