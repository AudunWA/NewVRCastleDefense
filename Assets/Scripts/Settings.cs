using System.Collections;
using UnityEngine;

public enum GameMode
{
	EASY, MEDIUM, HARD, INSANE, FREEPLAY
}

public class Settings : MonoBehaviour {

	public GameMode GameMode;

	void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }

	public int GetLevel() {
		switch (GameMode) {
			case GameMode.EASY:
				return 1;
			case GameMode.MEDIUM:
				return 2;
			case GameMode.HARD:
				return 3;
			case GameMode.INSANE:
				return 4;
			default:
				return 2;
		}
	}

	public bool IsFreeplay() {
		return GameMode == GameMode.FREEPLAY;
	}
}
