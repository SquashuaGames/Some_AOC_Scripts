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
        isSuperCrit = false;
        isCrit = false;
        int critRoll = Random.Range(0, 100);
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
    public void EnactMove(out bool isCrit, out bool isSuperCrit)
    {
        int x = GetDamage(user, target, move, out bool miss, out isCrit, out isSuperCrit);
        if (miss)
        {
                battleDialogue.ShowDialogue(user.name + " missed!" );               
                return;
        }
        if (isCrit)
        {
            battleDialogue.ShowDialogue(user.name + move.firstText + target.name + move.secondText + x + "." + "\n CRIT!");
        }

        battleDialogue.ShowDialogue(user.name + move.firstText + target.name + move.secondText + x + ".");
        combatManager.DealDamage(x);
        
    }

    public void OnClick()
    {       
        holder.SetActive(false);
    }
}
