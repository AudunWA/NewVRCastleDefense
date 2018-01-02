using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelController : MonoBehaviour {

	private int _level = 1;
	public int level = 1;
	private Image[] bars;

	// Use this for initialization
	void Start () {
		bars = gameObject.GetComponentsInChildren<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (level != _level) {
			_level = level;
			int i = 0;
			foreach (Image bar in bars) {
				if (i < level) {
					bar.color = new Color32 (128, 224, 251, 255);
				} else {
					bar.color = new Color32 (128, 224, 251, 100);
				}
				i++;
			}
		}
	}
}
