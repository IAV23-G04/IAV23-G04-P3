using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


/// <summary>
/// Clase de la Cantante suministrada y que gestiona todo lo relacionado con el estado y acciones de este personaje.
/// Se ha dado aleatoriedad, implementada en la máquina de estados, al tiempo de descanso para dar más variedad en la partida.
/// </summary>
public class Cantante : MonoBehaviour
{
    // Segundos que estara cantando
    public double tiempoDeCanto;
    // Segundo en el que comezo a cantar
    private double tiempoComienzoCanto;
    // Segundos que esta descanasando
    public double tiempoDeDescansoMin, tiempoDeDescescansoMax;
    // Segundo en el que comezo a descansar
    private double tiempoComienzoDescanso;
    // Si esta capturada actualmente
    public bool capturada = false;

    [Range(0, 180)]
    // Angulo de vision en horizontal
    public double anguloVistaHorizontal;
    // Distancia maxima de vision
    public double distanciaVista;
    // Objetivo al que ver"
    public Transform objetivo;

    // Segundos que puede estar merodeando
    public double tiempoDeMerodeo;
    // Segundo en el que comezo a merodear
    public double tiempoComienzoMerodeo = 0;
    // Distancia de merodeo
    public int distanciaDeMerodeo = 16;
    // Si canta o no
    public bool cantando = false;

    // Componente cacheado NavMeshAgent
    private NavMeshAgent agente;

    // Objetivos de su itinerario
    public Transform Escenario;
    public Transform Bambalinas;

    // La blackboard
    public GameBlackboard bb;

    //para seguir al fantasma o al vizconde
    public Transform fantasma, vizconde;
    //si esta siguiendo a alguien para actualizar el objetivo
    bool persiguiendo = false;
    Transform objetivoPerseguir;

    //para tiempo descanso actual
    double tiempoDeDescanso;

    //guarda la sala donde se encuentra
    GameObject salaActual;

    public void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    public void Start()
    {
        agente.updateRotation = false;

        bb = GameBlackboard.blackBoard;
    }

    public void LateUpdate()
    {
        //si esta persiguiendo a alguien actualzia la posicion donde se encuentra este
        if (persiguiendo)
            agente.SetDestination(objetivoPerseguir.position);

        if (agente.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agente.velocity.normalized);
        }
    }

    // Comienza a cantar, reseteando el temporizador
    public void Cantar()
    {
        tiempoComienzoCanto = 0;
        cantando = true;
    }

    // Comprueba si tiene que dejar de cantar
    public bool TerminaCantar()
    {
        return Time.timeSinceLevelLoad >= tiempoComienzoCanto + tiempoDeCanto;
    }

    // Comienza a descansar, reseteando el temporizador
    public void Descansar()
    {
        tiempoDeDescanso = Random.Range((float)tiempoDeDescansoMin, (float)tiempoDeDescescansoMax);
        tiempoComienzoDescanso = Time.timeSinceLevelLoad;

        nuevoObjetivo(Bambalinas.position);
        // IMPLEMENTAR
    }

    // Comprueba si tiene que dejar de descansar
    public bool TerminaDescansar()
    {
        return Time.timeSinceLevelLoad >= tiempoComienzoDescanso + tiempoDeDescanso;
    }

    // Comprueba si se encuentra en la celda
    public bool EstaEnCelda()
    {
        return salaActual == bb.celda;
    }

    // Comprueba si esta en un sitio desde el cual sabe llegar al escenario
    public bool ConozcoEsteSitio()
    {

        return salaActual == bb.backStage || salaActual == bb.stage || salaActual == bb.patioButacas || salaActual == bb.stage;
    }

    //Mira si ve al vizconde con un angulo de vision y una distancia maxima
    public bool Scan(float ang, float distMax)
    {
        //no lo ve si:
        //si esta mas lejos de la distancia maxima
        float dist = Vector3.Distance(transform.position, vizconde.position);
        if (dist > distMax)
            return false;

        Vector3 dir = (vizconde.position - transform.position).normalized;
        //o fuera del angulo
        if (Vector3.Angle(transform.forward, dir) < ang / 2)
            return false;
        //o hay obstaculos entre medias, reducimos la distancia para que ignore la colision con el vizconde
        if (Physics.Raycast(transform.position, dir, dist -0.5f))
            return false;
        //en caso contrario si que lo esta viendo
        return true;
    }

    // Genera una posicion aleatoria a cierta distancia dentro de las areas permitidas
    private Vector3 RandomNavmeshPosition(float distance)
    {
        Vector3 randomPos;
        NavMeshHit navMeshHit;
        do
        {
            randomPos = Random.insideUnitSphere * distance + transform.position;

        } while (!NavMesh.SamplePosition(randomPos, out navMeshHit, distance, NavMesh.AllAreas));

        return navMeshHit.position;
    }

    // Genera un nuevo punto de merodeo cada vez que agota su tiempo de merodeo actual
    public void IntentaMerodear()
    {
        nuevoObjetivo(RandomNavmeshPosition(4));
    }
    public bool GetCapturada()
    {
        return capturada;
    }

    public void setCapturada(bool cap)
    {
        capturada = cap;
    }

    public void sigueFantasma()
    {
        persiguiendo = true;
        objetivoPerseguir = fantasma;
    }

    public void sigueVizconde()
    {
        persiguiendo = true;
        objetivoPerseguir = vizconde;
    }

    private void nuevoObjetivo(Vector3 obj)
    {
        if (persiguiendo)
            persiguiendo = false;

        agente.SetDestination(obj);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PointerLayer"))
        {
            salaActual = collision.gameObject;
        }
    }
}
