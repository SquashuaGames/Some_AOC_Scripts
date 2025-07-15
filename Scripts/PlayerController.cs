using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable interactable { get; set;}

    public float moveSpeed;
    float speedX, speedY;
    Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    int spriteToUse = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueUI.isOpen) return;
        speedX = Input.GetAxisRaw("Horizontal");
        speedY = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(speedX, speedY).normalized;
        movement.x *= moveSpeed;
        movement.y *= moveSpeed;
        rb.velocity = movement;
        FindDirection(movement);

        if (Input.GetKeyDown(KeyCode.E))
        {
                interactable?.Interact(this);
        }
        
    }

    private void FindDirection(Vector2 direction)
    {
        if (direction.x > 0 && direction.y > 0)
        {
            spriteToUse = 3;
        }
        else if (direction.x > 0 && direction.y < 0)
        {
            spriteToUse = 1;
        }
        else if (direction.x < 0 && direction.y < 0)
        {
            spriteToUse = 7;
        }
        else if (direction.x < 0 && direction.y > 0)
        {
            spriteToUse = 5;
        }
        else if (direction.x == 0 && direction.y > 0)
        {
            spriteToUse = 4;
        }
        else if (direction.x == 0 && direction.y < 0)
        {
            spriteToUse = 0;
        }
        else if (direction.x > 0 && direction.y == 0)
        {
            spriteToUse = 2;
        }
        else if (direction.x < 0 && direction.y == 0)
        {
            spriteToUse = 6;
        }

        spriteRenderer.sprite = sprites[spriteToUse]; 
    }

}
