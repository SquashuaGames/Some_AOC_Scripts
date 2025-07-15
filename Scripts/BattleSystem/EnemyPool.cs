using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public CharacterStats[] enemyTypes;
    public int[] difficultyTypes;
    public Transform[] enemyHolders;

    List<CharacterStats> enemyList = new List<CharacterStats> { };

    public List<CharacterStats> generateEnemies(out List<GameObject> enemyObjects, int enemyCount = 14, int enemyDifficulty = 14)
    {
        
        enemyObjects = new List<GameObject> { };
        int currentDifficulty = 0;
        for (int i = 0; i < enemyCount; i++)
        {
            int chosenMonster = Random.Range(0, enemyTypes.Length -1);        
            if (currentDifficulty + difficultyTypes[chosenMonster] > enemyDifficulty) break;
            enemyList.Add(enemyTypes[0]);
            currentDifficulty += difficultyTypes[chosenMonster];
            enemyObjects.Add(Instantiate(enemyTypes[chosenMonster].characterPrefab, enemyHolders[chosenMonster]));

        }
        return enemyList;
    }
}
