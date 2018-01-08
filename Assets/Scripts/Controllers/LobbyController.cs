using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[Serializable]
public class GameModeButton
{
	public Button Button;
	public GameMode GameMode;
}

public class LobbyController : MonoBehaviour
{
	public GameModeButton[] GameModeButtons;
	private Settings Settings;

	void Start ()
	{
		Settings = GameObject.FindGameObjectWithTag("SETTINGS").GetComponent<Settings>();
		foreach (GameModeButton GameModeButton in GameModeButtons)
		{
			SetClickEvent(GameModeButton);
		}
	}

	void SetClickEvent(GameModeButton GameModeButton)
	{
		GameModeButton.Button.onClick.AddListener(() => {
			Settings.GameMode = GameModeButton.GameMode;
			SteamVR_LoadLevel.Begin("Battlefield");
		});
	}


	
}
