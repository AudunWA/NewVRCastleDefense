using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class ArrowSelection : MonoBehaviour
{
	private Hand hand;
	private GameObject arrowSelectionInstance;
	
	[SerializeField] private GameObject arrowSelectionPrefab;
	[SerializeField] private GameObject noArrowsPrefab;
	[SerializeField] private GameObject noBombPrefab;
	[SerializeField] private GameObject noRainPrefab;

	private float bombTimer = 30.0f;
	private float rainTimer = 60.0f;

	public float bombCoolDown = 30.0f;
	public float rainCoolDown = 60.0f;

	private HighlightScript bombArrowHighlightScript;
	private HighlightScript rainArrowHighlightScript;
	
	void OnAttachedToHand(Hand hand)
	{
		this.hand = hand;
	}

	void Update()
	{

		bombTimer += Time.deltaTime;
		rainTimer += Time.deltaTime;
		
		if (hand.controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
		{
			if (arrowSelectionInstance == null)
			{

				Quaternion rotation = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up);
				
				if (bombTimer < bombCoolDown && rainTimer < rainCoolDown)
				{
					arrowSelectionInstance = Instantiate(noArrowsPrefab, transform.position, rotation);
				}
				else if (bombTimer < bombCoolDown && rainTimer > rainCoolDown)
				{
					arrowSelectionInstance = Instantiate(noBombPrefab, transform.position, rotation);
				} else if (bombTimer > bombCoolDown && rainTimer < rainCoolDown)
				{
					arrowSelectionInstance = Instantiate(noRainPrefab, transform.position, rotation);
				}
				else
				{
					arrowSelectionInstance = Instantiate(arrowSelectionPrefab, transform.position, rotation);
				}
			}
		}
		else if (arrowSelectionInstance != null)
		{

			GameObject selectedArrow = hand.hoveringInteractable?.GetComponent<ArrowType>()?.ArrowPrefab;
			GameObject selection = hand.hoveringInteractable?.gameObject;
			if (selectedArrow != null)
			{
				Destroy(GetComponent<ArrowHand>().currentArrow);
				GetComponent<ArrowHand>().FireArrow();
				GetComponent<ArrowHand>().arrowPrefab = selectedArrow;

				if (selectedArrow.name == "BombArrow")
				{
					bombTimer = 0;
				}

				if (selectedArrow.name == "RainArrow")
				{
					rainTimer = 0;
				}
			}
			
			Destroy(arrowSelectionInstance);

			hand.hoveringInteractable = null;
		}
	}
	
	
}
