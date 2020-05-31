using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BarType{ //enum con los dos tipo de barras
    healthBar,
    manaBar
}

public class PlayerBar : MonoBehaviour//clase que gestiona las barras de vida y mana del canvas de inGame
{
    private Slider slider;
    public BarType type;//para que en el editor se le pueda establecer a cada barra su tipo
    
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        switch (type) {//inicializamos el atributo de maxValue con los maximos de salud y mana permitidos
            case BarType.healthBar:
                slider.maxValue = PlayerController.MAX_HEALTH;
                break;
            case BarType.manaBar:
                slider.maxValue = PlayerController.MAX_MANA;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        slider = GetComponent<Slider>();
        switch (type)//segun el tipo de barra, se va actualizando el valor trayendo en todo momento la salud y el mana de las funciones para ello del playerController
        {
            case BarType.healthBar:
                slider.value = GameObject.Find("player").GetComponent<PlayerController>().GetHealth();
                break;
            case BarType.manaBar:
                slider.value = GameObject.Find("player").GetComponent<PlayerController>().GetMana();
                break;
        }
    }
}
