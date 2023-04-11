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
    GameObject Vizconde;
    NavMeshAgent agent;

    CapsuleCollider cc;
    bool golpeado = false;

    public override void OnAwake()
    {
        Vizconde = GameBlackboard.blackBoard.player;
        agent = GetComponent<NavMeshAgent>();

    }

    public override TaskStatus OnUpdate()
    {

        if(Vector3.Distance(Vizconde.transform.position, transform.position) < 1f)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;

    }

   
}
