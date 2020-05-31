using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour//clase donde se programara la logica del enemigo
{
    public float runningSpeed = 6f;
    public int enemyDamage = -30;
    public bool facingRight = false;//por defecto el personaje no mira hacia la derecha

    Rigidbody2D rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float currentRunningSpeed = runningSpeed;

        if (facingRight)//mirando a la derecha
        {
            currentRunningSpeed = runningSpeed;
            this.transform.eulerAngles = new Vector3(0, 180, 0);//gira el enemigo hacia la otra direccion
        }else{//mirando a la izquierda
            currentRunningSpeed = -runningSpeed;
            this.transform.eulerAngles = Vector3.zero;//vuelve a la direccion original
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame || GameManager.sharedInstance.currentGameState == GameState.gameOver) {//solo si esta en inGame que se ejecute la accion de mover los enemigos
            rigidBody.velocity = new Vector2(currentRunningSpeed, rigidBody.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "coin") {
            return;
        }

        if (collision.name == "player") {//si choca con el personaje debe bajarle vida, pasandole un numero negativo al metodo CollectHealth del PlayerController
            collision.gameObject.GetComponent<PlayerController>().CollectHealth(enemyDamage);
            return;
        }

        //si no choca con esos dos elementos de arriba, va a chocar con el terreno y debe girar su sentido el enemigo
        facingRight = !facingRight;//lo contrario al facingRight que traia

    }
}
