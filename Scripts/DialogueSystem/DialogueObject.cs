using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Object")]

public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] private Response[] responses;
    [SerializeField] private Sprite characterIcon;
    public string[] Dialogue => dialogue;
    public Sprite CharacterIcon => characterIcon;

    public bool HasResponses => Responses != null && Responses.Length != 0;
    public Response[] Responses => responses;
}
