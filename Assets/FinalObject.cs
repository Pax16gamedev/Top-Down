using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalObject : MonoBehaviour, IInteractable
{
    SpriteRenderer spriteRenderer;
    [SerializeField] GameObject texto;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Menu");
    }

    public void Interact()
    {
        
        {
            spriteRenderer.enabled = false;
            texto.SetActive(true);
        }

        StartCoroutine(Wait());
    }
}
