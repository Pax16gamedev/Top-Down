using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplode : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float timeToExplode;
    [SerializeField] private LayerMask destroyLayer;
    void Start()
    {
        StartCoroutine(Explode(timeToExplode));
    }

    private IEnumerator Explode(float time)
    {
        yield return new WaitForSeconds((time/3)*2);
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds((time/3));

        foreach (Collider2D col in Physics2D.OverlapCircleAll(transform.position, radius, destroyLayer))
        {
            if(col.gameObject.TryGetComponent<NonPersistentDestroyable>(out var dest))
                dest.DestroyPermanent();
            Destroy(col.gameObject);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
