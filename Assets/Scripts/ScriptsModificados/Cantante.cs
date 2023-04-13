using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


/// <summary>
/// Clase de la Cantante suministrada y que gestiona todo lo relacionado con el estado y acciones de este personaje.
/// Se ha dado aleatoriedad, implementada en la máquina de estados, al tiempo de descanso para dar más variedad en la partida.
/// Se han añadido diferentes métodos para facilitar el acceso a ciertas funciones.
/// Se han implementado dos sistemas de "deambular", uno donde viaja de sala en sala que es el usado y uno en distancias más cortas 
/// que hemos descartado por ser poco realista
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
    //si esta persiguiendo a alguien
    bool persiguiendo = false;
    //si esta en la celda
    bool inJail = true;

    [Range(0, 180)]
    // Angulo de vision en horizontal
    public double anguloVistaHorizontal;
    // Distancia maxima de vision
    public double distanciaVista;

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
    Transform objetivoPerseguir;

    //para tiempo descanso actual
    double tiempoDeDescanso;

    //guarda la sala donde se encuentra
    GameObject salaActual;

   //ref a la cancion para reproducirla 
    AudioSource cancion;


    //variables iniciales
    public void Start()
    {
        agente.updateRotation = false;

        cancion = GetComponent<AudioSource>();
    }

    public void LateUpdate()
    {
        //si esta persiguiendo a alguien actualzia la posicion donde se encuentra este
        if (capturada)
        {
            transform.position = objetivoPerseguir.position - objetivoPerseguir.forward.normalized * 1.5f;
        }
        //si esta diriguiendose a una ubicación y no esta encerrada se desplaza el agente a ella
        if (persiguiendo && !inJail)
        {
            agente.SetDestination(objetivoPerseguir.position);
        }
        //en casod de un movimiento relevante se ajusta su rotacion
        if (agente.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agente.velocity.normalized);
        }
    }

    // Comienza a cantar, reseteando el temporizador
    public void Cantar()
    {
        //se desactiva que este persiguiendo para evitar que se mueva
        persiguiendo = false;
        //se reinicia el temporizador  
        tiempoComienzoCanto = Time.timeSinceLevelLoad;
        // mira al publico
        transform.rotation = Quaternion.LookRotation(new Vector3(bb.patioButacas.transform.position.x,
            transform.position.y, bb.patioButacas.transform.position.z));
        cantando = true;
        //reproduce la cancion
        cancion.Play();
    }

    // Comprueba si tiene que dejar de cantar
    public bool TerminaCantar()
    {
        return Time.timeSinceLevelLoad >= tiempoComienzoCanto + tiempoDeCanto;
    }

    // Comienza a descansar, reseteando el temporizador
    public void Descansar()
    {
        //deja de cantar
        cantando = false;
        cancion.Stop();
        //actualiza el tiempo de descanso
        tiempoDeDescanso = Random.Range((float)tiempoDeDescansoMin, (float)tiempoDeDescescansoMax);
        tiempoComienzoDescanso = Time.timeSinceLevelLoad;
        //lo desplaza al backstage
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
        if (Physics.Raycast(transform.position, dir, dist - 0.5f))
            return false;
        //en caso contrario si que lo esta viendo
        if (inJail) inJail = false;
        return true;
    }

    // Genera una posicion aleatoria a cierta distancia dentro de las areas permitidas
    private Vector3 RandomNavmeshPosition(float distance)
    {
        //var necesarias
        Vector3 randomPos;
        NavMeshHit navMeshHit;
        //obtiene una posicion aleatoria dentro de un area
        do
        {
            randomPos = Random.insideUnitSphere * distance + transform.position;

            //y busca una nueva en caso de estar fuera del navmesh
        } while (!NavMesh.SamplePosition(randomPos, out navMeshHit, distance, NavMesh.AllAreas));

        return navMeshHit.position;
    }

    // Genera un nuevo punto de merodeo cada vez que agota su tiempo de merodeo actual
    public void IntentaMerodear()
    {
        //si estaba opersiguiendo a alguien se detiene
        if (capturada)
            finCaptura();


        //si cumple su espacio de tiempo se desplaza hacia su nueva posicion objetivo
        if (tiempoDeMerodeo + tiempoComienzoMerodeo <= Time.timeSinceLevelLoad)
        {
            //NavMeshHit navMeshHit;
            //NavMesh.SamplePosition(transform.position, out navMeshHit, 100, NavMesh.AllAreas);

            //transform.position = navMeshHit.position + Vector3.up * offset;
            //agente.Warp(navMeshHit.position + Vector3.up * offset);

            //actualizamos tiempo de merodeo
            tiempoComienzoMerodeo = Time.timeSinceLevelLoad;
            //y si es posible el agente se desplaza a una habitsacion aleatoria
            if (agente.enabled)
                nuevoObjetivo(bb.getRandomSitio().transform.position);
            //movimiento aleatorio basado en proximidad
            //nuevoObjetivo(RandomNavmeshPosition(distanciaDeMerodeo));
        }

    }

    //si esta capturada
    public bool GetCapturada()
    {
        return capturada;
    }
    //método llamado cuando se captura o libera a la cantante
    public void setCapturada(bool cap, bool porFantasma = false)
    {
        //se ajustan las variables para conocer el estado de captura y por parte de quein
        capturadaPorFantasma = porFantasma;
        capturada = cap;
        persiguiendo = false;

        //si esta capturada
        if (capturada)
        {
            //se elimina el estado de canto en caso de ser necesaario
            cantando = false;
            cancion.Stop();
            //activa el seguimiento automatico del objetivo correspondiente
            if (capturadaPorFantasma)
                sigueFantasma();
            else
                sigueVizconde();
        }
        //si no esta capturada se llama al metodo correspondiente
        else
            finCaptura();

    }

    //desactiva el agente para que el seguimiento sea automatico y ajusta el objetivo
    void sigueFantasma()
    {
        agente.enabled = false;
        objetivoPerseguir = fantasma;
    }
    //similar al anterior
    void sigueVizconde()
    {
        if (inJail) inJail = false;
        agente.enabled = false;
        objetivoPerseguir = vizconde;
    }

    //persigue al vizconde pero en etse caso mediante el navmesh
    public void perseguirVizconde()
    {
        //activa la variable que controla la persecucion
        persiguiendo = true;
        objetivoPerseguir = vizconde.transform;
    }

    //método usado para dirigirse al escenario para cantar
    public void irAlEscenario()
    {
        //comprobacion de seguridad
        if (capturada)
            finCaptura();
        //ajusta el destino del agente al escenario
        agente.SetDestination(bb.stage.transform.position);
    }
    //si ha llegado al escenario, objetivo de su ruta
    public bool enEscenario()
    {
        return Vector2.Distance(new Vector2(bb.stage.transform.position.x, bb.stage.transform.position.z), new Vector2(transform.position.x, transform.position.z)) <= 3f
            && agente.pathStatus == NavMeshPathStatus.PathComplete;
    }
    //ajusta un nuevo objetivo al que dirigirse
    //es un objetivo estatico que no debe actualzia su pposicion por lo que para ahorrar recursos desactiva el persiguiendo
    private void nuevoObjetivo(Vector3 obj)
    {
        persiguiendo = false;
        agente.SetDestination(obj);
    }
    //llamado cuando se termina la captura
    public void finCaptura()
    {
        //desactiva la captura
        capturada = false;
        //da un tiempo d emargen para reactivar el movimiento
        Invoke("reactivarMovimiento", 1f);
    }

    public void enPrision()
    {
        inJail = true;
    }

   //activa el agente y ajusta la posicion del agente a la de su transform
    void reactivarMovimiento()
    {

        agente.enabled = true;
        agente.Warp(transform.position);
    }

    //almacena en que sala se encuentra en cada momento cuando cambia de sala para saber si la conoce
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PointerLayer"))
        {
            salaActual = other.gameObject;
        }
    }
}
