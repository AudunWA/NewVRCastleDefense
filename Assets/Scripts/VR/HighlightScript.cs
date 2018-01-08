using UnityEngine;

public class HighlightScript : MonoBehaviour {

    public bool highlight = false;
	private Material highLightMaterial;
	private SkinnedMeshRenderer renderer;
	private MeshRenderer _meshRenderer;
	private float timer = 0;

	// Use this for initialization
	void Start () {
		highLightMaterial = Resources.Load("HoverHighlight") as Material;
		if (gameObject?.GetComponent<SkinnedMeshRenderer>())
		{
			renderer = gameObject.GetComponent<SkinnedMeshRenderer>();
		} else if (gameObject?.GetComponent<MeshRenderer>())
		{
			_meshRenderer = gameObject.GetComponent<MeshRenderer>();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		timer += Time.fixedDeltaTime;
		
		if (highlight && renderer)
        {
	        HighlightWithRenderer();
	        timer = 0;
        } else if (renderer)
		{
			renderer.enabled = false;
		}
		if (highlight && _meshRenderer)
		{
			HighlightWithMeshRenderer();
			timer = 0;
		} else if (_meshRenderer)
		{
			_meshRenderer.enabled = false;
		}
	}

	void HighlightWithRenderer()
	{
		gameObject.layer = gameObject.layer;
		gameObject.tag = gameObject.tag;
		renderer.material = highLightMaterial;
		renderer.enabled = true;
	}

	void HighlightWithMeshRenderer()
	{
		gameObject.layer = gameObject.layer;
		gameObject.tag = gameObject.tag;
		_meshRenderer.material = highLightMaterial;
		_meshRenderer.enabled = true;
	}
}
