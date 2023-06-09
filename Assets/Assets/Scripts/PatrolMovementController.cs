using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovementController : MonoBehaviour
{
    [SerializeField] private Transform[] checkpointsPatrol;
    [SerializeField] private Rigidbody2D myRBD2;
    [SerializeField] private AnimatorController animatorController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float velocityModifier = 5f;
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private LayerMask layerInteraction;
    private Transform currentPositionTarget;
    private int patrolPos = 0;
    private float fastVelocity = 0f;
    private float normalVelocity;


    /* Al iniciar, colocamos la posici�n a la que se debe ir el enemigo, como el primer punto dentro del arreglo de puntos del mapa
     y colocamos valores a normalvelocity y fastvelocity */
    private void Start()
    {
        currentPositionTarget = checkpointsPatrol[patrolPos];
        transform.position = currentPositionTarget.position;

        normalVelocity = velocityModifier;
        fastVelocity = velocityModifier * 2.5f;
    }

    private void Update()
    {
        CheckNewPoint();
        /* Cambiamos la velocidad de la variable del animator, para que el personaje realice la animaci�n de caminar */
        animatorController.SetVelocity(velocityCharacter: myRBD2.velocity.magnitude);
    }

    /* La funci�n de CheckNewPoint se encarga de que el enemigo recorra todos los puntos que tenemos dentro del arreglo*/

    private void CheckNewPoint()
    {
        if (Mathf.Abs((transform.position - currentPositionTarget.position).magnitude) < 0.25)
        {
            /* Cuando la posici�n del enemigo, restada con el punto al que debe ir, es menor a 0.25, hacemos que este se dirija hacia el
            siguiente punto que se encuentra dentro del array*/
            transform.position = currentPositionTarget.position;
            patrolPos = patrolPos + 1 == checkpointsPatrol.Length ? 0 : patrolPos + 1;
            currentPositionTarget = checkpointsPatrol[patrolPos];
            CheckFlip(myRBD2.velocity.x);
        }

        /* Creamos un vector2 encargado de calcular la distancia entre el punto objetivo y nuestro transform
        para luego dibujar un raycast en esa direcci�n */

        Vector2 distanceTarget = currentPositionTarget.position - transform.position;
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, distanceTarget, raycastDistance, layerInteraction);
        if (hit2D)
        {
            /* Cuando el raycast choca con el player, la velocidad del enemigo aumenta y cuando no, se mantiene normal*/
            if (hit2D.collider.CompareTag("Player"))
            {
                velocityModifier = fastVelocity;
            }
        }
        else
        {
            velocityModifier = normalVelocity;
        }
        /* Le colocamos velocidad al enemigo y dibujamos el raycast*/
        myRBD2.velocity = (currentPositionTarget.position - transform.position).normalized * velocityModifier;
        Debug.DrawRay(transform.position, distanceTarget * raycastDistance, Color.cyan);

    }
    /* Funci�n encargada del flip del sprite */
    private void CheckFlip(float x_Position)
    {
        spriteRenderer.flipX = (x_Position - transform.position.x) < 0;
    }
}
