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

/// <summary>
/// Condicion de si la cantante esta encerrads
/// </summary>

public class ImprisonedCondition : Conditional
{
    GameBlackboard blackboard;

    public override void OnAwake()
    {
        blackboard = GameBlackboard.blackBoard;
    }

    //mediante la variable del blackboard correspondiente
    public override TaskStatus OnUpdate()
    {
        if (blackboard.imprisoned)
            return TaskStatus.Success;

        return TaskStatus.Failure;
    }
}