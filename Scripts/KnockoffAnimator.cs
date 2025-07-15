using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockoffAnimator : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public int fps = 6;
    private int frameSeparator;
    private int counter;
    private int nextSprite = 1;
    public bool destroyOnLoop;

    // Start is called before the first frame update
    void Start()
    {
        frameSeparator = 60 / fps;
        counter = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter++;
        if (counter == frameSeparator)
        {
            counter = 0;
            spriteRenderer.sprite = sprites[nextSprite];
            nextSprite++;
            if (nextSprite > sprites.Length - 1)
            {
                if (destroyOnLoop) Destroy(gameObject);
                nextSprite = 0;
            }
        }
    }
}
