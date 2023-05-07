using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizzardController : MonoBehaviour
{

    [SerializeField] Transform[] patrolPoints;
    [SerializeField] private Rigidbody2D myRBD2;
    [SerializeField] private float velocity = 3f;
    [SerializeField] private float aumentospeed = 2f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BulletController bullet;
    [SerializeField] GameObject Controlador;

    private Transform currentTarget;
    private int Patrolposition = 0;
    private Transform currentPositionTarget;
    private bool isPatrolling = true;


    private bool isFollowing;
    private bool isMoving;
    private bool canShoot = true;



    /* Al iniciar, colocamos la posición a la que se debe ir el enemigo, como el primer punto dentro del arreglo de puntos del mapa
     y colocamos valores a normalvelocity y fastvelocity */
    private void Start()
    {
        currentPositionTarget = patrolPoints[Patrolposition];
        transform.position = currentPositionTarget.position;

    }

    private void Update()
    {
        /* Si isPatrolling es verdadero, se realiza la función Patrol, sino, es porque el enemigo está persiguiendo al personaje,
         y empieza a dispararle */ 
        if (isPatrolling)
        {
            Patrol();
        }
        else
        {
            if (isMoving)
            {
                myRBD2.velocity = (currentTarget.position - transform.position).normalized * velocity * aumentospeed;

                if (isFollowing && canShoot)
                {
                    StartCoroutine(ShootBullet());
                    canShoot = false;
                }

                CalculateDistance();
            }
            else
            {
                myRBD2.velocity = (currentTarget.position - transform.position).normalized * velocity;
                CalculateDistance();

            }
        }



    }

    /* De lo que se encarga esta posición es de detener al ogro cuando llega a su objetivo */

    private void CalculateDistance()
    {
        if ((currentTarget.position - transform.position).magnitude < 0.05f)
        {
            transform.position = currentTarget.position;
            isMoving = false;
            myRBD2.velocity = Vector2.zero;
        }
        else
        {
            isMoving = true;
        }
    }

    /* Este método es el encargado del comportamiento de disparo del enemigo, aquí instanciamos la bala, y le damos una velocidad y layer
     para luego esperar un segundo y volverla a repetir */
    IEnumerator ShootBullet()
    {
        Instantiate(bullet, transform.position, Quaternion.identity).SetUpVelocity(myRBD2.velocity, gameObject.layer);
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }
   
    /* La función de Patrol se encarga de que el enemigo recorra todos los puntos que tenemos dentro del arreglo*/
    private void Patrol()
    {
        /* Cuando la posición del enemigo, restada con el punto al que debe ir, es menor a 0.25, hacemos que este se dirija hacia el
         siguiente punto que se encuentra dentro del array*/
        if (Mathf.Abs((transform.position - currentPositionTarget.position).magnitude) < 0.25)
        {
            transform.position = currentPositionTarget.position;
            Patrolposition = Patrolposition + 1 == patrolPoints.Length ? 0 : Patrolposition + 1;
            currentPositionTarget = patrolPoints[Patrolposition];
            CheckFlip(myRBD2.velocity.x);
            
        }

        myRBD2.velocity = (currentPositionTarget.position - transform.position).normalized * velocity;
    }
    
    /* Función encargada del flip del sprite */
    private void CheckFlip(float x_Position)
    {
        spriteRenderer.flipX = (x_Position - transform.position.x) < 0;
    }


    /* Cuando el Player entra en la zona del enemigo, currentTarget se vuelve la posición del player y following es true*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            currentTarget = other.transform;
            isFollowing = true;
            isPatrolling = false;
        }
    }


    /*Cuando el player sale de la zona del enemigo, current posición es la posición inicial del controlador y following es false;*/

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            currentTarget = transform;
            isFollowing = false;
            isPatrolling = true;
        }
    }

    // Cuando se destruye el ogro, se destruye su controlador para que no surja un error
    private void OnDestroy()
    {
        Destroy(Controlador);
    }
}


