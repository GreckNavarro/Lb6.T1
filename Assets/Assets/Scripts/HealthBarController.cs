using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private int maxValue;
    [Header("Health Bar Visual Components")] 
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private RectTransform modifiedBar;
    [SerializeField] private float changeSpeed;

    private int currentValue;
    private float _fullWidth;
    private float TargetWidth => currentValue * _fullWidth / maxValue;
    private Coroutine updateHealthBarCoroutine;


    /* Creamos dos Acciones */
    public event Action onHit;
    public event Action onDeath;


    private void Start() {
        currentValue = maxValue;
        _fullWidth = healthBar.rect.width;
    }

    /// <summary>
    /// Metodo <c>UpdateHealth</c> actualiza la vida del personaje de manera visual. Recibe una cantidad de vida modificada.
    /// </summary>
    /// <param name="amount">El valor de vida modificada.</param>
    /// 


    /* Funci�n encargada de actualizar la barra visual del personaje*/
    public void UpdateHealth(int amount){
        currentValue = Mathf.Clamp(currentValue + amount, 0, maxValue);
        onHit?.Invoke();

        if(updateHealthBarCoroutine != null){
            StopCoroutine(updateHealthBarCoroutine);
        }
        updateHealthBarCoroutine = StartCoroutine(AdjustWidthBar(amount));

        if(currentValue == 0){
            onDeath?.Invoke();
        }
    }

    /* Corrutina encargada de saber cu�l es la barra que se modificar�, la segunda se encarga de animar esta, y ajusta 
     el ancho de ambas para que se pueda ver bien */
    IEnumerator AdjustWidthBar(int amount){
        RectTransform targetBar = amount >= 0 ? modifiedBar : healthBar;
        RectTransform animatedBar = amount >= 0 ? healthBar : modifiedBar;

        targetBar.sizeDelta = SetWidth(targetBar,TargetWidth);

        while(Mathf.Abs(targetBar.rect.width - animatedBar.rect.width) > 1f){
            animatedBar.sizeDelta = SetWidth(animatedBar,Mathf.Lerp(animatedBar.rect.width, TargetWidth, Time.deltaTime * changeSpeed));
            yield return null;
        }

        animatedBar.sizeDelta = SetWidth(animatedBar,TargetWidth);
    }

    /* Funci�n que se utiliza en la corrutina Anterior, para poder ajustar el ancho de una barra de vida del personaje o enemigo
     * en pantalla */
    private Vector2 SetWidth(RectTransform t, float width){
        return new Vector2(width, t.rect.height);
    }
}
