using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageManager : MonoBehaviour
{
    /* Creamos un Singleton que se encargue de gestionar todos los eventos de daño que hay en el juego */
    public static DamageManager instance {get; private set;}

    /* Realizamos primero esto en el awake, para comprar si ya hay una instancia de este objeto, y si es así 
     destruimos el objeto actual */
    private void Awake() {
        if(instance != null && instance != this){
            Destroy(this.gameObject);
        }

        instance = this;
    }

    /*public void SubscribeToEvent(Action <int, HealthBarController> currentAction){
        currentAction += DamageCalculation;
    }*/



    /* Hacemos que las dos funciones se suscriban a la función Damage Calculation, para que cuando alguna de estas instancias 
     choquen con algo, se realice la resta de vida */
    public void SubscribeFunction(BulletController enemy){
        enemy.onCollision += DamageCalculation;
    }

    public void SubscribeFunction(EnemySimpleController enemy){
        enemy.onCollision += DamageCalculation;
    }

    /* Esta función recibe dos parámetros, el daño y una barra de vida, para luego restarle esta cantidad de vida 
      a la barra correspondiente */ 
    private void DamageCalculation(int damageTaken, HealthBarController healthBarController){
        healthBarController.UpdateHealth(-damageTaken);
    }
}
