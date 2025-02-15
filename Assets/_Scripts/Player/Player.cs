using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerCollisions playerCollisions;

    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerCollisions PlayerCollisions => playerCollisions;

    [SerializeField]
    private GameObject bomb;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCollisions = GetComponent<PlayerCollisions>();
    }

    private void Update()
    {
        GetInputs();
    }

    void GetInputs()
    {
        playerMovement.MovePlayer();

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerCollisions.CheckForInteractions();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            UseItem();
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerMovement.Attack();
        }
    }

    private void UseItem()
    {
        Instantiate(bomb, transform.position + Vector3.forward*2, Quaternion.identity);
    }
}
