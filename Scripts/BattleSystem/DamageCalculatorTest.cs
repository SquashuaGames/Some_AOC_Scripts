using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculatorTest : MonoBehaviour
{
    public CombatManager combatManager;
    public CharacterStats user;
    public CharacterStats target;
    public Move move;
    public BattleDialogue battleDialogue;
    public GameObject holder;   
    
    public int GetDamage(CharacterStats user, CharacterStats target, Move move, out bool miss, out bool isCrit, out bool isSuperCrit)
    {
        int toHit;
        float X;
        int levelDiff = 0;

        if (user.isPlayer && (user.level - target.level) < 0) levelDiff = (user.level - target.level) * 5;

        toHit = Random.Range(1, 100) + user.accuracy - target.evasion + levelDiff + move.hitAdjustment;
        int critStrike = RollForCrit(user.critRate, user.critMultiplier, out isCrit, out isSuperCrit);
        
        if (toHit > 0 && move.needsAccuracy)
        {
            miss = false;
            X = ((float)critStrike * (float)move.power * (float)user.strength * 13.3f * ((float)user.skill / (float)user.Ability) * ((float)user.intellect / (float)user.level) * (float)user.level);
            X -= X * (target.physicalResistance / 100);      
            
            if (target.physicalResistance > 100) return (int)X;

            X -= ((float)target.defense * (float)target.defense * 0.3f) + (3 * Random.Range(1, target.intellect)) + (float)target.intellect / (float)target.Fortitude - Random.Range((float)user.technique / (float)user.Energy - (float)target.technique / (float)target.Energy, (float)user.technique);

            if (X <= 0)
            {              
                return 0;
            }

            return (int)X;
        }

        else if (!move.needsAccuracy)
        {
            miss = false;
            return move.setDamage;
        }

        miss = true;
        return 0;
    }

    public int RollForCrit(int critRate, int critMultiplier, out bool isCrit, out bool isSuperCrit)
    {
        //In this function, we generate a number 0-99 and if the number is less than crit rate, we make it a crit.
        //If it is EXACTLY 0, it is a super crit and does double damage ON TOP of the regular crit.

        isSuperCrit = false;
        isCrit = false;
        int critRoll = Random.Range(0, 99);

        if (critRoll < critRate)
        {
            if (critRoll == 0)
            {
                isSuperCrit = true;
                isCrit = true;
                return critMultiplier * 2;
            }

            isCrit = true;
            return critMultiplier;
        }
        
        return 1;
    }
    public void EnactMove()
    {
        //This is used by combat manager to finalize the move process. It finishes out all the visual cues that an attack has been made.
        //This includes showing the attack dialogue, showing a combat particle, and later on, making a particle with the damage.
        int x = GetDamage(user, target, move, out bool miss, out bool isCrit, out bool isSuperCrit);


        if (miss)
        {
                battleDialogue.ShowDialogue(user.name + " missed!" );               
                return;
        }

        else if (isCrit)
        {
            battleDialogue.ShowDialogue(user.name + move.firstText + target.name + move.secondText + x + "." + "\n CRIT!");

            //this makes the special effects for a crit if there are any or just makes the regular one if there arent
            if (isSuperCrit && move.specialFX)
            {
                Instantiate(move.bonusFX[move.bonusFX.Length - 1], combatManager.characterObjects[combatManager.pointerLocation].transform.position, combatManager.characterObjects[combatManager.pointerLocation].transform.rotation);
            } 

            else if (move.specialFX)
            {
                Instantiate(move.bonusFX[0], combatManager.characterObjects[combatManager.pointerLocation].transform.position, combatManager.characterObjects[combatManager.pointerLocation].transform.rotation);
            } 

            else
            {
                Instantiate(move.combatFX, combatManager.characterObjects[combatManager.pointerLocation].transform.position, combatManager.characterObjects[combatManager.pointerLocation].transform.rotation);
            }

        } 

        else
        {
            //This assumes no crit was made and continues to use basic text and basic effect;

            /* NOTE FOR LATER JOSH  
             * You want to give EACH MOVE an animation clip and have them use that clip when the move is used.
             * Same goes for audio cues.
             * This is genius.
            */

            battleDialogue.ShowDialogue(user.name + move.firstText + target.name + move.secondText + x + ".");
            Instantiate(move.combatFX, combatManager.characterObjects[combatManager.pointerLocation].transform.position, combatManager.characterObjects[combatManager.pointerLocation].transform.rotation);
        }
            



        combatManager.DealDamage(x, move);       
    }

    public void OnClick()
    {       
        holder.SetActive(false);
    }

}
