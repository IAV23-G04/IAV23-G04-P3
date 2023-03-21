# IAV23-G04-P2

## Autores
- Raul Saavedra de la Riera (<a href=https://github.com/RaulSaavedraRiera> RaulSaavedraRiera</a>)
- Antonio Povedano Ortiz (<a href=https://github.com/AntonioPove> AntonioPove</a>)

## Propuesta
Esta práctica consiste en implementar un pequeño juego en el que controlaremos al vizconde, el cual se podrá mover por un pequeño teatro intentando salvar a la cantanate cuando esta sea secuestrada por el fantasma, además otras muchas más acciones que pueden ser realizadas por los diferentes personajes del juego.
El enunciado de esta práctica se encuentra en:
https://narratech.com/es/inteligencia-artificial-para-videojuegos/decision/historias-de-fantasmas/

El avatar del jugador será el **vizconde** el cual podremos controlar con el click izquierdo del ratón. Su objetivo será rescatar a la cantante si esta es encarcelada por el fantasma en algún momento. Además podrá arreglar la lámpara si alguna vez esta ha sido tirada por el fantasma.

El **fantasma** alternará entre acciones, entre las que se encuentrar el tirar la lámpara secuestrar a la cantante, encarcelarla, cerrar la rejas, etc.

La **cantante** alternara entre acciones al igual que el fantasma, entre estas acciones encontramos la de cantar en el escenario merodear por el mapa de manera "desorientada" y dejarse coger por diferentes personajes.

El **público** se encuentra justo delante del escenerio. Si en algún momento las lámparas se caen, todos los espectadores huirán fuera del teatro hasta que la lámpara vuelva ser colocada.

Además podremos cambiar la vista de la cámara a partir de los números 1, 2, 3 o 4, golpear a partir del espacio, usar Q para capturar a la cantante  y la E para interactuar con el elementos del escenario (palancas).

## Punto de partida
Se parte de un proyecto base de Unity proporcionado por el profesor aquí:
https://github.com/Narratech/IAV-Decision

El prototipo que se nos da consta de una plantilla donde encontramos:

-Una escena de Unity, que contiene:
    - Un mapa de juego formado por diferentes habitaciones y caminos.
    - Diferenetes elementos con los que interactuar a través del personaje (barca, palancas, etc.).
    - El personaje del jugador que se puede controlar a través del ratón.
    - Un público que se encuentra delante del escenario.
    - La cantante, fantasma y piano aún sin ningún tipo de función.


Los scripts que se nos proporcionan en dicho proyecto son varios y de diversa índole (muchos de estos scripts han sido ya utilizados en la práctica 1 de esta asignatura, https://github.com/IAV23-G04/IAV23-G04-P1):
 
 **GameManager**
 En primer lugar encontramos el script GameManager que se encarga de controlar el estado del juego y mostrar diversos valores en pantalla. Desde él podemos ajustar el FrameRate, recargar la escena y cambiar el tipo de heurística mostrada. 
 Otro métdodo relevante es el de FindGO() que obtiene los datos relevantes de la escena diferenciando entre menú y laberinto. Por ejemplo el texto de la label en el primero o el avatar en el segundo.
 Como último punto relevante señalar que posee métodos para "generar" la entrada y la salida del laberitno dadas por el GraphGrid del que hablaremos más adelante.

 Tenemos como otr pilar del proyecto los scripts de Graphs, encargadados tanto de la generación del mapa, como del cálculo de costes, su visualización...

Los dos primeros scripts son simples y su función principal es almacenar y comparar información sobre el mapa donde se calcularán las rutas.

Los siguientes scripts están relacionados con el funcionamiento de las acciones y movimiento del personaje del fantasma, además de accines realicionadas con la lámpara, cantante y bizconde:

**CantanteCondition**
    Script que hereda de conditional, encargado de comprobar si la acción de la cantante, en este caso que se encuentre cantando, se haya realizado de manera correcta o por si el contrario esta acción ha sido un fallo.

**CapturadaCondition**
    Script que hereda de conditional, encargado de comprobar si la acción de capturar a la cantante ha sido realizada con éxito o no. Además es el encargado de realizar esta misma acción.

**GhostArreglaPianoAction**  
    Script que hereda de action, se encarga de realizar la acción de arreglar el piano y devuelve si esta acción se esta realizando o no.

**GhostChaseAction**
    Script que hereda de action, se encarga de hacer que el personaje del fantasma siga a la cantante para poder capturarla. Además devuelve si esta acción se está realizando o no. 

**GhostCloseDoorAction**
    Script que hereda de action, encargado de cambiar la posicion de la puerta (cerrarla y abrirla), cuando el personaje del fantasma interactue con esta.

**GhostLlevarCantante**
    Script que hereda de action, encargado de hacer que el fantasma capture a la cantante. Comprueba la posición de ambos para ver si la acción puede ser realizada o no, y hace que la cantante se mueva a la misma vez que el fantasma. 

**GhostReturnAction**
    Script que hereda de action, encargado de hacer que el fantasma vuelva a la sala de música y devuelve si la aciión ha sido completada con éxito.

**GhostSearchRandomAction**
    Script que hereda de action, encargado de hacer que el fantasma vuelva se mueva a salas aleatorias del mapa a partir de la asignación de GameBlackBoard, y cuando el fantasma esté en esta nueva ubicación devolverá si la acción ha sido realizada con éxito.

**GhostSearchStageAction**
    Script que hereda de action, encargado de hacer que el fantasma vaya al escenario y devolverá si la acción ha sido realizada con éxito.

**ImprisonedCondition**
    Script que hereda de conditional, encargado de comprobar si la acción de la cantante, en este caso que se encuentre encarcelada, se haya realizado de manera correcta o por si el contrario esta acción ha sido un fallo.

**PianoCondition**
    Script que hereda de conditional, encargado de comprobar si el piano está siendo tocado por el fantasma o no.

**PublicoCondition**
    Script que hereda de conditional, encargado de comprobar ambas partes del público (este y oeste) y de comprobar las acciones que estos están realizando.

**VizcondeChocaCondition**
    Script que hereda de conditional, encargado de comprobar las acciones del vizconde cuando la cantante se encuentra en el palco.

**Vertex**
    Script simple que guarda un punto de la ruta. Su id y su coste conforman la información que posee. Permite comparar con objetos y otros vértices así como comparalos para ver cual posee un coste menor.

 **Node**
    Script simple que es usado para almacenar la información de un nodo. Guarda su vertexID, el anterior a este, el coste hasta ahora y el aproximado. Hay métodos de comparació entre nodos y en resumen su infunción es la de almacenar la información de un punto en el "mapa".

Otro grupo de script importante es el de los Grafos. Contienen el mapa a nivel lógico y lo generan en el espacio de juego.

**Graph**
    Es una clase abstracta, por lo que su objetivo es ser usada como base para crear grafos sobre ella.
    Contiene variables que almacenan todos los vértices, sus vértices vecinos, los costes de estos, el tamaño... En definitiva: almacena toda la información del mapa.
    Contiene un métodos virtuales varios para la gestión de los vértices y sus costes, así como para el mapa:
        -Load() para cargar los mapas.
        -UpdateVertexCosts() para actualizar valores de los vértices respecto al mapa.
        -GetNearestVertex() para obtener el vértice más cercano y GetRandomPos() para obtener ua posición aleatoria del mapa.
        -GetNeighbourdsCosts() para obtener los costes de los vecinos desde un vértice.
    A su vez cuenta con métodos para calcular caminos óptimos mediante diferentes algoritmos: BFS, DFS y PathAstar o A*. Para suavizarlos con SMooth() y BuildPath() para reconstruir los caminos realizados.   

**GraphGrid**
    Es una clase que hereda de Graph y como tal cuenta con las funciones que esta  traía consigo.
    Sobreescribe el método Load() y usando diferentes prefabs que posee como variables genera un mapa jugable.
    A su vez en esta función llama a métodos del GameManager para definir la entrada y la salida.
    Crea el método SetNeighbourds para definir los vecinos de cada uno de los vértices durante el métodod Load().
    Crea el método WallInstantiate() para generar aquellas zonas donde no hayas vértices navegables, poniendo muros para crear el laberinto, tanto visualmente como de gameplay, deseado.
    Sobreescribe los métodos GeNearestVertex(), UpdateVertexCost() y GetRandomPos() para que se adapten al mapa establecido.

**TheseusGraph**
    Es un script encargado de calcular y dibujar el camino óptimo de un punto a otro. Pudiendo usar diferentes heurísticas y algoritmos de búsqueda. De base se da por sentado 2 heurísticas y los 3 algoritmos de búsqueda ya comentados.
    Como variables base posee unas que debe da rel desarrollador como el tipo de algoritmo con el que se debe calcular, si hay que suavizar la ruta, los tags de los nodos, color de la ruta y el radio del nodo. Y otras que son tomadas del GameObject como la cámara o el lineRender y otras que pueden variar como la heurística en uso.
    Ariadna es el nombre que se le da al comportamiento donde genera la mejor ruta desde el nodo actual hasta el destino.
    En el Update podemos cambiar si usamos a Ariadna o no y en caso positivo se calcula dicha ruta y se muestra en el mapa.
    En el OnDrawGizmos se dibuja  esferas sobre los puntos de la ruta específicados.
    Posee métodos como GetNextNode(), GetNodeFromScreen() para obtener nodos.
    ShowPathVertices() recalca el camino calculado y DIbujaHilo() muestra el hilo sobre el amap. Siendo ambos métodos relevantes para la visión.

EL siguiente conjunto de scripts relevantes relacionados con IA es el de los comportamientos Agente usando la msima estructura que los vistos en la práctica anterior.
De nuevo los scripts bases para esta estructura son dos:

**Agente**
    Igual al de la práctica anterior, se adjunta comentario:
    Clase que se encarga de la interacción entre la inteligencia artificial desarrollada y el medio "físico" de la escena mediante el movimiento. Contiene referencias al rigidbody, y variables para ajustar la velocidad, velocidad angular... Todos los componentes necesarios para poder desarrollar adecuadamente los movimientos deseados.
    A su vez, como apoyo para el Comportamiento Agente guarda variables para identificar si la combinación de acciones se da por peso o prioridad, y métodos como GetPrioridadDireccion llamados en su LateUpdate para la selección de movimiento.
    Por último, y siguiendo su rol de conexión Comportamiento-Medio tiene métodos auxiliares como LookDirection o OriToVec que ayudan a la hora de obtener direcciones o giros. Métodos que son llamados en Update y FixedUpdate donde se encarga de realziar todas las comprobaciones físicas pertienntes.

**ComportamientoAgente**
    Igual al de la práctica anterior, se adjunta comentario:
    Clase abstracta que sirve como plantilla para la creación de los diferentes comportamientos desarrollados. Tiene referencia a un script Agente para poder repercutir en el movimeinto del jugador y tomar los datos que esta proporciona. A su vez cuenta con una referencia común para los diferentes comportamientos como es un Objetivo.
    Cuenta con métodos para un mayor control de RigidBody y su relación con el mundo de Unity como OriToVector o RadianesAGrados.
    Además de esto solo cabe recalcar que en su Update hace referencia a la forma de ponderación de que dirección tomar dependiendo de peso, prioridad o simple según haya sido marcado en Agente.

Ambos usan como apoyo la siguiente clase para representar las fuerzas y direcciones:

**Direccion**
    Igual al de la práctica anterior, se adjunta comentario:
    script que se utiliza de apoyo para representar la dirección y que en código, sea más sencilla de utilizar.

Otro script alejado de los otros grupos de comportamiento e igual al de la práctica anterior es el de ControlJugador:

**ControlJugador**:
    Es un comportamiento "especial" ya que no es IA. Si no que se encarga de recoger la entrada de teclado y parsearla para que pueda ser procesado por Agente y ComportamientoAgente de manera correcta. Siendo el juagdor el que toma la decisión de hacia que dirección desplazarse.
    Lo más relevante de esta clase es que sobreescribe GetDirection para obtener la dirección mediante la entrada de teclado.

Una vez visto estos scripts base hablaremos de los diferentes scripts que implementan multitud de comportamientos.

**LLegada**
    Su principal función es la de una vez se llegue a una distancia del objetivo se reduzca la fuerza de desplazamiento para poder lograr una llegada a este más pausa y realista en lugar de una instántanea menos realista.
    Contiene un comportamiento Avoidance() para evitar la colisión con objetos cuando se encuentre muy cerca del objetivo aunque no esta usada.

**Merodear**
    Su principal función es obtener una dirección aleatoria y desplazarse en ella durante un tiempo determinado. Cuenta con una función de retroceso en caso de colisión con un obstáculo.

Otro conjunto relevante son los relacionados con el minotauro, indpeendientes de agente u otro grupo, que se encargan de gestionar diferentes eventos con los minotauros o generarlos.

**MinoCollision**
    Script independiente al agente que reinicia el nivel si el minotauro golpea con Teseo.

**MinoEvader**
    Script independiente al agente que se encarga de en caso de obtener un evento trigger con un objeto y, en caso de que este sea el minotauro, rehacer la ruta.

**MinoManager**
    Encargado de generar el número de minotauros en el laberinto y con una posición aleatoria dentro del laberinto.

Otro conjunto de scripts de comportamiento son aquellos relacionados directamente con el jugador además del ControlJugador:

**Slow**
    Se encarga de modificar la velocidad máxima cuando el juagdor se acerce al trigger, al área, del minotauro.

**SeguirCamino**
    Su función es la de, como su nombre indica, seguir un camino. Dado un T nodo sigueinte se desplaza hacia él y tiene una función para setear el Path del TheseusGraph del que obtiene los nodos entendemos.

**Teseo**
    Es el que controla la forma en la que se desplaza el jugador que en este caso hace de Teseo. Permite elegir entre seguir el camino de forma automática o desplazamiento por ej jugador. Tiene referencias tanto a SeguirCamino como a ControlJugador para activarlos/desactivarlos.

Estos serían relacionados con la inteligencia artificial y el mapa. Ahora hablaremos de otros scripts auxiliares como los de animación y los extras:

**AnimalAnimationController**
Encargado de controlar y cambiar el estado de la animación del jugador (está moviendose o esta quieto) dependiendo de la velocidad que tenga este en cada momento. 

**CameraFollow**
Encargado de que la cámara tenga siempre el jugador en el centro de esta, y también se encarga de las diferentes interacciones que podemos realizar en esta, asi como aplicar un zoom.

**PlayerAnimation**
Encargado de ajustar la velocidad del jugador para que el animator pueda usarla correctamente.

**BinaryHeap**
Encargado de implemnetar el montículo binario para poder utilizarlo en la práctica ya que Unity no tiene uno implementado de base.

**DropDown**
Encargado de pasar y cambiar los datos de creación de escenario (tamaño) y el número de minotauros que se quieren establecer en cada nivel.

Tenemos la estrcutura base del proyecto como en la práctica anterior pero con muchos scripts, los relacionados al comportamiento, incompletos; que deberemos de completar junto a aquellos que añadamos nosotros mismo para lograr lso objetivos propuestos para esta práctica.

## Diseño de la solución

Lo que vamos a realizar para resolver esta práctica es implementar los diferentes comportamientos explicados anteriormente para los diferentes elementos del juego.
Para los algoritmos que implementaremos nos basaremos en los pseudocódigo de Millington.

El pseudocódigo del algoritmo de seguimiento es:

```
class KinematicSeek:
character: Static
target: Static

maxSpeed: float

function getSteeringl -> KinematicSteering0utput:
    result = new KinematicSteering0utput( )

    #Get the direction to the target.
    result.velocity = target.position - character.position

    #The velocity is along this direction, at full speed.
    result .velocity.normalize( )
    result.velocity *= maxSpeed

    #Face in the direction we want to move.
    character.orientation = newOrientation(
    character .orientation,
    result,velocity)

result.rotation = 0 
return result
```

El pseudocódigo del algoritmo de merodeo es:

```
class Kinematiciander:
character: Static
maxSpeed: float

# The maximum rotation speed we"d like, probably should be smaller
# than the maximum possible, for a letsurely change in direction.
maxRotation: float

function getSteering() -> KinematicSteer ingOutput:
    result = new KinematicSteer ing0utput( )

    #Get velocity from the vector form of the orientation.
    result.velocity = maxSpeed * character .orientation.asVector( )

    #Change our orientation randomly.
    result.rotation = randomBinonial() * maxRotation

    return result
```

## Pruebas y métricas

Aquí se podrán los diferentes videos de las pruebas y partes del proyecto para ir documentando su funcionalidad:

## Ampliaciones

    Pondremos las modificaciones externas a la práctica cuando estas sean realizadas

## Producción

Las tareas se han realizado y el esfuerzo ha sido repartido entre los autores.

| Estado  |  Tarea  |  Persona  |  
|:-:|:--|:-:|
| ✔ | Readme explicacion de clases | Raul |
| ✔ | Readme explicacion de clases del fantasma y propuesta | Antonio |
| OPCIONAL|

Todo se ha hecho junto ya que residimos en la misma vivienda


## Referencias

Los recursos de terceros utilizados son de uso público.

- *AI for Games*, Ian Millington. Hemos obtenido de aqui los pseudocódigos que se van a usar para implementar estar práctica.
- Diapositivas sacadas de la página: https://narratech.com/es/category/sector_es/informatica/
- [Kaykit Medieval Builder Pack](https://kaylousberg.itch.io/kaykit-medieval-builder-pack)
- [Kaykit Dungeon](https://kaylousberg.itch.io/kaykit-dungeon)
- [Kaykit Animations](https://kaylousberg.itch.io/kaykit-animations)

