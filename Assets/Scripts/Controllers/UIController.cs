using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Valve.VR.InteractionSystem;

public class UIController : MonoBehaviour {

	[Serializable]
	public class TypedButton
	{
		public GameObject button;
		public MinionAttribute attr;
	}

	private Player Player;
	private string tabName;
	private Button tab;
	private RectTransform panel;
	public TypedButton[] upgradeBtns;

	void Start()
	{
		Player = Player = GameObject.FindGameObjectWithTag("World").GetComponent<WorldController>().GoodPlayer;
		tab = gameObject.GetComponent<Button> ();
		tabName = tab.GetComponentInChildren<Text> ().text;
		panel = transform.parent.GetComponent<RectTransform>();
		tab.onClick.AddListener (OnTabClick);

		foreach(TypedButton b in upgradeBtns)
		{
			b.button.GetComponent<Button>().onClick.AddListener(delegate{OnUpgradeClick(b.attr);});
			SpawnType spawnType = GetSpawnType(tabName);
			int price = Player.SpawnController.GetUpgradeCost(spawnType, b.attr);
			b.button.GetComponentInChildren<Text>().text = "$" + price;
		}

		if (gameObject.transform.parent.name == "Fighter")
		{
			tab.interactable = false;
			tab.GetComponentInChildren<Text> ().color = new Color32 (47, 78, 91, 255);
			panel.SetAsLastSibling();
			foreach (Collider c in transform.parent.Find("Panel").GetComponentsInChildren<Collider>()) {
				c.enabled = true;
			}
		}
	}

	public void OnTabClick ()
	{
		Transform bg = transform.parent.parent;
		foreach (Transform panel in bg)
		{
			if (panel != transform.parent)
			{
				Button tab = panel.GetComponentInChildren<Button> ();
				tab.interactable = true;
				tab.GetComponentInChildren<Text> ().color = new Color32 (150, 222, 248, 255);
				foreach (Collider c in panel.Find("Panel").GetComponentsInChildren<Collider>())
				{
					c.enabled = false;
				}
			} else
			{
				foreach (Collider c in panel.Find("Panel").GetComponentsInChildren<Collider>())
				{
					c.enabled = true;
				}
			}
		}
		tab.interactable = false;
		tab.GetComponentInChildren<Text> ().color = new Color32 (47, 78, 91, 255);
		panel.SetAsLastSibling (); // Renders last, aka. on top.
	}

	public void OnUpgradeClick(MinionAttribute attr)
	{
		GameObject go = EventSystem.current.currentSelectedGameObject;
		Button btn = go.GetComponent<Button>();
		UILevelController levelController = go.transform.parent.Find("Level").GetComponent<UILevelController>();
		SpawnType spawnType = GetSpawnType(tabName);
	    if (attr == MinionAttribute.Range && (spawnType == SpawnType.Tank || spawnType == SpawnType.Fighter)) return;
        if (Player.SpawnController.UpgradeMinionType(spawnType, attr))
		{
		  
            levelController.level++;
		}
		int price = Player.SpawnController.GetUpgradeCost(spawnType, attr);
		btn.GetComponentInChildren<Text>().text = "$" + price;
		if (levelController.level >= 10)
		{
			btn.interactable = false;
			btn.GetComponent<Interactable>().enabled = false;
		}
	}

	private SpawnType GetSpawnType(string name)
	{
		return (SpawnType) Enum.Parse(typeof(SpawnType), name);
	}

}
