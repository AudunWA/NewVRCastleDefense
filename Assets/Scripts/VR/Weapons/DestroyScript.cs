using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{

	private void OnEnable()
	{
		float random = Random.Range(0, 20);
		Invoke(nameof(Destroy),random);
	}

	private void OnDisable()
	{
		if (gameObject?.GetComponent<FixedJoint>())
		{
			Destroy(gameObject.GetComponent<FixedJoint>());
		}
		CancelInvoke(nameof(Destroy));
	}

	void Destroy()
	{
		gameObject.SetActive(false);
	}
}
