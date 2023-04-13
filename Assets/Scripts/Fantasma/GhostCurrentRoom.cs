using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Se encarga de saber en que sala se encuentra el fantasma. En caso de que sea el escenario principal 
y no tenga agarrada a la cantante se hace invisible para simular que ha ido por arriba del escenario ocultandose.
 */
public class GhostCurrentRoom : MonoBehaviour
{
    public Cantante cantante;
    public GameObject salaActual;

   
    private void OnTriggerEnter(Collider other)
    {
        //usando un nuevo colider de tipo trigger si este colisiona con una nueva sala
        if (other.gameObject.layer == LayerMask.NameToLayer("PointerLayer"))
        {
            //actualiza la sala actual para tener referencia
            salaActual = other.gameObject;
            //y lo hace invisible si requiere
            if (salaActual == GameBlackboard.blackBoard.stage && (!cantante.capturada || !cantante.capturadaPorFantasma))
                transform.GetChild(0).gameObject.SetActive(false);
            else
                transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
