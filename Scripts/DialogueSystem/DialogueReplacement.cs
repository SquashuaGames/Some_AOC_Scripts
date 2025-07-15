using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueReplacement : MonoBehaviour
{
    public DialogueObject[] dialogueObjects;
    public string wordToReplace;
    public void ReplaceDialogue(string wordReplacer)
    {
        foreach (DialogueObject dialogueObject in dialogueObjects)
        {
            int i = 0;
            foreach (string line in dialogueObject.Dialogue)
            {
                Debug.Log(line);
                line.Replace(wordToReplace, wordReplacer);
                dialogueObject.Dialogue[i] = line;
                i++;
                Debug.Log(line);
            }
        }
    }

}
