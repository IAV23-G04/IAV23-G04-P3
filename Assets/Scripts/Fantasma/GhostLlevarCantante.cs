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

public class GhostLlevarCantante : Action
{
    NavMeshAgent agent;
    NavMeshAgent singerNav;
    GameObject cantante;

    GameObject sotanoNorte;

    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();

        var bb = GameBlackboard.blackBoard;


        sotanoNorte = bb.basement;
        cantante = bb.singer;
    }

    public override void OnStart()
    {
        cantante.GetComponent<Cantante>().setCapturada(true, true);
       
    }

    public override TaskStatus OnUpdate()
    {
        agent.SetDestination(sotanoNorte.transform.position);
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(sotanoNorte.transform.position.x, sotanoNorte.transform.position.z)) < 1.2f 
            && agent.pathStatus == NavMeshPathStatus.PathComplete)
            return TaskStatus.Success;
        else return TaskStatus.Running;

    }
}
