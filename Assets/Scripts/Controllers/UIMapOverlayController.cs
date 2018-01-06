using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMapOverlayController : MonoBehaviour {

	GameObject Money;
	Player Player;
	public Image EnemyCastleHealthBar;
	public Text EnemyCastleHealthText;
	public Image MyCastleHealthBar;
	public Text MyCastleHealthText;

	// Use this for initialization
	void Start () {
		Money = transform.Find("MyMoney").gameObject;
		Player = GameObject.FindGameObjectWithTag("World").GetComponent<WorldController>().GoodPlayer;
	}
	
	// Update is called once per frame
	void Update () {
		Money.GetComponent<Text>().text = "$" + Player.Money;
		EnemyCastleHealthText.text = Mathf.Floor(EnemyCastleHealthBar.fillAmount * 100) + "/100";
		MyCastleHealthText.text = Mathf.Floor(MyCastleHealthBar.fillAmount * 100) + "/100";
	}
}
