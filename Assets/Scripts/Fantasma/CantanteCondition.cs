/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;


/// <summary>
/// Condicional para saber si la cantante esta cantando en el momento obteniendo una referencia a su script
/// </summary>

public class CantanteCondition : Conditional
{

    Cantante cantante;

    public override void OnAwake()
    {
        cantante = GameObject.FindGameObjectWithTag("Cantante").GetComponent<Cantante>();
    }

    public override TaskStatus OnUpdate()
    {
        if (cantante.cantando)
            return TaskStatus.Success;

        return TaskStatus.Failure;
    }
}
