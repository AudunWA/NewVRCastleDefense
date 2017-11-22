
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoneyController : MonoBehaviour
{
    public Player Player{ get; set; }
    public Text textField;
    private float f = 0.0f;
	// Use this for initialization
	void Start () {
	    GameObject goWorld= GameObject.FindWithTag("World");
	    GuiController guiController = goWorld.GetComponent<GuiController>();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    f += Time.deltaTime;
	    if (f >= 0.2f && Player != null)
	    {
            UpdatePlayerMoneyDisplay();
	        f = 0.0f;
	    }
    }

    public void UpdatePlayerMoneyDisplay()
    {
        if (textField != null) textField.text = ""+Player.Money;
    }
}
