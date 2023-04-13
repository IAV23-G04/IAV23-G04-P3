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

/// <summary>
/// Accion de volver a la sala de musica
/// </summary>
public class GhostReturnAction : Action
{
    //variables necesarias: agente y ubicacion de la sala
    NavMeshAgent agent;
    GameObject musicRoom;

    public override void OnAwake()
    {
        musicRoom = GameBlackboard.blackBoard.musicRoom;
        agent = GetComponent<NavMeshAgent>();
    }

    //mientras se ejecuta la accion se dirige a sala de musica y se devuelve success
    public override TaskStatus OnUpdate()
    {
        if (agent.enabled)
            agent.SetDestination(musicRoom.transform.position);
        return TaskStatus.Success;
    }
}