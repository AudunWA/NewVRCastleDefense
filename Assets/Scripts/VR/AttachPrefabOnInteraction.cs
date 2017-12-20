using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class AttachPrefabOnInteraction : MonoBehaviour {
    [SerializeField] private GameObject prefab;

    // Called while hand is hovering over us
    void HandHoverUpdate(Hand hand)
    {
        if (hand.GetStandardInteractionButtonDown())
        {
            // Set default flags
            Hand.AttachmentFlags flags = Hand.AttachmentFlags.SnapOnAttach | Hand.AttachmentFlags.ParentToHand |
                                         Hand.AttachmentFlags.DetachOthers;

            GameObject instance = Instantiate(prefab, transform.position, transform.rotation);
            Throwable throwable = instance.GetComponent<Throwable>();
            if (throwable != null)
                flags = throwable.attachmentFlags;

            hand.AttachObject(instance, flags);
        }
    }
}
