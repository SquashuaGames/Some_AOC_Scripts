using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerBehavior : MonoBehaviour
{
    public CombatManager combatManager;
    public DamageCalculatorTest damageCalculator;
    [HideInInspector]
    public bool targeting = false;
    public GameObject optionHolder;
    [HideInInspector]
    public int pointerLocation;
    [HideInInspector]
    public int QTBCallBack;   

    public void Update()
    {
        if (targeting)
        {
            
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Mouse1)) 
            {
                combatManager.pointers[pointerLocation].SetActive(false);                
                pointerLocation = RedirectPointer(pointerLocation, 1);
                if (pointerLocation >= combatManager.pointers.Count) pointerLocation = 0;
                combatManager.pointers[pointerLocation].SetActive(true);
                return;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) 
            {
                combatManager.pointers[pointerLocation].SetActive(false);
                pointerLocation = RedirectPointer(pointerLocation, -1);
                if (pointerLocation < 0) pointerLocation = combatManager.pointers.Count -1;
                combatManager.pointers[pointerLocation].SetActive(true);
                return;
            }
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Mouse0)) 
            {
                combatManager.pointers[pointerLocation].SetActive(false);                
                targeting = false; 
                combatManager.QTBReset(QTBCallBack);
                combatManager.pointerLocation = pointerLocation;
                combatManager.SelectTarget(pointerLocation);
                return;
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                targeting = false;
                optionHolder.SetActive(true);
                combatManager.pointers[pointerLocation].SetActive(false);
                combatManager.pointerLocation = pointerLocation;
            }

        }
    }

    public void StartTargeting(int i, int callBack)
    {
        QTBCallBack = callBack;
        targeting = true;
        pointerLocation = i;

        if (!combatManager.alive[i]) pointerLocation = RedirectPointer(i, 1);

        if (combatManager.pointers[pointerLocation] != null) combatManager.pointers[pointerLocation].SetActive(true);       
    }

    public int RedirectPointer(int i, int up) 
    {
        //If a character is dead, this should skip over them in the targeting.
        i += up;

        if (i < 0) i = combatManager.allCharacters.Count - 1;

        else if (i >= combatManager.allCharacters.Count) i = 0;


        while (!combatManager.alive[i])
        {
            i += up;

            if (i < 0) i = combatManager.allCharacters.Count -1;

            else if (i >= combatManager.allCharacters.Count) i = 0;

        }
        
        return i;
    }
}
