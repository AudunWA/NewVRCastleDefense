using UnityEngine;

public class DestroyScript : MonoBehaviour
{

	private void OnEnable()
	{
		float random = Random.Range(5, 20);
		Invoke(nameof(Destroy),random);
	}

	void Destroy()
	{
		if (gameObject?.GetComponent<FixedJoint>())
		{
			Destroy(gameObject.GetComponent<FixedJoint>());
		}
		gameObject.SetActive(false);
	}
}
