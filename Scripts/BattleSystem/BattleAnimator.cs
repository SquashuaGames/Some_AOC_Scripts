using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimator : MonoBehaviour
{
    public Sprite[] sprites;
    public Animation animation;
    public SpriteRenderer spriteRenderer;
    public GameObject deathParticle;

    private int[] spriteIndices;
    private int spriteIndexIndex = 0;
    private int fpsCounter = 1000;
    private int i = 0;
    private bool readyToPlay;
    private AnimationClip animationClip;

    public void PlayAnimation(int[] spriteIndices, AnimationClip animationClip, int fps)
    {
        //Same concept as knockoff animator but the goal is to play something once and then stop.
        //Also has the benefit of not taking a gajillion different animation clips for each character and move.
        //All this will do is the move will have a clip that it plays that just moves the transform around.
        //Then the character will also change sprites. This is useful so that each character can use sprites without making a new clip.

        readyToPlay = true;
        this.animationClip = animationClip;
        this.spriteIndices = spriteIndices;
        fpsCounter = 60 / fps;
        spriteIndexIndex = 0;
        i = fpsCounter;
    }

    public IEnumerator PlayDeath()
    {
        yield return new WaitForSeconds(0.6f);
        Instantiate(deathParticle, transform);
        animation.Stop();
        i = 0;
        spriteRenderer.sprite = null;
        yield return null;
    }


    private void Update()
    {
        if(animation.isPlaying || readyToPlay)
        {
            readyToPlay = false;

            if (!animation.isPlaying) animation.PlayQueued(animationClip.name);

            if (i >= fpsCounter)
            {

                if (spriteIndices.Length !>= spriteIndexIndex) spriteRenderer.sprite = sprites[spriteIndices[spriteIndexIndex]];
                spriteIndexIndex++;
                i = 0;
            }

            i++;

        }
    }
}
