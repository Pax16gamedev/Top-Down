using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Vector2 nextPosition;
    [SerializeField] Vector2 nextOrientation;
    [SerializeField] int nextSceneIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            GameManagerSO.Instance.LoadNewScene(nextPosition, nextOrientation, nextSceneIndex);
        }
    }
}
