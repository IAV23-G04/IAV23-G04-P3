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
    public bool cantando = false, capturadaPorFantasma = false;


    // Componente cacheado NavMeshAgent
    public NavMeshAgent agente;
  
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

    
    public void Start()
    {
        agente.updateRotation = false;
    }

    public void LateUpdate()
    {
        //si esta persiguiendo a alguien actualzia la posicion donde se encuentra este
        if (persiguiendo)
        {
            transform.position = objetivoPerseguir.position - objetivo.forward.normalized;
        }
            

        if (agente.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agente.velocity.normalized);
        }
    }

    // Comienza a cantar, reseteando el temporizador
    public void Cantar()
    {
        tiempoComienzoCanto = Time.timeSinceLevelLoad;
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

        nuevoObjetivo(bb.backStage.transform.position);
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
        //si estaba opersiguiendo a alguien se detiene
        if (persiguiendo)
        {
            persiguiendo = false;
            agente.enabled = true;
        }
           

        //si cumple su espacio de tiempo se desplaza hacia su nueva posicion objetivo
        if (tiempoDeMerodeo + tiempoComienzoMerodeo <= Time.timeSinceLevelLoad)
        {
            tiempoComienzoMerodeo = Time.timeSinceLevelLoad;
            nuevoObjetivo(RandomNavmeshPosition(distanciaDeMerodeo));
        }
       
    }
    public bool GetCapturada()
    {
        return capturada;
    }

    public void setCapturada(bool cap, bool porFantasma)
    {
        capturadaPorFantasma = porFantasma;
        capturada = cap;
    }

    public void sigueFantasma()
    {
        comenzarAPerseguir();
        objetivoPerseguir = fantasma;
    }

    public void sigueVizconde()
    {
        comenzarAPerseguir();
        objetivoPerseguir = vizconde;
    }
    public void irAlEscenario()
    {
        if (persiguiendo)
            dejarDePerseguir();

        agente.SetDestination(bb.stage.transform.position);
    }

    public bool enEscenario()
    {
        return Vector2.Distance(new Vector2(bb.stage.transform.position.x, bb.stage.transform.position.z), new Vector2(transform.position.x, transform.position.z)) <= 3f
            && agente.pathStatus == NavMeshPathStatus.PathComplete;
    }

    private void nuevoObjetivo(Vector3 obj)
    {
        agente.SetDestination(obj);
    }
    
    public void dejarDePerseguir()
    {
        persiguiendo = false;
        agente.enabled = true;
        agente.Warp(transform.position);
    }

    private void comenzarAPerseguir()
    {
        persiguiendo = true;
        agente.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PointerLayer"))
        {
            salaActual = collision.gameObject;
        }
    }
}
