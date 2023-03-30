
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

/*
 * Accion de ir tirar una de las lamparas
 */

public class GhostDropLamp : Action
{
    NavMeshAgent agentGhost;
    GameBlackboard blackboard;

    ControlPalanca palancaI;
    ControlPalanca palancaD;


    [SerializeField] bool pI;
    [SerializeField] bool pD;

    public override void OnAwake()
    {
        blackboard = GameBlackboard.blackBoard;
        agentGhost = GetComponent<NavMeshAgent>();

        palancaI = blackboard.westLever.GetComponent<ControlPalanca>();
        palancaD = blackboard.eastLever.GetComponent<ControlPalanca>();
    }

  
    public override TaskStatus OnUpdate()
    {

        pD = palancaD.caido;
        pI = palancaI.caido;

        Vector3 newPosition;


        if (!pI && !pD)
        {
            if ((palancaI.transform.position - agentGhost.transform.position).magnitude >
                (palancaD.transform.position - agentGhost.transform.position).magnitude)
            {
                newPosition =  palancaD.transform.position;
            }
            else
            {
                newPosition = palancaI.transform.position;
            }
        }
        else if (!pI)
        {
            newPosition = palancaI.transform.position;
        }
        else newPosition = palancaD.transform.position;


        agentGhost.SetDestination(newPosition);

       
        if(pD && pI)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;

    }

}