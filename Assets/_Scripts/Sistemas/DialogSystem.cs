using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    public static DialogSystem Instance;


    [Header("Config")]
    [SerializeField] GameObject marcoDialogo;
    [SerializeField] TextMeshProUGUI textoDialogoTMP;

    [Header("Dialogue")]
    [SerializeField, TextArea(1, 5)] string[] phrases;
    [SerializeField] float timeBetweenCharacters = 0.03f;

    private bool isTalking;
    private int currentIndex = -1;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowDialog(string[] phrases, float timeBetweenCharacters)
    {
        this.phrases = phrases;
        this.timeBetweenCharacters = timeBetweenCharacters;

        GameManagerSO.Instance.ChangePlayerState(false);
        marcoDialogo.SetActive(true);
        if(!isTalking)
        {
            NextPhrase();
        }
        else
        {
            CompletePhrase();
        }
    }

    IEnumerator WriteText()
    {
        isTalking = true;
        textoDialogoTMP.text = "";
        char[] phraseChars = phrases[currentIndex].ToCharArray();
        foreach(char c in phraseChars)
        {
            textoDialogoTMP.text += c;
            yield return new WaitForSeconds(timeBetweenCharacters);
        }
        isTalking = false;
    }

    private void NextPhrase()
    {
        currentIndex++;
        if(currentIndex >= phrases.Length)
        {
            FinishDialogue();
        }
        else
        {
            StartCoroutine(WriteText());
        }
    }

    private void CompletePhrase()
    {
        StopAllCoroutines();
        textoDialogoTMP.text = phrases[currentIndex];
        isTalking = false;
    }

    private void FinishDialogue()
    {
        isTalking = false;
        marcoDialogo.gameObject.SetActive(false);
        textoDialogoTMP.text = "";
        currentIndex = -1;
        GameManagerSO.Instance.ChangePlayerState(true);
    }
}
