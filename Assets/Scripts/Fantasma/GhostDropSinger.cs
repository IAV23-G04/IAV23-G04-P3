
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;


public class GhostDropSinger : Action
{
    NavMeshAgent agent;
    GameObject singer;

    public override void OnAwake()
    {
        singer = GameBlackboard.blackBoard.singer;
        agent = GetComponent<NavMeshAgent>();
    }

  
    public override TaskStatus OnUpdate()
    {
        if(!singer.GetComponent<Cantante>().capturada)
        {
            return TaskStatus.Failure;
        }
        
        agent.SetDestination(transform.position);
        singer.GetComponent<Cantante>().setCapturada(false);

        return TaskStatus.Success;

    }

}