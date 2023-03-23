using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Se encarga de controlar si el público debería huir o quedarse en el patio de butacas.
 * Al ser un comportamiento muy simple no requiere de máquinas de estado y solo necesita
 * cambiar el objetivo del agente entre su posición en el escenario y el lobby.
 */

public class Publico : MonoBehaviour
{
    //int lucesEncendidas = 2;
    bool miLuzEncendida;
    bool sentado = true;

    //GameObject luzAsociada;
    NavMeshAgent agent;
    Vector3 posStage;
    BoxCollider lobbyZone;
    private void Start()
    {
        //lucesEncendidas = 2;
        agent = GetComponent<NavMeshAgent>();
        sentado = true;
        posStage = transform.position;
        lobbyZone = GameBlackboard.blackBoard.lobby.GetComponent<BoxCollider>();
    }

    public void LateUpdate()
    {
        //para que rote hacia donde se mueve
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(GetComponent<NavMeshAgent>().velocity.normalized);
        }
        else if(miLuzEncendida)  //para que al llegar a su butaca miren hacia delante(el escenario)
            transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public bool getLuces()
    {
        return sentado;
    }

    public void apagaLuz()
    {
        miLuzEncendida = false;
        sentado = false;
        agent.SetDestination(GetRandomPosInLobby());
        //lucesEncendidas--;
        //sentado = lucesEncendidas == 2;
    }
    //se llama cuando el fantasma o el vizconde desactivan o activan las luces
    public void enciendeLuz()
    {
        agent.SetDestination(posStage);
        miLuzEncendida = true;
        sentado = true;
        // lucesEncendidas++;
        //sentado = lucesEncendidas == 2;
    }

    Vector3 GetRandomPosInLobby()
    {
        return lobbyZone.transform.position + new Vector3(Random.Range(-lobbyZone.size.x / 3, lobbyZone.size.x / 3), transform.position.y, Random.Range(-lobbyZone.size.z / 3, lobbyZone.size.z / 3));
    }

}
