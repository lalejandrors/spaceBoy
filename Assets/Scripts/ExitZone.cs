using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour//clase para gestionar la destruccion del bloque que quede atras del player
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)//se destruye el bloque anterior y se da paso al nuevo
    {
        if (collision.tag == "Player") {//si el jugador cruza esta zona
            LevelManager.sharedInstance.AddLevelBlock();//agregar un nuevo bloque
            LevelManager.sharedInstance.RemoveLevelBlock();//eliminar el bloque que dejo atras
        }
    }
}
