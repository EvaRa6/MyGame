using UnityEngine;

public class InteractbleObject : MonoBehaviour
{
    //base class for interactble objects (items, doors, levers and ext)

    protected PlayerManager player; //player interacting with the objects
    protected Collider interactableCollider; //collider enabling the img interacting when the player is close enough for interaction
    [SerializeField] protected GameObject interactableImage; //img indicating a player can interact with this object

    private void OnTriggerEnter(Collider other)
    {
        //OPTIONAL check for specific layer of collider

        if (player == null)
        {
            player = other.GetComponent<PlayerManager>();
        }

        if (player != null)
        {
            interactableImage.SetActive(true);
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
            interactableImage.SetActive(false);
        }
    }
}