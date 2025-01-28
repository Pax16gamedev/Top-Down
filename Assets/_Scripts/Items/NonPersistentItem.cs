using UnityEngine;

public class NonPersistentItem : Item
{
    // Posible solucion: Guardar los items en un excel/csv con los uniqueIds
    // Otra forma, usar el InstanceId del objeto
    [SerializeField] private int uniqueId;

    public int UniqueId => uniqueId;

    private void Awake()
    {
        //uniqueId = gameObject.GetInstanceID();
    }

    protected void Start()
    {
        // Si estoy en la lista es que ya he spawneado el objeto y no tengo que agregarlo a la lista (respawnear)
        if(GameManagerSO.Instance.NonPersistentItems.ContainsKey(uniqueId))
        {
            // Item ya recogido, lo destruyo
            if(!GameManagerSO.Instance.NonPersistentItems[uniqueId])
            {
                Destroy(gameObject);
            }
        }
        else // Primer spawn
        {
            GameManagerSO.Instance.NonPersistentItems.Add(uniqueId, true);
        }
    }

    public override void Interact()
    {
        GameManagerSO.Instance.NonPersistentItems[uniqueId] = false; // No vuelve a respawnear
        base.Interact();
    }
}
