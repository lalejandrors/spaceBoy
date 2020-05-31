using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { //es un enumerador global que puede ser accedido desde cualquier lugar del juego. Por eso lo dejamos fuera de la clase
    menu,
    inGame,
    gameOver
}

public class GameManager : MonoBehaviour//clase que maneja los estados del game
{
    public GameState currentGameState = GameState.menu;//una variable de tipo gameState (el enum que creamos afuera) inicializada con el valor de menu
    private PlayerController controller;//la referencia a PlayerController para conectar con esa clase

    public int collectedObject = 0;//el contador de las monedas recolectadas

    //las instancias para los sonidos. Es un array porque agregamos al player mas de 1 sonido
    AudioSource[] sounds;
    AudioSource sound1;//start
    AudioSource sound2;//gameOver

    //SINGLETON///////////////////////////////////////////////
    public static GameManager sharedInstance;//define la palabra singleton, osea una unica instancia de esta clase en todo el juego que nunca cambia (static) y se define en el awake del mismo. Por medio de esta instancia puedo acceder a los metodos y variables de esta clase desde cualquier otro script

    void Awake()//se ejecuta antes del start, osea antes del primer frame
    {
        if (sharedInstance == null) {//para asegurarnos que no se vaya a definir esta instancia de la clase más de una vez
            sharedInstance = this;//la referencia a la clase actual
        }    
    }
    ///END SINGLETON///////////////////////////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("player").GetComponent<PlayerController>();//con la referencia a la clase, usamos el elemento de unity "GameObject" que permite localizar elementos del juego por nombre, tag, etc... y obtenemos su componente de PlayerController
        sounds = GetComponents<AudioSource>();//getComponents porque son varios
        sound1 = sounds[0];
        sound2 = sounds[1];
    }

    // Update is called once per frame
    void Update()
    {
        //Si estamos en el juego y presiono enter se va a menu, y si estoy en menu y presiono enter, se va al juego. Si estoy muerto y presiono enter, se reinicia el juego
        if (this.currentGameState == GameState.inGame)
        {
            if (Input.GetButtonDown("Submit"))
            {
                BackToMenu();
            }
        }
    }

    public void StartGame() {
        this.currentGameState = GameState.inGame;//se cambia el estado del juego a inGame
        MenuManager.sharedInstance.ChangeStateInGame(true);//que se visualice el menu principal
        MenuManager.sharedInstance.ChangeStateMainMenu(false);//que se visualice el menu principal
        MenuManager.sharedInstance.ChangeStateGameOver(false);//que se visualice el menu principal
    }

    public void GameOver() {
        sound2.Play();
        this.currentGameState = GameState.gameOver;//se cambia el estado del juego a inGame
        MenuManager.sharedInstance.ChangeStateGameOver(true);//que se visualice el menu de game over
        MenuManager.sharedInstance.ChangeStateMainMenu(false);//que se visualice el menu principal
        MenuManager.sharedInstance.ChangeStateInGame(false);//que se visualice el menu principal
    }

    public void RestartGame() {
        sound1.Play();
        MenuManager.sharedInstance.ChangeStateInGame(true);//que se visualice el menu principal
        MenuManager.sharedInstance.ChangeStateMainMenu(false);//que se visualice el menu principal
        MenuManager.sharedInstance.ChangeStateGameOver(false);//que se visualice el menu principal
        if (this.currentGameState == GameState.gameOver) {//para asegurar que solo entre acá cuando el estado es game over
            controller.RestartGame();//iniciamos la funcion de reiniciar el juego definida en PlayerController
        }
    }

    public void BackToMenu() {
        this.currentGameState = GameState.menu;//se cambia el estado del juego a inGame
        MenuManager.sharedInstance.ChangeStateMainMenu(true);//que se visualice el menu principal
        MenuManager.sharedInstance.ChangeStateGameOver(false);//que se visualice el menu principal
        MenuManager.sharedInstance.ChangeStateInGame(false);//que se visualice el menu principal
    }

    public void CollectObject(Collectable collectable){//se va a encargar de sumar al contador de collectables
        collectedObject += collectable.value; 
    }
}
