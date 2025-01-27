using UnityEngine;

public interface IInteractable
{
    public Transform transform {  get; }

    public void Interact();
}
