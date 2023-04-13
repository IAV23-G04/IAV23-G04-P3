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
/// Accion para detener al fantasma
/// </summary>
public class GhostStop : Action
{
    //mediante el agente
    NavMeshAgent agent;
    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        agent.SetDestination(transform.position);
        return TaskStatus.Success;
 
    }
}
