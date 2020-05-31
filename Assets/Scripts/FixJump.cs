using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixJump : MonoBehaviour//este script trabaja sobre el salto original del script principal. Mientras mantengo presionados los botones de salto, el salto en el script principal se ejecuta normalmente, y al soltarlos le agrego gravedad al salto del script original y da la sensación de que salta más o salta menos dependiendo de que tanto dejo presionados los botones de salto gracias a este script
{
    public float fallMultiplier = 2f;//el plus de gravedad al caer (para que caiga más rápido)
    public float lowJumpMultiplier = 3f;//el plus de gravedad al subir (para que empiece a caer más rapido cuando estoy dejando de presionar sostenidamente el botón de salto)

    Rigidbody2D rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();//se inicializa el rigidBody del personaje
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidBody.velocity.y < 0)//si el personaje está cayendo...
        {
            rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;//esta operación al dar un número negativo (gravity.y = -9.81), se le agrega el factor de más gravedad; osea se le suma un valor negativo a la velocidad original en Y que lleva el personaje
        }
        else
        {//el personaje está subiendo
            if (rigidBody.velocity.y > 0 && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Space))//si el personaje está subiendo en el salto y dejé de presionar la tecla de saltar, se activa la gravedad mayor para que empiece a descender
            {
                rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;//esta operación al dar un número negativo (gravity.y = -9.81), se le agrega el factor de más gravedad; osea se le suma un valor negativo a la velocidad original en Y que lleva el personaje
            }
        }
    }
}
