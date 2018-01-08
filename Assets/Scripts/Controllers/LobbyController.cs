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
	public Material skybox;
	public GameModeButton[] GameModeButtons;
	private Settings Settings;
	SteamVR_LoadLevel loader;

	void Start ()
	{
		Settings = GameObject.FindGameObjectWithTag("SETTINGS").GetComponent<Settings>();
		foreach (GameModeButton GameModeButton in GameModeButtons)
		{
			SetClickEvent(GameModeButton);
		}
		loader = new GameObject("loader").AddComponent<SteamVR_LoadLevel>();
		loader.levelName = "Battlefield";
		loader.showGrid = false;
		loader.fadeOutTime = 0.5f;
		loader.front = skybox.GetTexture("_FrontTex");
		loader.back = skybox.GetTexture("_BackTex");
		loader.left = skybox.GetTexture("_LeftTex");
		loader.right = skybox.GetTexture("_RightTex");
		loader.top = skybox.GetTexture("_UpTex");
		loader.bottom = skybox.GetTexture("_DownTex");
		loader.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
	}

	void SetClickEvent(GameModeButton GameModeButton)
	{
		GameModeButton.Button.onClick.AddListener(() => {
			Settings.GameMode = GameModeButton.GameMode;
			loader.Trigger();
		});
	}


	
}
