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
using Unity.VisualScripting;

/// <summary>
/// Accion para que el fantasma cierre la puerta
/// </summary>

public class GhostCloseDoorAction : Action
{
    //variables necesarias, agente del fantasma y ubicacion de la puerta
    NavMeshAgent agent;
    GameBlackboard blackboard;
    GameObject puerta;

    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
        blackboard = GameBlackboard.blackBoard;
        puerta = blackboard.puerta;
    }

    //mientras se ejecuta la accion se dirige al agente a la puerta; una vez esta lo suficientemente cerca para activarla
    //devuelve success
    public override TaskStatus OnUpdate()
    {
        agent.SetDestination(puerta.transform.position);
        if (Vector3.SqrMagnitude(transform.position - puerta.transform.position) < 1.5f)
        {
            agent.SetDestination(transform.position);
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}