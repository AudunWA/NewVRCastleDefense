using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class ArrowSelection : MonoBehaviour
{
	private Hand hand;
	private GameObject arrowSelectionInstance;
	
	[SerializeField] private GameObject arrowSelectionPrefab;


	void OnAttachedToHand(Hand hand)
	{
		this.hand = hand;
	}

	void Update()
	{
		if (hand.controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
		{
			if (arrowSelectionInstance == null)
			{

				Quaternion rotation = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up);
				arrowSelectionInstance = Instantiate(arrowSelectionPrefab, transform.position, rotation);
			}
		}
		else if (arrowSelectionInstance != null)
		{
			Destroy(arrowSelectionInstance);

			GameObject selectedArrow = hand.hoveringInteractable?.GetComponent<ArrowType>()?.ArrowPrefab;
			if (selectedArrow != null)
				GetComponent<ArrowHand>().arrowPrefab = selectedArrow;
			
			hand.hoveringInteractable = null;
		}
	}
	
	
}
