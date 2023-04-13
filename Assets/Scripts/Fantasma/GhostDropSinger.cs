
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

/// <summary>
/// Accion de soltar a la cantante capturadas
/// </summary>
public class GhostDropSinger : Action
{
    //variables necesarias agente y cantante
    NavMeshAgent agent;
    Cantante singer;

    public override void OnAwake()
    {
        singer = GameBlackboard.blackBoard.singer.GetComponent<Cantante>();
        agent = GetComponent<NavMeshAgent>();
    }

  
    public override TaskStatus OnUpdate()
    {
        //si la cantante no esta capturada failure
        if(!singer.capturada)
        {
            return TaskStatus.Failure;
        }
        
        //en caso contrario se detiene al agente y se llama al metodo de la cantante para soltarla
        agent.SetDestination(transform.position);
        singer.setCapturada(false);
        //tras lo que devuelve success
        return TaskStatus.Success;

    }

}