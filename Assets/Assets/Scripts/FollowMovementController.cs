using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMovementController : MonoBehaviour
{
    [SerializeField] private Transform ogreTransform;
    [SerializeField] private Rigidbody2D ogreRB2D;
    [SerializeField] private float velocityModifier;
    [SerializeField] private BulletController bullet;
    

    private Transform currentTarget;
    private bool isFollowing;
    private bool isMoving;
    private bool canShoot = true;

    /* Hacemos que currentTarget sea la posición central de nuestro controlador al iniciar */
    private void Start() {
        currentTarget = transform;
    }

    private void Update() {
        /* Si ismoving es true, le añadimos al Rb del ogro una velocidad, restando la posición del player y del enemigo, para que el ogro se dirija hacia él*/
        if (isMoving){
            ogreRB2D.velocity = (currentTarget.position - ogreTransform.position).normalized * velocityModifier;

            /* Cuando lo está siguien y disparar es true, empieza la corrutina de disparar */
            if (isFollowing && canShoot){
                StartCoroutine(ShootBullet());
                canShoot = false;
            }
            /* Calcula la distancia para saber si llegó a su objetivo o no */
            CalculateDistance();
        }else{
            /* Modificamos la velocidad del ogro, para que vuelva a su posición inicial */
            ogreRB2D.velocity = (currentTarget.position - ogreTransform.position).normalized * velocityModifier;
            /* Calcula la distancia para saber si llegó a su objetivo o no */
            CalculateDistance();
        }
    }

    /* De lo que se encarga esta posición es de detener al ogro cuando llega a su objetivo */
    private void CalculateDistance(){
        if((currentTarget.position - ogreTransform.position).magnitude < 0.05f){
            ogreTransform.position = currentTarget.position;
            isMoving = false;
            ogreRB2D.velocity = Vector2.zero;
        }else{
            isMoving = true;
        }
    }


    /* Este método es el encargado del comportamiento de disparo del enemigo, aquí instanciamos la bala, y le damos una velocidad y layer
      para luego esperar un segundo y volverla a repetir */
    IEnumerator ShootBullet(){
        Instantiate(bullet, ogreTransform.position, Quaternion.identity).SetUpVelocity(ogreRB2D.velocity, gameObject.layer);
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }

    /* Cuando el Player entra en la zona del enemigo, currentTarget se vuelve la posición del player y following es true*/
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            currentTarget = other.transform;
            isFollowing = true;
        }
    }

    /*Cuando el player sale de la zona del enemigo, current posición es la posición inicial del controlador y following es false;*/
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            currentTarget = transform;
            isFollowing = false;
        }
    }
}
