using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private CombatManager combatManager;
    [SerializeField]
    private GameObject optionHolder, dialoguePanel;
    [SerializeField]
    private Image screen, spotLight;
    [SerializeField]
    private int fpsCounter = 30;
    [SerializeField]
    private Color black;
    [SerializeField]
    private Animation introAnimation;
    [SerializeField]
    private Sprite[] one, two, three, four, five, six;

    private Sprite[] sprites;
    private SpriteRenderer mainCharacter;
    private int frame, spriteIndex = 0;
    private bool readyToPlay = false;

    private void FixedUpdate()
    {
        if (readyToPlay && !introAnimation.isPlaying && frame >= fpsCounter)
        {
            
            frame = 0;
            if (spriteIndex != sprites.Length)
            {
                screen.sprite = sprites[spriteIndex];
                spriteIndex++;
                
            } 

            else
            {
                frame = fpsCounter;
                spriteIndex = 0;
                sprites = PickLoop(Random.Range(1, 100));
            }

        } 

        else if (readyToPlay && introAnimation.isPlaying)
        {
            mainCharacter = combatManager.characterObjects[0].GetComponent<SpriteRenderer>();
            spotLight.sprite = mainCharacter.sprite;
        }
        
        frame++;
    }
    private Sprite[] PickLoop(int i)
    {
        

        switch (i)
        {
            case > 99: return six;
            case > 95: return five;
            case > 90: return four;
            case > 85: return three;
            case > 70: return two;
            default: return one;
        }
    }

    private void OnEnable()
    {
        introAnimation.Play();

        optionHolder.SetActive(false);
        dialoguePanel.SetActive(false);

        

        spotLight.color = black;

        sprites = PickLoop(Random.Range(1, 100));

        readyToPlay = true;

    }

}
