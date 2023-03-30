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

public class PublicoCondition : Conditional
{
    GameBlackboard blackboard;

    [SerializeField] bool publicoWest;
    [SerializeField] bool publicoEast;

    ControlPalanca palancaWest;
    ControlPalanca palancaEast;

    public override void OnAwake()
    {
        palancaEast = GameBlackboard.blackBoard.eastLever.GetComponentInChildren<ControlPalanca>();
        palancaWest = GameBlackboard.blackBoard.westLever.GetComponentInChildren<ControlPalanca>();
    }

    public override TaskStatus OnUpdate()
    {

        publicoEast = palancaEast.caido;
        publicoWest = palancaWest.caido;

        if(publicoWest || publicoEast)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
