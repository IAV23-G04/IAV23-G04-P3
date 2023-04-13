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
/// Accionde llevar a la cantante al sotano donde se encuentra la celda
/// </summary>
public class GhostLlevarCantante : Action
{
    //variables necsarias: agente, cantante y el sotano
    NavMeshAgent agent;
    GameObject cantante;

    GameObject sotanoNorte;

    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
        var bb = GameBlackboard.blackBoard;
        sotanoNorte = bb.basement;
        cantante = bb.singer;
    }

    //al empezar la accion se informa a la cantante que esta capturada para evitar que realice acciones
    public override void OnStart()
    {
        cantante.GetComponent<Cantante>().setCapturada(true, true);

    }

    public override TaskStatus OnUpdate()
    {
        //si el agente esta activado se le direcciona al sotano
        if (agent.enabled)
            agent.SetDestination(sotanoNorte.transform.position);
        //y si esta lo sufiientemente cerca y la ruta completa
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(sotanoNorte.transform.position.x, sotanoNorte.transform.position.z)) < 1.2f
            && agent.pathStatus == NavMeshPathStatus.PathComplete)
            //se devuelve success
            return TaskStatus.Success;
        else return TaskStatus.Running;

    }
}
