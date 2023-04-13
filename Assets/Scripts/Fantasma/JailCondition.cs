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

/*
 * Condicion de si la puerta esta cerrada
 */

public class JailCondition : Conditional
{
    GameBlackboard blackboard;

    public override void OnAwake()
    {
        blackboard = GameBlackboard.blackBoard;
    }

    //se obtiene mediante la variable de control del blackboard
    public override TaskStatus OnUpdate()
    {
        if (blackboard.gate)
            return TaskStatus.Success;

        return TaskStatus.Failure;
    }
}