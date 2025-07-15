using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatManager : MonoBehaviour
{   
    public Party party;  
    public Image[] characterIcon = new Image[4];
    public GameObject optionHolder;
    public Image holderCharacterIcon;
    public Image[] QTBBars;
    public int QTBThreshold = 75;
      
    //private int QTBGaugeOne = 0, QTBGaugeTwo = 0, QTBGaugeThree = 0, QTBGaugeFour = 0;
    private int[] QTBGauge = new int[4];

    private MoveOptionHolder holderScript;    
    private bool IERunning = false;

    public Transform[] characterHolder = new Transform[4];    
    
    [HideInInspector]
    public List<GameObject> characterObjects;
    [HideInInspector]
    public List<GameObject> pointers;
    [HideInInspector]
    public int pointerLocation;


    public PointerBehaviour pointerBehaviour;

    [HideInInspector]
    public List<int> hp, tp, maxhp, maxtp;
    public TMP_Text[] healthText = new TMP_Text[4];
    public TMP_Text[] tpText = new TMP_Text[4];

    public EnemyPool enemyPool;

    [HideInInspector]
    public List<CharacterStats> players;
    [HideInInspector]
    public List<CharacterStats> enemies;
    [HideInInspector]
    public List<CharacterStats> allCharacters;

    public DamageCalculatorTest damageCalculator;
    


    private void Start()
    {
        pointerLocation = party.party.Length;
        holderScript = optionHolder.GetComponent<MoveOptionHolder>();
        QTBGauge[0] = 0;
        QTBGauge[1] = 0;
        QTBGauge[2] = 0;
        QTBGauge[3] = 0;        
        BattleSetupCharacters();
    }

    private void Update()
    {
        if(!IERunning)
        {
            StartCoroutine(CombatTime());
        }

    }

    public IEnumerator CombatTime()
    {
        IERunning = true;
        
        for(int i = 0; i < party.party.Length; i++)
        {           
            if (QTBGauge[i] <= QTBThreshold)
            {
                //This increases the guage of each team member but makes sure to stop increasing it if they are already ready.
                QTBGauge[i] += players[i].speed;                
                if (QTBGauge[i] > QTBThreshold)
                {
                    QTBBars[i].fillAmount = 1;
                    continue;
                }              
                QTBBars[i].fillAmount = (float)QTBGauge[i] / (float)QTBThreshold;
            }

            else if (!optionHolder.activeInHierarchy && !pointerBehaviour.targeting)
            {
                //This checks one by one if each character is ready to attack and then takes necessary steps to prepare.
                damageCalculator.user = players[i];
                holderCharacterIcon.sprite = players[i].characterIcon;
                optionHolder.SetActive(true);                 
                holderScript.jankCallQTB = i;
            }  
            
        }

        yield return new WaitForSeconds(0.5f);        
        IERunning = false;
    }

    public void QTBReset(int i)
    {
        QTBBars[i].fillAmount = 0;
        QTBGauge[i] = 0;
    }

    private void BattleSetupCharacters()
    {
        for (int i = 0; i < party.party.Length; i++)
        {           
            players[i] = party.GetPlayer(i);            
            characterObjects[i] = Instantiate(party.GetPlayer(i).characterPrefab, characterHolder[i]);
            pointers.Add(characterObjects[i].GetComponentInChildren<PointerClarifier>(true).gameObject);
            characterIcon[i].sprite = players[i].characterIcon;
            maxtp.Add(players[i].techPoints);
            maxhp.Add(players[i].health);
            hp.Add(players[i].currentHealth);
            tp.Add(players[i].currentTechPoints);
            healthText[i].text = "HP["  + hp[i] + "/" + maxhp[i] + "]";
            tpText[i].text = "TP[" + tp[i] + "/" + maxtp[i] + "]";
            Debug.Log("TP = " + tp[i] + " FullTP = " + maxtp[i]);
            allCharacters.Add(players[i]);
        }

        enemies = enemyPool.generateEnemies(out List<GameObject> enemyList);
        int x = 0;

        foreach(GameObject enemy in enemyList)
        {
            characterObjects[party.party.Length] = enemy;
            pointers.Add(characterObjects[party.party.Length].GetComponentInChildren<PointerClarifier>(true).gameObject);
            allCharacters.Add(enemies[x]);
            maxtp.Add(enemies[x].techPoints);
            maxhp.Add(enemies[x].health);
            hp.Add(enemies[x].currentHealth);
            tp.Add(enemies[x].currentTechPoints);
            x++;
        }
              
    }
    
    public void DealDamage(int x)
    {
        hp[pointerLocation] -= x;
        characterObjects[pointerLocation].GetComponent<Animation>().Play();
        for (int i = 0; i < party.party.Length; i++)
        {
            if (allCharacters[pointerLocation] == players[i])
            {                
                if (hp[pointerLocation] <= 0)
                {
                    hp[pointerLocation] = 0;
                }
                healthText[i].text = "HP[" + hp[pointerLocation] + "/" + maxhp[pointerLocation] + "]";
            } 


        }
        
        Debug.Log(hp[pointerLocation] + "" + allCharacters[pointerLocation].name);
    }

    public void StartPointerTargeting(int callBack)
    {
        pointerBehaviour.StartTargeting(pointerLocation, callBack);
    }

    public void SelectTarget(int i)
    {
        damageCalculator.target = allCharacters[i];
        damageCalculator.EnactMove(out bool isCrit, out bool isSuperCrit);

        if (isCrit && damageCalculator.move.specialFX)
        {
            Instantiate(damageCalculator.move.bonusFX[0], characterObjects[pointerLocation].transform);          
        }       
        else if (isSuperCrit && damageCalculator.move.specialFX)
        {
            Instantiate(damageCalculator.move.bonusFX[damageCalculator.move.bonusFX.Length - 1], characterObjects[pointerLocation].transform);           
        } 
        else
        {
            Instantiate(damageCalculator.move.combatFX, characterObjects[pointerLocation].transform);          
        }
    }

}
