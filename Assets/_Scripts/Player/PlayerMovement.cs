using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private PlayerAnimations playerAnimations;

    [SerializeField] float speed = 5;
    [SerializeField] float attackRadius = 0.75f;
    [SerializeField] float timeBetweenAttcks;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 destinationPoint;
    private bool isMoving;

    private bool canAttack = true;
    
    private Vector3 lastInput;

    private Collider2D forwardCollider; // Collider que tengo delante del Player

    void Awake()
    {
        player = GetComponent<Player>();
        playerAnimations = GetComponentInChildren<PlayerAnimations>();
    }

    private void Start()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        if(GameManagerSO.Instance.SceneLoaded)
        {
            transform.position = GameManagerSO.Instance.NewPosition;
            playerAnimations.SetMovement(GameManagerSO.Instance.NewOrientation.x, GameManagerSO.Instance.NewOrientation.y);
        }
    }

    private void Update()
    {
        if (verticalInput == 0)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
        }

        if (horizontalInput == 0)
        {
            verticalInput = Input.GetAxisRaw("Vertical");
        }
    }

    public void MovePlayer()
    {
        if (CheckIfCanMove())
        {
            CheckDestinationPoint();

            playerAnimations.SetMovement(horizontalInput, verticalInput);

            forwardCollider = player.PlayerCollisions.CheckForCollisions();

            if (!forwardCollider || forwardCollider.gameObject.CompareTag(Constants.TAGS.DOOR))
            {
                StartCoroutine(Move());
            }
        }
        else if (horizontalInput == 0 && verticalInput == 0)
        {
            playerAnimations.IsMoving(false);
        }
    }

    // Permito movimiento solo si estoy en una casilla y hay input
    bool CheckIfCanMove()
    {
        return !player.PlayerCollisions.IsInteracting && !isMoving && (horizontalInput != 0 || verticalInput != 0);
    }

    void CheckDestinationPoint()
    {
        lastInput = new Vector3(horizontalInput, verticalInput);
        destinationPoint = transform.position + lastInput;
        player.PlayerCollisions.InteractionPoint = destinationPoint;
    }

    IEnumerator Move()
    {
        isMoving = true;
        playerAnimations.IsMoving(true);

        while (transform.position != destinationPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinationPoint, speed * Time.deltaTime);
            yield return null;
        }
        destinationPoint = transform.position + lastInput;
        player.PlayerCollisions.InteractionPoint = destinationPoint;
        
        playerAnimations.IsMoving(false);
        isMoving = false;
    }

    public void Attack()
    {
        //if(canAttack)
        {
            canAttack = false;
            playerAnimations.AttackAnim();
            foreach(Collider2D coll in Physics2D.OverlapCircleAll(transform.position, attackRadius))
            {
                if(coll.gameObject.TryGetComponent(out Enemy enemy))
                    enemy.TakeDamage(1);
            }
            StartCoroutine(AttackCooldown(timeBetweenAttcks));
        }
        
    }

    private IEnumerator AttackCooldown(float attackCoolown)
    {
        yield return new WaitForSeconds(attackCoolown);
        canAttack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
