using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType { //los tipos de coleccionables disponibles
    healthPotion,
    manaPotion,
    money
}

public class Collectable : MonoBehaviour//clase que van a llevar relacionados todos los elementos recolectables del juego, como las monedas y las pociones
{
    public CollectableType type = CollectableType.money;//por defecto el tipo de coleccionable es money

    //obtenemos los componentes sprite y collider de los coleccionables para poderlos manipular
    private SpriteRenderer sprite;
    private CircleCollider2D itemCollider;

    public int value = 1;//aumentara de a 1 la cantidad por moneda/pocion que el personaje tome

    GameObject player;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<CircleCollider2D>();//los elementos a recolectar deben tener circle collider
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Hide() {//cuando se toma, se oculta
        sprite.enabled = false;
        itemCollider.enabled = false;
    }

    void Collect() {//funcion cuando se toma un collectable
        Hide();

        //las monedas las manejamos en el gameManager y la salud y mana en el playerController, ya que por logica de la vida real las monedas no son atributos tuyos, como si lo son la salud y el mana
        switch (this.type) {
            case CollectableType.money:
                GameManager.sharedInstance.CollectObject(this);//lamamos a la funcion definida en GameManager que recibe un parametro del tipo de esta glase (collectable) y lo suma al contador de monedas
                GetComponent<AudioSource>().Play();//que suene el sonido de moneda cuando la recolecto
                break;

            case CollectableType.healthPotion:
                //al PlayerController no ser singletone, debo acceder al gameObject del personaje y obtener su playerController y asi llamar su funcion de recolectar salud
                player.GetComponent<PlayerController>().CollectHealth(this.value);
                GetComponent<AudioSource>().Play();//que suene el sonido de pocion cuando la recolecto
                break;

            case CollectableType.manaPotion:
                //al PlayerController no ser singletone, debo acceder al gameObject del personaje y obtener su playerController y asi llamar su funcion de recolectar salud
                player.GetComponent<PlayerController>().CollectMana(this.value);
                GetComponent<AudioSource>().Play();//que suene el sonido de pocion cuando la recolecto
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)//el evento que se ejecuta al transpasar un trigger
    {
        if (collision.tag == "Player") {//si usamos el personaje para tocar la moneda, que esta se tome y sume al puntaje con la funcion Collect
            Collect();
        }    
    }
}
