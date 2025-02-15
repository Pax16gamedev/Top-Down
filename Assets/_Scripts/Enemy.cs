using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float hp;

    public void TakeDamage(float damage)
    {
        StartCoroutine(MakeRed());
        hp--;

        if(hp<= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator MakeRed()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
