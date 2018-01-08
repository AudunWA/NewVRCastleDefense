using UnityEngine;
using Random = UnityEngine.Random;

public class DuplicateArrows : MonoBehaviour
{
	public GameObject copy;
	public int copyAmount;
	private float spaceBetweenArrows = 0.4f;
	public bool collision = false;
	[SerializeField] private GameObject arrowRainParticleEffect;
	private float startDistance;
	private bool split = false;
	private ObjectPooling pool;
	
	
	
	// Use this for initialization
	void Start ()
	{
		pool = GameObject.Find("ArrowRainPool").GetComponent<ObjectPooling>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}

	void ArrowFired()
	{
		startDistance = gameObject.transform.position.z;
		Invoke(nameof(Duplicate), 0.5f);
	}

    void Duplicate()
    {
	    GameObject effect = Instantiate(arrowRainParticleEffect, gameObject.transform.position, gameObject.transform.rotation);
	    for (int i = 0; i < copyAmount; i++)
	    {
		    float randomX = Random.Range(-25, 25);
		    float randomY = Random.Range(-25, 25);
		    float randomZ = Random.Range(-25, 25);
		    
		    //GameObject go = Instantiate(copy,
			    //new Vector3(gameObject.transform.position.x + (spaceBetweenArrows * randomX), gameObject.transform.position.y + (spaceBetweenArrows * randomY),
				    //gameObject.transform.position.z + (spaceBetweenArrows * randomZ)), gameObject.transform.rotation);

		    GameObject poolGo = pool.GetPooledObject();
		    poolGo.transform.position = new Vector3(gameObject.transform.position.x + (spaceBetweenArrows * randomX),
			    gameObject.transform.position.y + (spaceBetweenArrows * randomY),
			    gameObject.transform.position.z + (spaceBetweenArrows * randomZ));
		    poolGo.transform.rotation = gameObject.transform.rotation;
		    
		    poolGo.GetComponent<FollowVelocity>().followObGameObject = gameObject;
		    poolGo.SetActive(true);
	    }

	    Destroy(effect, 2.0f);
    }

	private void OnCollisionEnter(Collision other)
	{
		CancelInvoke(nameof(Duplicate));
		collision = true;
	}
}
