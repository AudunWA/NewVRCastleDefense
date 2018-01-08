using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetablePlayerController : MonoBehaviour
{
	public TargetablePlayer TargetablePlayer
	{
		get { return Valve.VR.InteractionSystem.Player.instance.TargetablePlayer; }
		private set { Valve.VR.InteractionSystem.Player.instance.TargetablePlayer = value; }
	}

	private Vector3 originalPosition;

	public GameObject cameraRig;

	private float timer = 0.0f;

	private WorldController worldController;
	
	// Use this for initialization
	void Start () 
	{
		GameObject goWorld = GameObject.FindWithTag("World");
		worldController = goWorld.GetComponent<WorldController>();
		TargetablePlayer = new TargetablePlayer(worldController.goodPlayer, 100.0f, gameObject.transform.position);
		TargetablePlayer.gameObject = gameObject;
		originalPosition = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		timer += Time.deltaTime;

		if (timer > 3 && TargetablePlayer.Health < 100)
		{
			TargetablePlayer.Health += 1;
			timer = 0;
		}
		
		TargetablePlayer.Position = gameObject.transform.position;
		if (TargetablePlayer.Health <= 0)
		{
			FadeOut();
			Invoke(nameof(FadeIn),1f);
			TargetablePlayer.Health = 100;
			worldController.EvilPlayer.Money += 100;
		}
	}

	void FadeIn()
	{
		cameraRig.transform.position = originalPosition;
		SteamVR_Fade.Start( Color.clear, 1.0f );
	}

	void FadeOut()
	{
		SteamVR_Fade.Start( new Color(0.09f, 0.01f, 0f), 0.3f );
	}
}
