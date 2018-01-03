using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMapOverlayController : MonoBehaviour {

	GameObject Money;
	Player Player;

	// Use this for initialization
	void Start () {
		Money = GameObject.Find("MyMoney");
		Player = GameObject.FindGameObjectWithTag("World").GetComponent<WorldController>().GoodPlayer;
	}
	
	// Update is called once per frame
	void Update () {
		Money.GetComponent<Text>().text = "$" + Player.Money;
	}
}
