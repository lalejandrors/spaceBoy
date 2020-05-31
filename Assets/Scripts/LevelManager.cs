using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour//esta clase va a gestionar los bloques de los niveles... agregar y quitar bloques
{
    public List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();//listado de todos los bloques disponibles a generarse
    public List<LevelBlock> currentLevelBlocks = new List<LevelBlock>();//listado de todos los bloques que se estan utilizando en el momento
    public Transform levelStartPosition;

    private int previousLevelBlockIdx = 1000, randomIdx = 0;

    //SINGLETON///////////////////////////////////////////////
    public static LevelManager sharedInstance;//define la palabra singleton, osea una unica instancia de esta clase en todo el juego que nunca cambia (static) y se define en el awake del mismo. Por medio de esta instancia puedo acceder a los metodos y variables de esta clase desde cualquier otro script

    void Awake()//se ejecuta antes del start, osea antes del primer frame
    {
        if (sharedInstance == null)
        {//para asegurarnos que no se vaya a definir esta instancia de la clase más de una vez
            sharedInstance = this;//la referencia a la clase actual
        }
    }
    ///END SINGLETON///////////////////////////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
        GenerateInitialBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddLevelBlock() { //funcion que añade el nuevo bloque dinamicamente segun como sea necesario
        while (randomIdx == previousLevelBlockIdx) {
            randomIdx = Random.Range(1, allTheLevelBlocks.Count);//para que aleatoreamente la funcion escoja que bloque del nivel se va a mostrar excepto el primero
        }
        
        LevelBlock block;//la instancia del bloque que se va a generar
        Vector3 spawnPosition = Vector3.zero;//es la posicion de donde se va a colocar cada nuevo bloque

        if (currentLevelBlocks.Count == 0)//si aun no se agrego el primer bloque, que se agregue siempre el de posicion 0
        {
            block = Instantiate(allTheLevelBlocks[0]);//Instantiate "Optimiza creando clones de los prefabs"
            spawnPosition = levelStartPosition.position;//el primer bloque va a quedar en la posicion del level start position
        }
        else {//si ya se genero el primer bloque
            block = Instantiate(allTheLevelBlocks[randomIdx]);//Instantiate "Optimiza creando clones de los prefabs"
            spawnPosition = currentLevelBlocks[currentLevelBlocks.Count - 1].exitPoint.position;//esto ubica el nuevo bloque justo al final del que se habia generado antes
        }

        block.transform.SetParent(this.transform,false);//agregarle al bloque nuevo su padre (LevelManager... por eso es this), y el false indica que las transformaciones de los nuevos bloques NO seran relativas a las de su padre
        Vector3 correction = new Vector3(spawnPosition.x - block.startPoint.position.x, spawnPosition.y - block.startPoint.position.y, 0);//se hubica el nuevo bloque en el punto final del anterior generado
        block.transform.position = correction;//asignamos la posicion final que tendra el nuevo bloque de nivel
        currentLevelBlocks.Add(block);//agregamos el nuevo bloque al array de bloques generados dinamicamente

        previousLevelBlockIdx = randomIdx;//la variable ya toma el valor del ultimo randomIdx
    }

    public void RemoveLevelBlock()//ocurre cada vez que cruzamos una exitZone
    {
        LevelBlock oldBlock = currentLevelBlocks[0];//inicializamos el bloque a destruir
        currentLevelBlocks.Remove(oldBlock);//lo eliminamos del listado
        Destroy(oldBlock.gameObject);//destruimos ese gameObject
    }

    public void RemoveAllLevelBlocks() {//para eliminar todos los bloques
        while (currentLevelBlocks.Count > 0) {//mientas haya bloques que me elimine el primero en la lista
            RemoveLevelBlock();
        }
    }

    public void GenerateInitialBlocks() {//genera los bloques de manera dinamica
        for (int i=0; i < 2; i++) {//indicamos la cantidad de bloques que queremos que se generen (Deben haber por lo menos 3 definidos). La cantidad en el if debe ser minimo 2 para que inicialmente genere el 0 y luego entre el 1 y 2 escoja cual sigue despúes. Nunca el 0 se repite de nuevo
            AddLevelBlock();
        }
    }
}
