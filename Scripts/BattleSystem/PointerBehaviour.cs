using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerBehaviour : MonoBehaviour
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
                pointerLocation++;
                if (pointerLocation >= combatManager.pointers.ToArray().Length) pointerLocation = 0;
                combatManager.pointers[pointerLocation].SetActive(true);
                return;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) 
            {
                combatManager.pointers[pointerLocation].SetActive(false);
                pointerLocation--;
                if (pointerLocation < 0) pointerLocation = combatManager.pointers.ToArray().Length -1;
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
        try
        {
            combatManager.pointers[pointerLocation].SetActive(true);
        }
        catch 
        { 

        }
        
        
    }
}
