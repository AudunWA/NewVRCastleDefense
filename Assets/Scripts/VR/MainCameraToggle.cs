using UnityEngine;
using UnityEngine.XR;

public class MainCameraToggle : MonoBehaviour {
	void Start () {
        if (XRDevice.isPresent)
        {
            gameObject.SetActive(false);
        }
        else
        {
            GameObject.FindWithTag("VR").SetActive(false);
        }
    }
}
