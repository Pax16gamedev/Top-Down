using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerCollisions playerCollisions;

    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerCollisions PlayerCollisions => playerCollisions;

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
    }
}
