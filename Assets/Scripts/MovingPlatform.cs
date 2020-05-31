using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour//Clase para activar la animacion de las plataformas moviles al pararme encima de ellas
{
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //hacemos que la transformacion de posicion del player dependa de la de la plataforma, para que el personaje no se caiga al moverse esta
        if (collision.gameObject == player) {
            player.transform.parent = transform;
        }
        
        //activamos la animacion de la plataforma
        Animator animator = GetComponent<Animator>();
        animator.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //hacemos que la transformacion de posicion del player dependa de la de la plataforma, para que el personaje no se caiga al moverse esta
        if (collision.gameObject == player)
        {
            player.transform.parent = null;
        }
    }
}
