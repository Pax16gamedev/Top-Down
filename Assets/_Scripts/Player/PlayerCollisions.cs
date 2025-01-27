using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private Player player;

    private Collider2D forwardCollider; // Collider que tengo delante del Player

    [SerializeField] float interacionRadius = .3f;
    private Vector3 interactionPoint;

    private bool isInteracting;

    public Vector3 InteractionPoint { get => interactionPoint; set => interactionPoint = value; }
    public bool IsInteracting { get => isInteracting; set => isInteracting = value; }

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void CheckForInteractions()
    {
        forwardCollider = CheckForCollisions();
        if (!forwardCollider) return;

        if(forwardCollider.TryGetComponent(out IInteractable interactable))
        {
            interactable.Interact();
        }
    }

    public Collider2D CheckForCollisions()
    {
        return Physics2D.OverlapCircle(interactionPoint, interacionRadius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(interactionPoint, interacionRadius);
    }
}
