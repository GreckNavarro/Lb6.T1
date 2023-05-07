using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera myVC;
    private CinemachineBasicMultiChannelPerlin noise;

    /* Obtenemos referencia de un componente del cinemachine, y la guardamos en un atributo privado */
    private void Start() {
        noise = myVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        //StartCoroutine(ShakeCamera(3,5f));
    }

    /* Función encargada de iniciar la corrutina del movimiento de la cámara al recibir daño */
    public void CallScreenShake(){
        StartCoroutine(ShakeCamera(5,0.5f));
    }

    /* IEnumerator encargado de realizar un pequeño movimiento de la cámara virtual,modificando valores que serán la intensidad con la que 
     se mueva y la durabilidad de esta */
    IEnumerator ShakeCamera(float intensity, float time){
        noise.m_AmplitudeGain = intensity;
        float totalTime = time;
        float initIntensity = intensity;
        while(totalTime > 0){
            totalTime -= Time.deltaTime;
            noise.m_AmplitudeGain = Mathf.Lerp(initIntensity,0f, 1-(totalTime/time));
            yield return null;
        }
    }
}
