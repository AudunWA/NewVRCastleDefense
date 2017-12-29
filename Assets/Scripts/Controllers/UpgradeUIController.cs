using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeUIController : MonoBehaviour {

	public Color activeTab;
	private Button btn;
	private RectTransform rectTransform;

	void Awake () {
		rectTransform = transform.parent.GetComponent<RectTransform>();
	}

	void Start() {
		btn = gameObject.GetComponent<Button> ();
		btn.onClick.AddListener (OnTabClick);
	}

	public void OnTabClick () {
		Transform bg = transform.parent.parent;
		foreach (Transform panel in bg) {
			Button tab = panel.GetComponentInChildren<Button> ();
			tab.interactable = true;
			tab.GetComponentInChildren<Text> ().color = new Color32 (150, 222, 248, 255);
		}
			
		btn.interactable = false;
		btn.GetComponentInChildren<Text> ().color = new Color32 (47, 78, 91, 255);
		rectTransform.SetAsLastSibling ();
	}
}
