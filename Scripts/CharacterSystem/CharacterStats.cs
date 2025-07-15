using UnityEngine;

[CreateAssetMenu(menuName = "Character/Stats")]
public class CharacterStats : ScriptableObject
{
    public GameObject characterPrefab;
    public Sprite characterIcon;
    [HideInInspector]
    public int currentHealth;
    public int currentTechPoints;
    public bool isPlayer;
    public int[] expThresholds; 
    public int exp;
    public bool debugMode = false;
    public bool firstLoad = true;
    public int levelPoints = 0;
    public int level = 1;
    [SerializeField]
    private int strengthBase = 1, defenseBase = 1, skillBase = 1, magicBase = 1, techniqueBase = 1;
    [Space]
    [SerializeField]
    private int speedBase = 3;
    [SerializeField]
    private int speedIncremental;
    [SerializeField]
    private int intellectBase = 3;
    [SerializeField]
    private int intellectIncremental;
    [SerializeField]
    private int healthBase = 100, techPointsBase = 30;
    [SerializeField]
    private int healthIncremental;
    
    [Space]
    [SerializeField]
    private int evasionBase = 0;
    [SerializeField]
    private int accuracyBase = 0;
    [SerializeField]
    private int critRateBase = 5;
    [SerializeField]
    private int critMultiplierBase = 2;
    [SerializeField]
    private int physicalResistanceBase = 0;
    [SerializeField]
    private int energyResistanceBase = 0;
    [SerializeField]
    private int magicResistanceBase = 0;

    


    /*
    So here is how this works. There are LB, LS, EA, IA stats. (Level-Boosted, Level-Scaled, Equipment-Adjusted, Innate-Aptitude respectively)
    Every stat has the previous features as well if it is a higher tier. E.g. evasion still has an innate level based on the character even though it is an EA stat.
    LB stats can have points assigned to them to boost their total.
    LS stats grow with levels.
    EA stats can be increased by equipment.
    IA stats are not changed and have a specific value that they stay at.
    */

    // LB STATS ##### These are stats that people can assign level points to. E.g. people will allocate points to strength to boost their attack.
    [HideInInspector]
    public int strength, defense, skill, magic, technique;

    // LS STATS ##### These are stats that scale arbitrarily with each level. E.g. a character levels up and their health increases from 115 to 129
    [HideInInspector]
    public int speed, intellect, health, techPoints;

    // EA STATS ##### These are stats that don't change unless an equipment or something else modifies them. E.g. glasses may boost accuraccy
    [HideInInspector]
    public int evasion, physicalResistance, energyResistance, magicResistance, critRate, accuracy, critMultiplier;
    // IA STATS ##### These stats are ones set arbitrarily and are kept unchanged. They affect the growth of certain stats. E.g. putting a point in strength while having a 10 power, would increase your strength by 10 
    public int Power;
    public int Fortitude;
    public int Ability;
    public int Energy;
    public int Magicka;
    
    private void OnEnable()
    {
        
        if (firstLoad || debugMode)
        {
            strength = strengthBase;
            defense = defenseBase;
            magic = magicBase;
            skill = skillBase;
            technique = techniqueBase;
            speed = speedBase;
            intellect = intellectBase;
            health = healthBase;
            techPoints = techPointsBase;
            evasion = evasionBase;
            physicalResistance = physicalResistanceBase;
            energyResistance = energyResistanceBase;
            magicResistance = magicResistanceBase;
            critRate = critRateBase;
            accuracy = accuracyBase;
            critMultiplier = critMultiplierBase;            
            firstLoad = false;
            
        }
        currentTechPoints = techPoints; 
        currentHealth = health;
    }
    
    public void LevelUp()
    {
        level++;
        levelPoints++;
        techPoints += Random.Range(1, PointStatRatio(3, out int[] vs));
        speed += Random.Range(speedIncremental*2, speedIncremental);
        health += healthIncremental * Random.Range(1, XStat(vs));        
        intellect += Random.Range(intellectIncremental*2, intellectIncremental);
        currentHealth = health;
    }

    private int XStat(int[] vs)
    {
        int xStat = 0;
        for(int i = 0; i < vs.Length; i++)
        {
            if (xStat < vs[i]) xStat = vs[i];
        }
        Debug.Log(xStat);
        return xStat;
    }

    public int PointStatRatio(int callcase, out int[] ratioList)
    {
        ratioList = new int[5];
        ratioList[0] = strength/Power;
        ratioList[1] = defense/Fortitude;
        ratioList[2] = skill/Ability;
        ratioList[3] = technique/Energy;
        ratioList[4] = magic/Magicka;
        if (ratioList.Length > callcase) return ratioList[callcase];   
        return 0;
    }
}
