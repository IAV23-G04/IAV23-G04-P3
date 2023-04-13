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
/// Accion de ir a una sala aleatoria, asignada por el Blackboard, cuando llega devuelve Success
/// </summary>
public class GhostSearchRandomAction : Action
{

    //variables necesarias: agente y sitio aleatorio
    NavMeshAgent agent;
    GameObject randomSitio;

    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    //al empezar obtiene un sitio random
    public override void OnStart()
    {
        randomSitio = GameBlackboard.blackBoard.getRandomSitio();
    }

    public override TaskStatus OnUpdate()
    {
        //meintras se ejecuta se obtiene la posicioon de navmesh mas cercana a la posicion aleatoria
        var navHit = new NavMeshHit();
        NavMesh.SamplePosition(transform.position, out navHit, 2, NavMesh.AllAreas);
        //si el agente esta activo se dirige a ella
        if(agent.enabled)
            agent.SetDestination(randomSitio.transform.position);
        //y si se alcanza dicha posicion se detiene al agente y se devuelve success
        if (Vector3.SqrMagnitude(transform.position - randomSitio.transform.position) < 1.5f)
        {
            agent.SetDestination(transform.position);
            return TaskStatus.Success;
        }
        else return TaskStatus.Running;
    }
}