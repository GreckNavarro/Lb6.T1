using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuiManager : MonoBehaviour
{
    /* Creamos un SingleTon que se encargue de todo el puntaje del juego */
    public static GuiManager instance {get; private set;}
    [SerializeField] private TMP_Text scoreText;
    private int scoreTotal = 0;


    private void Awake() {
        /* Realizamos primero esto en el awake, para comprar si ya hay una instancia de este objeto, y si es así 
        destruimos el objeto actual */
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        }

        instance = this;
    }

   /* Función que se encarga de aumentar el puntaje actual, cuando matamos a un enemigo, recibiendo como parámetro los puntos
     ganados */
    public void UpdateText(int pointsGained){
        scoreTotal += pointsGained;
        scoreText.text = string.Format("Score: {0} (+ {1})", scoreTotal, pointsGained);
    }
}
