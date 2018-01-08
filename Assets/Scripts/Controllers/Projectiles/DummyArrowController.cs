using UnityEngine;

public class DummyArrowController : MonoBehaviour
{
    private ArrowSoundController arrowSoundController;

    public bool Impact = true;
	// Use this for initialization
	void Start () {
	}

    private void OnEnable()
    {
        arrowSoundController = GetComponent<ArrowSoundController>();
        //if(Impact)arrowSoundController.PlayImpactSound();
        //else arrowSoundController.PlayMissSound();
        Invoke("Destroy", 30.0f);
    }

    private void Destroy()
    {
        gameObject.transform.parent = GameObject.Find("DummyArrowPool").transform;
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void unParent()
    {
        gameObject.transform.parent = GameObject.Find("DummyArrowPool").transform;
    }
}
