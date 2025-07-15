using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    private Transform player;
    private Rigidbody2D playerRigidbody;
    [SerializeField] private Sprite[] spriteToUse, playerSprites;
    
    public void UpdateDialogue(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }

    public void Interact(PlayerController playerController)
    {
        playerRigidbody.velocity = new Vector2(0, 0);
        GetComponent<SpriteRenderer>().sprite = GetFaceTowardsNPC(transform, player, spriteToUse);
        playerController.gameObject.GetComponent<SpriteRenderer>().sprite = GetFaceTowardsNPC(player, transform, playerSprites);

        foreach (DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if (responseEvents.DialogueObject == dialogueObject && enabled)
            {
                playerController.DialogueUI.AddResponseEvents(responseEvents.Events);
                break;
            }
        }

        playerController.DialogueUI.ShowDialogue(dialogueObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController playerController))
        {
            playerSprites = playerController.sprites;
            playerRigidbody = other.GetComponent<Rigidbody2D>();
            player = other.GetComponent<Transform>();
            playerController.interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController playerController))
        {
            if (playerController.interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                playerController.interactable = null;
            }
        }
    }

    private Sprite GetFaceTowardsNPC(Transform self, Transform player, Sprite[] spriteToUse)
    {

        Vector2 direction = new Vector2(player.position.x - self.position.x, player.position.y - self.position.y);
        int i = 0;
        if (spriteToUse.Length <= 7) return spriteToUse[0];

        if (direction.x > 0 && direction.y > 0)
        {
            i = 3;
        }
        else if (direction.x > 0 && direction.y < 0)
        {
            i = 1;
        }
        else if (direction.x < 0 && direction.y < 0)
        {
            i = 7;
        }
        else if (direction.x < 0 && direction.y > 0)
        {
            i = 5;
        }

        if (player.position.x <= self.position.x + 0.5 && player.position.x >= self.position.x - 0.5 && direction.y > 0)
        {
            return spriteToUse[4];
        }
        else if (player.position.x <= self.position.x + 0.5 && player.position.x >= self.position.x - 0.5 && direction.y < 0)
        {
            return spriteToUse[0];
        }
        else if (direction.x > 0 && player.position.y <= self.position.y + 0.5 && player.position.y >= self.position.y - 0.5)
        {
            return spriteToUse[2];
        }
        else if (direction.x < 0 && player.position.y <= self.position.y + 0.5 && player.position.y >= self.position.y - 0.5)
        {
            return spriteToUse[6]; 
        }

        return spriteToUse[i];
    }
}

