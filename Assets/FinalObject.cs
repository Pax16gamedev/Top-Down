using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalObject : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            spriteRenderer.enabled = false;
            //Poner victoria
        }

        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        //Cerrar ventana
    }
}
