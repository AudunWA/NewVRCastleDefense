using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpawnMinionController : MonoBehaviour {

	public Text fighterText;
	public Text archerText;
	public Text mageText;
	public Text tankText;
	Player Player;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("World").GetComponent<WorldController>().GoodPlayer;
	}
	
	// Update is called once per frame
	void Update () {
		fighterText.text = GetSpawnText(SpawnType.Fighter);
		archerText.text = GetSpawnText(SpawnType.Archer);
		mageText.text = GetSpawnText(SpawnType.Mage);
		tankText.text = GetSpawnText(SpawnType.Tank);
	}

	string GetSpawnText(SpawnType type) {
		MinionStat stat = Player.MinionStatistics[type];
		int level = stat.GetLevel();
		int cost = stat.Cost;
		return $"{type}\n<size=8>lvl. {level}</size>\n\n${cost}";
	}
}