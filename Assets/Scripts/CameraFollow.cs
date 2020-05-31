using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour//script de la camara, para que siga al personaje
{
    public Transform target;//el elemento que va a seguir, en este caso es el player
    public Vector3 offset = new Vector3(-5f, -2f, -10f);//la posicion de la camara respecto al player (target)
    public float dampingTime = 0.3f;//el delay que va a tener la camara de reaccionar para seguir el personaje
    public Vector3 velocity = Vector3.zero;

    void Awake()
    {
        Application.targetFrameRate = 60;//los frames a que queremos que vaya el juego
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera(true);
    }

    public void ResetCameraPosition() {//cuando la camara vuelve a la posicion inicial al morir el personaje 
        MoveCamera(false);
    }

    void MoveCamera(bool smooth) { //smooth en true es el efecto de suavisado activado... por lo cual en el juego(update) lo necesitamos. En false es un movimiento sin suavisado, y es el que necesitamos cuando queremos pasar de muerte al reinicio del juego
        Vector3 destination = new Vector3(target.position.x - offset.x, target.position.y - offset.y, offset.z);//es la posicion que debe ir adquiriendo la camara. En x es la posición del personaje restandole un pequeño valor para que el personaje no se vea en medio de la camara, sino un poco por detrás y tener más visión hacía adelante

        if (smooth)//si el personaje está vivo... 
        {
            //los parametros son: 1 posición actual de la camara, 2 el destino a donde me quiero mover, 3 un vector3 pasado por referencia a otro script de unity que me va a calcular la velocidad 4 el delay
            this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref velocity, dampingTime);//debemos implementar la transformacion de la camara con SmoothDamp, para que sea con el efecto suavisado de cine
        }
        else { //si el personaje no está alive
            this.transform.position = destination;//la transformacion sin el efecto, simplemente.
        }
    }
}
