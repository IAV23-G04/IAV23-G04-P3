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
 * Devuelve Success cuando el vizconde choca con el fantasma
 */


public class VizcondeChocaCondition : Conditional
{
    Player vizconde;
    NavMeshAgent agent;

    float size;

    public override void OnAwake()
    {
        vizconde = GameBlackboard.blackBoard.player.GetComponent<Player>();
        size = vizconde.areaAtaque.GetComponent<BoxCollider>().size.x;
        agent = GetComponent<NavMeshAgent>();

    }

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
