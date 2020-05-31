using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour//clase que maneja la logica del player
{
    //variables del controlador del player
    public float jumpForce = 9f;//fuerza con la que va a saltar el player
    public float runningSpeed = 5f;//la velocidad (2 metros por segundo)

    Rigidbody2D rigidBody;//la referencia al objeto del player que permitira que se le asignen comportamientos de la fisica
    Animator animator;//la referencia a animator que definiremos en el metodo awake
    CapsuleCollider2D collider;//la referencia al colider del player

    //las instancias para los sonidos. Es un array porque agregamos al player mas de 1 sonido
    AudioSource[] sounds;
    AudioSource sound1;//jump
    AudioSource sound2;//pain

    float playerScale;//la escala (direccion) del player
    private float sizeYCollider;//va a guardar el valor original del tamaño en Y del collider
    Vector3 startPosition;//para guardar la posición inicial del personaje al iniciar el juego 

    public LayerMask groundMask;//esta variable va a contener la capa que va a estar programada como el suelo del juego, capa que va a tener cada elemento suelo como el piso, las plataformas y las rocas

    //las variables que creamos en la configuracion de las animaciones. Son "const" (constantes) porque nunca cambian, son fijas
    const string STATE_ALIVE = "isAlive";
    const string STATE_ON_THE_GROUND = "isOnTheGround";
    const string STATE_RUNNING = "isRunning";
    const string STATE_DOWN = "isDown";

    //variables para manejar la salud y el mana
    private int healthPoints;
    private float manaPoints;
    public const int INITIAL_HEALTH = 50, INITIAL_MANA = 50, MAX_HEALTH = 100, MAX_MANA = 100, MIN_HEALTH = 0, MIN_MANA = 0;//valores que no van a cambiar nunca, se escriben en mayuscula para diferenciarlas de las demas

    //variables para manejar la relacion entre el mana y el super salto/correr
    public const int SUPER_JUMP_COST = 20;//cuanto mana me cuesta cada que uso super salto
    public const float SUPER_JUMP_FORCE = 1.5f;//aumenta un 50% mas el poder del salto
    public const float SUPER_RUN_COST = 0.2f;//cuanto mana me cuesta cada que uso super run
    public const float SUPER_RUN_MULTIPLIER = 1.6f;//aumenta un 60% mas la velocidad de la animacion de correr

    void Awake()//se ejecuta antes del start. Es como poner la llave del auto antes de encenderlo
    {
        rigidBody = GetComponent<Rigidbody2D>();//para tener acceso al rigidBody
        animator = GetComponent<Animator>();//para tener acceso al animator
        collider = GetComponent<CapsuleCollider2D>();//para tener acceso al collider
        sounds = GetComponents<AudioSource>();//getComponents porque son varios
        sound1 = sounds[0];
        sound2 = sounds[1];
    }

    // Start is called before the first frame update
    void Start()//cuando se ejecuta el primer frame
    {
        //iniciamos con el personaje vivo y en el piso
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, true);
        animator.SetBool(STATE_RUNNING, false);
        animator.SetBool(STATE_DOWN, false);

        playerScale = transform.localScale.x; // el personaje siempre inicia mirando hacia la derecha
        sizeYCollider = collider.size.y;//va a guardar el valor original del tamaño en Y del collider
        startPosition = this.transform.position;//guarda la posición inicial del personaje al iniciar el juego

        //iniciamos con los valores por defecto de salud y mana
        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;
    }

    public void RestartGame() {//funcion que se ejecuta al reiniciar el juego cuando el personaje muere. Practicamente reinicia los valores del player
        //cuando muero, quiero que todos los bloques de nivel se destruyan y aparezcan de nuevo los iniciales
        LevelManager.sharedInstance.RemoveAllLevelBlocks();
        LevelManager.sharedInstance.GenerateInitialBlocks();

        this.animator.SetBool(STATE_ALIVE, true);//ponemos de nuevo la animacion de estar vivo

        this.transform.position = startPosition;//seteamos a la posicion inicial del jugador cuando empezamos a jugar
        this.rigidBody.velocity = Vector2.zero;//ponemos la velocidad en 0 para evitar que vuelva con la velocidad de caida que estaba teniendo al morir

        //reiniciamos la cantidad de monedas
        GameManager.sharedInstance.collectedObject = 0;

        //iniciamos con los valores por defecto de salud y mana
        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;

        Invoke("RestartPosition", 0.3f);//llama al metodo agregandole un delay, estoy para que las animaciones no se solapen
    }

    void RestartPosition() {//vuelve el personaje a su estado y posicion inicial
        //devolvemos la camara pero sin la animación
        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<CameraFollow>().ResetCameraPosition();

        GameManager.sharedInstance.StartGame();//y ponemos el estado de nuevo en inGame
    }

    // Update is called once per frame
    void Update()//lo que va sucediendo cada frame. Si son 60fps entonces esto sucede 60 veces cada segundo
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame || GameManager.sharedInstance.currentGameState == GameState.gameOver)//solo si estoy en el estado de en juego puede mi personaje moverse
        {
            Time.timeScale = 1f;//reanuda el juego despues del pause (que tan rapido se ejecuta el juego)

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))//Input.GetMouseButtonDown(0) para el mouse...
            {
                if (IsTouchingTheGround())//si está tocando el suelo puede saltar
                {
                    sound1.Play();//que suene el sonido de salto
                    rigidBody.velocity = Vector2.up * jumpForce;//la direccion y la fuerza
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))//si el salto es con la barra espaciadora es super salto, y me consume mana
            {
                if (IsTouchingTheGround() && manaPoints >= SUPER_JUMP_COST)//si está tocando el suelo y el mana disponible cubre el costo del salto (20 puntos) puede super saltar
                {
                    sound1.Play();//que suene el sonido de salto
                    manaPoints -= SUPER_JUMP_COST;//reducimos a 5 el mana
                    rigidBody.velocity = Vector2.up * (jumpForce * SUPER_JUMP_FORCE);//la direccion y la fuerza aumentada al factor superJumpForce
                }
            }

            //Debug.DrawRay(this.transform.position, Vector2.down * 1.5f, Color.red);//para dibujar el rayo y saber que longitud colocarle
        }
        else {
            Time.timeScale = 0f;//de lo contrario que se pare. Pone el juego en pausa total
        }
    }

    void FixedUpdate()//sucede siempre a ritmo, nunca experimenta caidas de frame pero pueden ser menos que 60fps
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame || GameManager.sharedInstance.currentGameState == GameState.gameOver)//solo si estoy en el estado de en juego puede mi personaje moverse
        {
            Time.timeScale = 1f;//reanuda el juego despues del pause (que tan rapido se ejecuta el juego)

            if (IsTouchingTheGround()){//si esta tocando el suelo
                //esto cambia la animacion de tal manera que revisa el estado (si toca el suelo o no), para cambiar el booleano de STATE_ON_THE_GROUND, que por ende cambiara la animacion segun la logica que se describio al configurar las animaciones
                animator.SetBool(STATE_ON_THE_GROUND, true);

                if (Input.GetKey(KeyCode.LeftShift) && manaPoints >= SUPER_RUN_COST)
                {//si mientras me estoy moviendo presiono la tecla de correr
                    manaPoints -= SUPER_RUN_COST;//reducimos a 5 el mana
                    runningSpeed = 9f;
                    animator.SetFloat("speedMultiplier", SUPER_RUN_MULTIPLIER);//cambio el valor del speedMultiplier definido en la animacion
                }
                else
                {
                    runningSpeed = 5f;
                    animator.SetFloat("speedMultiplier", 1.2f);
                }
            }
            else{////Debug.Log(rigidBody.velocity.y);
                animator.SetBool(STATE_ON_THE_GROUND, false);
            }

            //dejar la posibilidad de movernos por el eje x aun en el mismo salto
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))//si se presionan las teclas derecha o D
            {
                animator.SetBool(STATE_RUNNING, true);
                animator.SetBool(STATE_DOWN, false);
                collider.size = new Vector2(collider.size.x, sizeYCollider);//poner el collider en su estado normal
                rigidBody.velocity = new Vector2(runningSpeed, rigidBody.velocity.y);//se le asigna una velocidad al rigidBody con los parametros velocidad en x y velocidad en y con la funcion vector2
                transform.localScale = new Vector2(playerScale, transform.localScale.y);//cambia la direccion del sprite hacia la derecha

                if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))//si mientras me estoy moviendo presiono la tecla de agacharme
                {
                    animator.SetBool(STATE_RUNNING, false);
                    animator.SetBool(STATE_DOWN, true);
                    collider.size = new Vector2(collider.size.x, sizeYCollider - 1.0f);//reducir el collider
                    rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);//pone la velocidad del player en 0, osea lo deja quieto
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))//si se presionan las teclas de flecha izquierda o A
                {
                    animator.SetBool(STATE_RUNNING, true);
                    animator.SetBool(STATE_DOWN, false);
                    collider.size = new Vector2(collider.size.x, sizeYCollider);//poner el collider en su estado normal
                    rigidBody.velocity = new Vector2(-runningSpeed, rigidBody.velocity.y);//se le asigna una velocidad al rigidBody con los parametros velocidad en x y velocidad en y con la funcion vector2
                    transform.localScale = new Vector2(-playerScale, transform.localScale.y);//cambia la direccion del sprite hacia la izquierda

                    if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))//si mientras me estoy moviendo presiono la tecla de agacharme
                    {
                        animator.SetBool(STATE_RUNNING, false);
                        animator.SetBool(STATE_DOWN, true);
                        collider.size = new Vector2(collider.size.x, sizeYCollider - 1.0f);//reducir el collider
                        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);//pone la velocidad del player en 0, osea lo deja quieto
                    }
                }
                else
                {
                    if (rigidBody.velocity.x == 0)//aca verificamos que cuando toca el suelo y no hay velocidad sobre el eje x, se quede quieto
                    {
                        animator.SetBool(STATE_RUNNING, false);

                        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))//tengo que revisar si presiono flecha abajo para agachar el personaje
                        {
                            animator.SetBool(STATE_DOWN, true);
                            collider.size = new Vector2(collider.size.x, sizeYCollider - 1.0f);//reducir el collider
                        }
                        else
                        {
                            animator.SetBool(STATE_DOWN, false);
                            collider.size = new Vector2(collider.size.x, sizeYCollider);//poner el collider en su estado normal
                        }
                    }
                    else //tambien habilitar la logica de si me agacho y el personaje lleva el impulso en eje x sin estar presionando ya las teclas de moverme
                    {
                        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))//si mientras me estoy moviendo presiono la tecla de agacharme
                        {
                            animator.SetBool(STATE_RUNNING, false);
                            animator.SetBool(STATE_DOWN, true);
                            collider.size = new Vector2(collider.size.x, sizeYCollider - 1.0f);//reducir el collider
                            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);//pone la velocidad del player en 0, osea lo deja quieto
                        }
                    }
                }
            }
        }
        else{
            Time.timeScale = 0f;//de lo contrario que se pare. Pone el juego en pausa total
        }
    }

    bool IsTouchingTheGround() { //nos va a devolver si el personaje toca la capa de ground que tienen el suelo, rocas y plataformas
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 2.1f, groundMask))//origen, direccion, distancia, layer mask son los parametros de raycast. Usamos un rayo desde el personaje disparado hacia abajo que determine si está 1.8f cerca del suelo (esto se mide desde el centro del personaje(collider)) donde ese rayo toca los elementos del juego que tienen la mascara "Ground" que creamos inicialmente
        { 
            return true;
        }
        else {
            return false;
        }
    }

    public void Die() {//el evento que se ejecuta al tocar la killZone y se activa en la clase killZone
        //buscamos y comparamos la nueva puntuacion, y ver si la guardamos como la nueva puntuacion maxima en memoria
        int coins = GameManager.sharedInstance.collectedObject;
        float travelledDistance = GetTravelledDistance() + (coins * 3);
        float previousMaxDistance = PlayerPrefs.GetFloat("maxScore", 0f);//playerPrefs permite acceder y a editar datos que en el juego se guarden en memoria. El segundo parametro es el valor por defecto, que la primera vez sera 0

        if (travelledDistance > previousMaxDistance) {//si el score actual supera al almacenado, que lo actualice por el actual
            PlayerPrefs.SetFloat("maxScore", travelledDistance);
        }

        this.animator.SetBool(STATE_ALIVE, false);//seteamos con la nueva animacion de morir
        GameManager.sharedInstance.GameOver();//accedemos al manager e indicamos el metodo GameOver que setea el estado del juego
    }

    public void CollectHealth(int points) {//funcion que suma a los puntos del personaje
        //si la vida es negativa, es un golpe y reproduzco el sonido de dolor
        if (points < 0)
        {
            sound2.Play();
        }
            
        this.healthPoints += points;

        if (this.healthPoints >= MAX_HEALTH) {//debe limitarse al maximo de salud, para que si llega al tope, no pase de alli
            this.healthPoints = MAX_HEALTH;
        }

        if (this.healthPoints <= 0) {//si el personaje pierde toda su salud 
            Die();
        }
    }

    public void CollectMana(int points){//funcion que suma a los puntos del personaje
        this.manaPoints += points;

        if (this.manaPoints >= MAX_MANA){//debe limitarse al maximo de mana, para que si llega al tope, no pase de alli
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth() {//para obtener la salud en el player bar y actualizar la barra
        return healthPoints;
    }

    public float GetMana(){//para obtener el mana en el player bar y actualizar la barra
        return manaPoints;
    }

    public float GetTravelledDistance() {//calcula la distancia entre el punto de arranque del juego y el punto donde va el personaje moviendose, en X solamente
        return this.transform.position.x - startPosition.x;
    }
}
