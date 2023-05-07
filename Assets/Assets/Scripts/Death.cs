using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] private GameObject ogroController;

    // Cuando se destruye el ogro, se destruye su controlador para que no surja un error
    private void OnDestroy()
    {
        Destroy(ogroController);
    }
}
