/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

/*
 * Accion de seguir a la cantante, cuando la alcanza devuelve Success
 */

public class GhostChaseAction : Action
{
    NavMeshAgent agent;
    GameObject singer;

    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
        singer = GameBlackboard.blackBoard.singer;

    }

    public override TaskStatus OnUpdate()
    {

        agent.SetDestination(singer.transform.position);

        if ((singer.transform.position - this.transform.position).magnitude < 1)
        {
            return TaskStatus.Success;
        }
           
        return TaskStatus.Running;
    }
}
