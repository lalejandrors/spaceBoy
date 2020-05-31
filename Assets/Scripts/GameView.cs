using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour//clase que se relaciona al canvas del inGame, para controlar las puntuaciones del personaje
{
    public Text coinsText, scoreText, maxScoreText;//los textos de las puntuaciones

    private PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame) {//si estoy en el juego, que se vayan actualizando las puntuaciones
            int coins = GameManager.sharedInstance.collectedObject;//obtenemos el contador que se actualiza en el GameManager
            float score = controller.GetTravelledDistance();//en la funcion del controlador del personaje llamada getTravelledDistance, podemos obtener la distancia del personaje que recorrio en x
            float maxScore = PlayerPrefs.GetFloat("maxScore", 0);//accedemos a la memoria al dato maxScore y obtenemos su valor

            //asignamos los valores a los textos de las puntuaciones
            coinsText.text = coins.ToString();
            scoreText.text = score.ToString("f1");//el valor dentro del toString, me indica que si estoy transformando un float, que solo deje un solo decimal
            maxScoreText.text = maxScore.ToString("f1");
        }
    }
}
