using UnityEngine;

public class MiniMapCameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Camera>().aspect = 4f / 3f;
	}
	
}
