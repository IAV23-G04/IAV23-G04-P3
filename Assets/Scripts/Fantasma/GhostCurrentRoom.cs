using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCurrentRoom : MonoBehaviour
{
    public Cantante cantante;
    public GameObject salaActual;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PointerLayer"))
        {
            salaActual = other.gameObject;

            if (salaActual == GameBlackboard.blackBoard.stage && (!cantante.capturada || !cantante.capturadaPorFantasma))
                transform.GetChild(0).gameObject.SetActive(false);
            else
                transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
