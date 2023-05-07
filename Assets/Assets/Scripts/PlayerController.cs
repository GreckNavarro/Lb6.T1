using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRBD2;
    [SerializeField] private float velocityModifier = 5f;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private AnimatorController animatorController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BulletController bulletPrefab;
    [SerializeField] private CameraController cameraReference;

    /* Obtenemos referencia de la barra de vida, y la suscribimos al evento onHit, el cual se activa cuando recibes daño, y llama a la función encargada
      de realizar el movimiento de la pantalla correspondiente*/
    private void Start() {
        GetComponent<HealthBarController>().onHit += cameraReference.CallScreenShake;
    }

    private void Update() {
        /* Movimiento del jugador */
        Vector2 movementPlayer = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        myRBD2.velocity = movementPlayer * velocityModifier;

        animatorController.SetVelocity(velocityCharacter: myRBD2.velocity.magnitude);

        /* Obtenemos el vector 3 de la posición de nuestro mouse */
        Vector3 mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        CheckFlip(mouseInput.x);
    
        Vector3 distance = mouseInput - transform.position; 
        Debug.DrawRay(transform.position, distance * rayDistance, Color.red);
        /* Restamos la posición del mouse y posición del player, para obtener la posición real del puntero, aunque
         se realice movimiento de pantalla */

        if (Input.GetMouseButtonDown(0)){
            /* Creamos la bala cuando se haga click derecho*/
            BulletController myBullet =  Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            myBullet.SetUpVelocity(distance.normalized, gameObject.layer);
        }else if(Input.GetMouseButtonDown(1)){
            Debug.Log("Left Click");
        }
    }

    /* Verifica si mi personaje está mirando hacia derecha o izquierda para cambiar el flip del sprite */
    private void CheckFlip(float x_Position){
        spriteRenderer.flipX = (x_Position - transform.position.x) < 0;
    }
}
