using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Provides functionality to load a scene provided from the inspector
/// </summary>
public class LoadScene : MonoBehaviour {
    [SerializeField] private SceneAsset scene;
	
    public void Load()
    {
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
    }

}
