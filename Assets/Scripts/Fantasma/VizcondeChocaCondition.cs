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
/// Condicion de si se choca con el vizconde
/// </summary>
public class VizcondeChocaCondition : Conditional
{
    //var necesarias, distancia para choque vizconde y agente
    Player vizconde;
    NavMeshAgent agent;

    float size;

    public override void OnAwake()
    {
        vizconde = GameBlackboard.blackBoard.player.GetComponent<Player>();
        size = vizconde.areaAtaque.GetComponent<BoxCollider>().size.x;
        agent = GetComponent<NavMeshAgent>();

    }

    //si se alcanza colision se detiene al agente y se devuelve success, en caso contrario se devuelve failure
    public override TaskStatus OnUpdate()
    {

        if(vizconde.areaAtaque.activeSelf && Vector3.Distance(vizconde.transform.position, transform.position) <= size*2)
        {
            agent.SetDestination(transform.position);
            return TaskStatus.Success;
        }
            
        else
            return TaskStatus.Failure;

    }

   
}
