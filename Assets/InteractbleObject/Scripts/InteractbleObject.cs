using UnityEngine;

public class InteractbleObject : MonoBehaviour
{
    //base class for interactble objects (items, doors, levers and ext)

    protected PlayerManager player; //player interacting with the objects
    protected Collider interactableCollider; //collider enabling the img interacting when the player is close enough for interaction
    [SerializeField] protected GameObject interactableCanvas; //img indicating a player can interact with this object

    private void OnTriggerEnter(Collider other)
    {
        //OPTIONAL check for specific layer of collider

        if (player == null)
        {
            player = other.GetComponent<PlayerManager>();
        }

        if (player != null)
        {
            interactableCanvas.SetActive(true);
            player.canInteract = true;
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (player != null)
        {
            if (player.inputManager.interactionInput)
            {
                Interact(player);
                player.inputManager.interactionInput = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //OPTIONAL check for specific layer of collider

        if (player == null)
        {
            player = other.GetComponent<PlayerManager>();
        }

        if (player != null)
        {
            interactableCanvas.SetActive(false);
            player.canInteract = false;
        }
    }

    protected virtual void Interact(PlayerManager player)
    {
        Debug.Log("You have interacted");
    }
}