using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HighlightScript : MonoBehaviour {

    public bool highlight = false;
	public Material highLightMaterial;
	private SkinnedMeshRenderer renderer;
	private MeshRenderer _meshRenderer;

	// Use this for initialization
	void Start () {
		if (gameObject?.GetComponent<SkinnedMeshRenderer>())
		{
			renderer = gameObject.GetComponent<SkinnedMeshRenderer>();
		} else if (gameObject?.GetComponent<MeshRenderer>())
		{
			_meshRenderer = gameObject.GetComponent<MeshRenderer>();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (highlight && renderer)
        {
	        gameObject.layer = gameObject.layer;
	        gameObject.tag = gameObject.tag;
	        renderer.material = highLightMaterial;
	        renderer.enabled = true;
        } else if (renderer)
		{
			renderer.enabled = false;
		}

		if (highlight && _meshRenderer)
		{
			gameObject.layer = gameObject.layer;
			gameObject.tag = gameObject.tag;
			_meshRenderer.material = highLightMaterial;
			_meshRenderer.enabled = true;
		} else if (_meshRenderer)
		{
			_meshRenderer.enabled = false;
		}
	}
}
