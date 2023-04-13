
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

/// <summary>
/// Accion de accudir secuencialmente a las lamparas y tirarla
/// </summary>
public class GhostDropLamp : Action
{
    //variables necesarias: agente, y control d elas palancas
    NavMeshAgent agentGhost;
    GameBlackboard blackboard;

    ControlPalanca palancaI;
    ControlPalanca palancaD;

    //booleanos para controlar que palanca esta caida
    bool pI;
    bool pD;

    public override void OnAwake()
    {
        blackboard = GameBlackboard.blackBoard;
        agentGhost = GetComponent<NavMeshAgent>();

        palancaI = blackboard.westLever.GetComponentInChildren<ControlPalanca>();
        palancaD = blackboard.eastLever.GetComponentInChildren<ControlPalanca>();
    }


    public override TaskStatus OnUpdate()
    {
        //si el agente no esta activado no hace nada
        if (!agentGhost.enabled)
            return TaskStatus.Running;

        //en caso contrario obtiene si las palancas han caido
        pD = palancaD.caido;
        pI = palancaI.caido;

        Vector3 newPosition;

        //si ninguna esta caida se dirige a la palanca mas cercana
        if (!pI && !pD)
        {
            if ((palancaI.transform.position - agentGhost.transform.position).magnitude >
                (palancaD.transform.position - agentGhost.transform.position).magnitude)
            {
                newPosition = palancaD.transform.position;
            }
            else
            {
                newPosition = palancaI.transform.position;
            }
        }
        //si no a la que falte por activarla
        else if (!pI)
        {
            newPosition = palancaI.transform.position;
        }
        else newPosition = palancaD.transform.position;


        //y se dirige a la posicion correspondiente
        agentGhost.SetDestination(newPosition);

        //una vez esten ambas palancas bajadas devuelve success
        if (pD && pI)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;

    }

}