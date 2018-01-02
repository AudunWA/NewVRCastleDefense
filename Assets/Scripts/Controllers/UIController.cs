using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour {

	[Serializable]
	public class TypedButton {
		public GameObject button;
		public UpgradeType upgradeType;
	}

	private string tabName;
	private Button tab;
	private RectTransform panel;
	public TypedButton[] upgradeBtns;

	public enum UpgradeType
	{
		ATTACK_DAMAGE,
		ATTACK_RANGE,
		ATTACK_SPEED,
		HEALTH,
		SPEED,
		ARMOR
	}
		

	void Start() {
		tab = gameObject.GetComponent<Button> ();
		tabName = tab.GetComponentInChildren<Text> ().text;
		panel = transform.parent.GetComponent<RectTransform>();
		tab.onClick.AddListener (OnTabClick);

		foreach(TypedButton b in upgradeBtns) {
			b.button.GetComponent<Button>().onClick.AddListener(delegate{OnUpgradeClick(b.upgradeType);});
		}

		if (gameObject.transform.parent.name == "Fighter") {
			tab.interactable = false;
			tab.GetComponentInChildren<Text> ().color = new Color32 (47, 78, 91, 255);
			panel.SetAsLastSibling();
			foreach (Collider c in transform.parent.Find("Panel").GetComponentsInChildren<Collider>()) {
				c.enabled = true;
			}
		}
	}

	public void OnTabClick () {
		Transform bg = transform.parent.parent;
		foreach (Transform panel in bg) {
			if (panel != transform.parent) {
				Button tab = panel.GetComponentInChildren<Button> ();
				tab.interactable = true;
				tab.GetComponentInChildren<Text> ().color = new Color32 (150, 222, 248, 255);
				foreach (Collider c in panel.Find("Panel").GetComponentsInChildren<Collider>()) {
					c.enabled = false;
				}
			} else {
				foreach (Collider c in panel.Find("Panel").GetComponentsInChildren<Collider>()) {
					c.enabled = true;
				}
			}
		}
		tab.interactable = false;
		tab.GetComponentInChildren<Text> ().color = new Color32 (47, 78, 91, 255);
		panel.SetAsLastSibling (); // Renders last, aka. on top.
	}

	public void OnUpgradeClick(UpgradeType type) {
		GameObject go = EventSystem.current.currentSelectedGameObject;
		Button btn = go.GetComponent<Button>();
		UILevelController levelController = go.transform.parent.Find("Level").GetComponent<UILevelController>();
		// TODO: Call the actual upgrade function here.
		levelController.level++;
		// TODO: Update button text with new price.
		if (levelController.level >= 10) {
			btn.interactable = false;
		}
	}

}
