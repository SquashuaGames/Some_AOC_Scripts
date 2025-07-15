using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Party")]
public class Party : ScriptableObject
{
    public CharacterStats[] party = new CharacterStats[4];
    public CharacterStats blankRound;
    public CharacterStats GetPlayer(int i)
    {
        if (i < party.Length) return party[i];
        else return blankRound;
    }

    public void ReplacePlayer(CharacterStats character, int i)
    {
        if (character.isPlayer)
        {
            party[i] = character;
        }
    }
}
