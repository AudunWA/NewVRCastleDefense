using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UpgradeUIController : MonoBehaviour {

	private string tabName;
	private Button tab;
	private List<Button> upgradeBtns;
	private RectTransform panel;
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
		//tab.onClick.AddListener (OnTabClick);

		panel = transform.parent.GetComponent<RectTransform>();
		Transform t = panel.Find ("Panel").transform;
		upgradeBtns = new List<Button> ();
		foreach(Transform btnObject in t) {
			Button upgrade = btnObject.GetComponentInChildren<Button> ();
		}
		foreach(Button b in upgradeBtns) {
			Debug.Log (b);
		}
	}

	public void OnTabClick () {
		Transform bg = transform.parent.parent;
		foreach (Transform panel in bg) {
			Button tab = panel.GetComponentInChildren<Button> ();
			tab.interactable = true;
			tab.GetComponentInChildren<Text> ().color = new Color32 (150, 222, 248, 255);
		}
		tab.interactable = false;
		tab.GetComponentInChildren<Text> ().color = new Color32 (47, 78, 91, 255);
		panel.SetAsLastSibling (); // Renders last, aka. on top.
	}

	public void OnUpgradeClick() {

	}

}
