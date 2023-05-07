using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySimpleController : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int score = 50;
    [SerializeField] private GameObject go;
    /* Creamos un Action que recibe de parámetros un int y una barra controller*/
    public event Action<int, HealthBarController> onCollision;


    private void Start() {
        /* Lo que hacemos es suscribir la función actual al DamageManager*/
        DamageManager.instance.SubscribeFunction(this);
        /* Obtenemos la referencia de nuestra barra de vida y la suscribimos al evento OnDeath, que se activa cuando nuestra barra llega a cero*/
        GetComponent<HealthBarController>().onDeath += OnDeath;
    }

    /* Cuando el enemigo muere, realiza la animación de morir, nuestro puntaje aumenta y el gameobject enemigo se destruye después de 1 segundo.*/
    private void OnDeath(){
        go.GetComponent<AnimatorController>().SetDie();
        GuiManager.instance.UpdateText(score);
        Destroy(go, 1f);
    }

    /* Cuando el enemigo choca con el jugador, obtenemos referencia de la barra de vida de este, y invocamos su evento de recibir daño, y le mandamos la cantidad.*/
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            if(other.GetComponent<HealthBarController>()){
                onCollision?.Invoke(damage,other.GetComponent<HealthBarController>());
            }
        }
    }
}
