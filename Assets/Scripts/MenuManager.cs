using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour//clase que gestiona lo que pasa en el menu del juego
{
    //debe estar un canvas asociado
    public Canvas MainMenuCanvas;
    public Canvas GameOverCanvas;
    public Canvas InGameCanvas;

    //SINGLETON///////////////////////////////////////////////
    public static MenuManager sharedInstance;//define la palabra singleton, osea una unica instancia de esta clase en todo el juego que nunca cambia (static) y se define en el awake del mismo. Por medio de esta instancia puedo acceder a los metodos y variables de esta clase desde cualquier otro script

    void Awake()//se ejecuta antes del start, osea antes del primer frame
    {
        if (sharedInstance == null)
        {//para asegurarnos que no se vaya a definir esta instancia de la clase más de una vez
            sharedInstance = this;//la referencia a la clase actual
        }
    }
    ///END SINGLETON///////////////////////////////////////////////////////
    ///

    public void ChangeStateMainMenu(bool state) {//cambia desde el gameManager si se muestra o no el mainMenu
        MainMenuCanvas.enabled = state;
    }

    public void ChangeStateGameOver(bool state)//cambia desde el gameManager si se muestra o no el gameOver
    {
        GameOverCanvas.enabled = state;
    }

    public void ChangeStateInGame(bool state)//cambia desde el gameManager si se muestra o no el gameOver
    {
        InGameCanvas.enabled = state;
    }

    public void ExitGame() {//lo que ocurre al darle exit al juego... si estamos en el editor de unity se cierra el game mode... en otro caso que se cierre la aplicación en la plataforma en la que el juego se lance
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // Start is called before the first frame update
    void Start()
    {
        MainMenuCanvas.enabled = true;//indicamos que en el inicio este canvas está inactivo
        GameOverCanvas.enabled = false;//indicamos que en el inicio este canvas está inactivo
        InGameCanvas.enabled = false;//indicamos que en el inicio este canvas está inactivo
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
