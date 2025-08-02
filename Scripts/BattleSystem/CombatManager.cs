using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatManager : MonoBehaviour
{   
    public Party party;  
    public Image[] characterIcon, KOIcons = new Image[4];
    public AnimationClip[] clips;

    public GameObject optionHolder;
    public Image holderCharacterIcon;
    public Image[] QTBBars;
    public int QTBThreshold = 75;
    
    private int[] QTBGauge = new int[4];

    private MoveOptionHolder holderScript;    
    private bool IERunning, dead = false;

    public Transform[] characterHolder = new Transform[4];    
    
    [HideInInspector]
    public List<GameObject> characterObjects, pointers = new List<GameObject>();
    [HideInInspector]
    public int pointerLocation;

    public PointerBehavior pointerBehavior;

    [HideInInspector]
    public List<int> hp, tp, maxhp, maxtp, speed= new List<int>();
    [HideInInspector]
    public List<bool> alive = new List<bool>();

    public TMP_Text[] healthText, tpText = new TMP_Text[4];

    public EnemyPool enemyPool;

    [HideInInspector]
    public List<CharacterStats> players, enemies, allCharacters = new List<CharacterStats>();

    public DamageCalculatorTest damageCalculator;

    public GameObject gameOver;
    
    


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
            if (alive[i])
            {
                if (QTBGauge[i] <= QTBThreshold)
                {
                    //This increases the guage of each team member but makes sure to stop increasing it if they are already ready.
                    QTBGauge[i] += speed[i];    
                
                    /*if (QTBGauge[i] > QTBThreshold)
                    {
                        QTBBars[i].fillAmount = 1;
                        continue;
                    } */ 
                
                    QTBBars[i].fillAmount = (float)QTBGauge[i] / (float)QTBThreshold;
                }

                else if (!optionHolder.activeInHierarchy && !pointerBehavior.targeting)
                {
                    //This checks one by one if each character is ready to attack and then takes necessary steps to prepare.
                    damageCalculator.user = allCharacters[i];
                    holderCharacterIcon.sprite = players[i].characterIcon;
                    optionHolder.SetActive(true);                 
                    holderScript.jankCallQTB = i;
                }  
            
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

            characterObjects[i] = (Instantiate(party.GetPlayer(i).characterPrefab, characterHolder[i]));

            pointers.Add(characterObjects[i].GetComponentInChildren<PointerClarifier>(true).gameObject);

            characterIcon[i].sprite = players[i].characterIcon;

            maxtp.Add(players[i].techPoints);
            maxhp.Add(players[i].health);

            hp.Add(players[i].currentHealth);
            tp.Add(players[i].currentTechPoints);

            speed.Add(players[i].speed);

            healthText[i].text = "HP["  + hp[i] + "/" + maxhp[i] + "]";
            tpText[i].text = "TP[" + tp[i] + "/" + maxtp[i] + "]";   
            

            allCharacters.Add(players[i]);


            if (players[i].currentHealth > 0)
            { 
                alive.Add(true);
            } 

            else
            {
                KillCharacter(i);
            }
            
        }

        enemies = enemyPool.generateEnemies(out List<GameObject> enemyList);
        int x = 0;

        foreach(GameObject enemy in enemyList)
        {
            characterObjects[party.party.Length + x] = enemy;
             
            pointers.Add(characterObjects[party.party.Length + x].GetComponentInChildren<PointerClarifier>(true).gameObject);
            allCharacters.Add(enemies[x]);

            maxtp.Add(enemies[x].techPoints);
            maxhp.Add(enemies[x].health);

            hp.Add(enemies[x].currentHealth);
            tp.Add(enemies[x].currentTechPoints);

            speed.Add(enemies[x].speed);

            alive.Add(true);
            x++;
        }
        
    }
    
    public void DealDamage(int x, Move move)
    {
        hp[pointerLocation] -= x;
        

        

        for (int i = 0; i < party.party.Length; i++)
        {
            if (allCharacters[pointerLocation] == players[i])
            {
                //Plays the special hitstun animation if the target is a player.
                characterObjects[pointerLocation].GetComponent<BattleAnimator>().PlayAnimation(new List<int> {4, 0}.ToArray(), clips[0], 1);
                healthText[i].text = "HP[" + hp[pointerLocation] + "/" + maxhp[pointerLocation] + "]";
                return;
            } 
        }

        //Plays the regular hitstun animation if the target is not a player.
        characterObjects[pointerLocation].GetComponent<BattleAnimator>().PlayAnimation(new List<int> {0, 0}.ToArray(), clips[0], 1);
    }

    public void KillCharacter(int i)
    {
        hp[i] = 0;
        speed[i] = 0;
        alive[i] = false;
        dead = true;
        for (int x = 0; x < party.party.Length; x++)
        {

            if (allCharacters[i] == players[x])
            {
                characterObjects[x].GetComponent<BattleAnimator>().PlayAnimation(new List<int> {6, 6}.ToArray(), clips[1], 1);
                QTBBars[x].fillAmount = 0;
                QTBGauge[x] = 0;
                KOIcons[x].gameObject.SetActive(true);
                healthText[x].text = "HP[" + hp[pointerLocation] + "/" + maxhp[pointerLocation] + "]";

                GameOverCheck();
                return;
            }
            
        }
                
        StartCoroutine(characterObjects[i].GetComponent<BattleAnimator>().PlayDeath());
    }

    public void StartPointerTargeting(int callBack)
    {
        pointerBehavior.StartTargeting(pointerLocation, callBack);
    }

    public void SelectTarget(int i)
    {
        damageCalculator.target = allCharacters[i];
        damageCalculator.EnactMove();

        if (hp[pointerLocation] <= 0)
        {
            KillCharacter(pointerLocation);
        }

    }

    public void GameOverCheck()
    {
        for (int x = 0; x < party.party.Length; x++)
        {

            if (alive[x])
            {
                dead = false;
                return;
            } 
            
            else
            {
                dead = true;
            }

        }

        if (dead) gameOver.SetActive(true);
    }

}
