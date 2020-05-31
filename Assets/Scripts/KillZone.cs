using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour//clase que maneja la logica de la killZone
{
    private PlayerController controller;//la referencia a PlayerController para conectar con esa clase

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)//funcion que se ejecuta cuando el personaje entra en colision con la zona de muerte, en este caso un game object que contiene un collider gobernado por un trigger
    {
        if (collision.tag == "Player") { //si el elemento que colisiona con la zona de muerte tiene un tag llamado "Player"
            controller = collision.GetComponent<PlayerController>();//del elemento que colisiona con la zona de muerte, obtenemos su player controler relacionado (el jugador tiene el script de PlayerController) para acceder al metodo allí descrito "die" para morir
            controller.Die();
        }
    }
}
