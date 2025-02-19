using UnityEngine;

public class EnemyAI : Enemy
{
    private SpriteRenderer spriteRenderer;

    [Header("Patrulla")]
    public Transform[] patrollPoints;            
    public float patrollSpeed = 2f;            
    private int currentIndex = 0;            

    [Header("Detección")]
    public float detectionDistance = 5f;           
    public LayerMask playerMask;                 

    [Header("Persecución")]
    public float chaseSpeed = 4f;         
    private Transform player;                    

    [Header("Ataque")]
    public float attackDistance = 1f;            
    public float timeBetweenAttacks = 2f;         
    private float attackTimer;
    

    private void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackTimer = timeBetweenAttacks;
    }

    private void Update()
    {
        if(player.position.x < transform.position.x) { spriteRenderer.flipX = true; } else { spriteRenderer.flipX = false; }
        Vector2 direccionJugador = (player.position - transform.position).normalized;

        // Lanza un raycast para detectar al jugador
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direccionJugador, detectionDistance, playerMask);


        Debug.DrawRay(transform.position, direccionJugador * detectionDistance, Color.red);


        if(hit.collider != null && hit.collider.CompareTag("Player"))
        {
            float distanciaActual = Vector2.Distance(transform.position, player.position);

            if(distanciaActual <= attackDistance)
            {
                AtacarJugador();
            }
            else
            {
                PerseguirJugador();
                attackTimer = timeBetweenAttacks;
            }
        }
        else
        {
            Patrullar();
        }
    }

    void Patrullar()
    {
        if(patrollPoints.Length == 0)
            return;

        Transform pointToGo = patrollPoints[currentIndex];
        transform.position = Vector2.MoveTowards(transform.position, pointToGo.position, patrollSpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, pointToGo.position) < 0.1f)
        {
            currentIndex = (currentIndex + 1) % patrollPoints.Length;
        }
    }

    void PerseguirJugador()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    void AtacarJugador()
    {
        attackTimer -= Time.deltaTime;
        if(attackTimer <= 0f)
        {
            Debug.Log("¡Atacando al jugador!");

            attackTimer = timeBetweenAttacks;
        }
    }
}

