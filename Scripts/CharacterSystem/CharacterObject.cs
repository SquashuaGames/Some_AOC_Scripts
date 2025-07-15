using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Character Object")]
public class CharacterObject : ScriptableObject
{
    /*
    [SerializeField] public string CharName { get; private set; }
    [SerializeField] public CharacterStats Stats { get; private set; }
    */
    /*
       //This increases the guage of each team member but makes sure to stop increasing it if they are already ready.
       QTBGaugeOne += players[0].speed;
       QTBGaugeTwo += players[1].speed;
       QTBGaugeThree += players[2].speed;
       QTBGaugeFour += players[3].speed;

       if(!optionHolder.activeInHierarchy) 
       {     
           //This checks one by one if each character is ready to attack and then takes necessary steps to prepare.
           if (QTBGaugeOne >= 75) { QTBGaugeOne = 0; characterIcon.sprite = players[0].characterIcon; optionHolder.SetActive(true); menuIsOpen = true; onStandby[0] = false; }
           else if (QTBGaugeOne >= 75) { QTBGaugeTwo = 0; characterIcon.sprite = players[1].characterIcon; optionHolder.SetActive(true); menuIsOpen = true; onStandby[0] = false; }
           else if (QTBGaugeThree >= 75) { QTBGaugeThree = 0; characterIcon.sprite = players[2].characterIcon; optionHolder.SetActive(true); menuIsOpen = true; onStandby[0] = false; }
           else if (QTBGaugeFour >= 75) { QTBGaugeFour = 0; characterIcon.sprite = players[3].characterIcon; optionHolder.SetActive(true); menuIsOpen = true; onStandby[0] = false; }
       }       
       */
}
