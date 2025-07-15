using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleDialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;
    public bool isOpen { get; private set; }

    private TypewriterEffect typewriterEffect;
    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
    }

    public void ShowDialogue(string dialogue)
    {       
        StartCoroutine(StepThroughDialogue(dialogue));
    }

    private IEnumerator StepThroughDialogue(string dialogue)
    {
        yield return RunTypingEffect(dialogue);

        textLabel.text = dialogue;
          
        yield return null;
        yield return new WaitForSeconds(1.5f);
        textLabel.text = "";
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {       
        typewriterEffect.Run(dialogue, textLabel);

        while (typewriterEffect.isRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                typewriterEffect.Stop();
            }

        }

    }

}
