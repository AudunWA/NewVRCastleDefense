using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class SpawnMinionOnInteraction : MonoBehaviour
{
    [SerializeField] private SpawnType spawnType;

    private Interactable interactable;
    private WorldController worldController;

    void Awake()
    {
        worldController = GameObject.FindGameObjectWithTag("World").GetComponent<WorldController>();
    }

    // Called while hand is hovering over us
    void HandHoverUpdate(Hand hand)
    {
        if (hand.GetStandardInteractionButtonDown())
        {
            worldController.GoodPlayer.SpawnController.Spawn(spawnType);
        }
    }
}
