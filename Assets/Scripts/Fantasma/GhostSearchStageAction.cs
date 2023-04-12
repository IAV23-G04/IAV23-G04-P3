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
 * Accion de ir al escenario, cuando llega devuelve Success
 */

public class GhostSearchStageAction : Action
{
    NavMeshAgent agent;
    GameObject stage;

    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
        stage = GameBlackboard.blackBoard.stage;
    }

    public override void OnStart()
    {
        agent.SetDestination(stage.transform.position);
    }

    public override TaskStatus OnUpdate()
    {
     
        if(agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}