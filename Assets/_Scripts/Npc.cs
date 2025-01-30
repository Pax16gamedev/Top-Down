using System.Collections;
using TMPro;
using UnityEngine;

public class Npc : MonoBehaviour, IInteractable
{

    [Header("Dialogue")]
    [SerializeField, TextArea(1, 5)] string[] phrases;
    [SerializeField] float timeBetweenCharacters = 0.03f;

    private bool isTalking;
    private int currentIndex = -1;

    public void Interact()
    {
        DialogSystem.Instance.ShowDialog(phrases, timeBetweenCharacters);
    }
}
