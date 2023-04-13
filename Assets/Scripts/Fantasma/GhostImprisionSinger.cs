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
/// Accion de encerrar a la cantante en la celda
/// </summary>
public class GhostImprisionSinger : Action
{
    //variables necesarias como el agente, la celda, la cantante
    GameBlackboard bb;

    NavMeshAgent agent;
    GameObject jail;
    Cantante singer;
    //booleano para saber si esta en camino
    bool going = false;

    public override void OnAwake()
    {
        bb = GameBlackboard.blackBoard;

        agent = GetComponent<NavMeshAgent>();
        singer = bb.singer.GetComponent<Cantante>();
        jail = bb.celda;
    }

    public override TaskStatus OnUpdate()
    {
        //si no esta en camino se da el destino de la celda 
        if(!going)
        {
            agent.SetDestination(jail.transform.position);
            going = true;
        }
       
        //si esta lo suficientemente cerca d ela celda
        if (Vector3.Distance(transform.position, jail.transform.position) < 1.3f)
        {
            //se detiene al agente y actualzian variables de estado
            bb.imprisoned = true;
            agent.SetDestination(transform.position);
            //se llamana a los metodos de la cantante pertinentes al encierro y se reubica
            singer.setCapturada(false);
            singer.enPrision();
            singer.transform.position = transform.position + transform.forward * 2;
            //se reinciia a la situacion inicial y se devuelve success
            going = false;
            return TaskStatus.Success;
        }
           
        return TaskStatus.Running;
    }
}
