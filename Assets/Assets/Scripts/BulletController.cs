using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRGB2D;
    [SerializeField] private float velocityMultiplier;
    [SerializeField] private int damage;
    public event Action<int, HealthBarController> onCollision;

    /*En esta parte del c�digo, creamos una funci�n que haga luego se llamar� en el player controller
     desde el cu�l configuraremos tanto la velocidad de la bala, como el layer con el que saldr� la bala*/
    
    public void SetUpVelocity(Vector2 velocity, int newTag){
        myRGB2D.velocity = velocity * velocityMultiplier;
        gameObject.layer = newTag;

        DamageManager.instance.SubscribeFunction(this);
    }

    //Destruye la bala cuando esta sale de la c�mara
    private void OnBecameInvisible() {
        Destroy(this.gameObject);
    }



    private void OnTriggerEnter2D(Collider2D other) {
        /* Lo que hace este if es comparar los tags de los objetos que colisionan, para que en todo caso, un player 
          dispare, este no pueda ser herido por su propia bala, o en todo caso, las balas enemigas no puedan da�ar
          a los otros enemigos */
        if(!other.CompareTag(gameObject.tag) && (other.CompareTag("Player") || other.CompareTag("Enemy"))){
            /* Obtenemos el componente de su barra para mandarle el da�o y la modificaci�n de esta misma*/
            if (other.GetComponent<HealthBarController>()){
                onCollision?.Invoke(damage,other.GetComponent<HealthBarController>());
            }
            Destroy(this.gameObject);
        }
    }
}
