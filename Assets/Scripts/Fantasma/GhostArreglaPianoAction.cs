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
/// Accion para que el fantasma arregle el piano si es pertinente
/// </summary>
public class GhostArreglaPianoAction : Action
{
    //variables del piano y el agente del fantasma necesarias para la implementacion
    ControlPiano pianoControl;
    GameObject piano;
    NavMeshAgent agent;

    public override void OnAwake()
    {
        pianoControl = GameObject.FindGameObjectWithTag("Piano").GetComponent<ControlPiano>();
        piano = GameObject.FindGameObjectWithTag("Piano");
        agent = GetComponent<NavMeshAgent>();
    }

    
    //mientras se ejecuta la accion se dirige al agente al piano; una vez esta cerca y si el piano esta roto lo arregla; devolviendo success
    public override TaskStatus OnUpdate()
    {
        agent.SetDestination(piano.transform.position);

        if (pianoControl.roto && 
            Vector2.Distance(new Vector2(piano.transform.position.x, piano.transform.position.z), new Vector2(transform.position.x, transform.position.z)) < 2f)
        {
            pianoControl.ArreglaPiano();
            return TaskStatus.Success;
        }
        else return TaskStatus.Running;
    }
}
