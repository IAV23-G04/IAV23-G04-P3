/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Condicion para saber si el piano esta roto
/// </summary>
public class PianoCondition : Conditional
{
    ControlPiano piano;

    public override void OnAwake()
    {
        piano = GameBlackboard.blackBoard.piano.GetComponent<ControlPiano>();
    }

    //devuelve la informacion de la variable de control del piano
    public override TaskStatus OnUpdate()
    {
        if (piano.roto)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}
