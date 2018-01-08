using UnityEngine;

public class ArrowSoundController : MonoBehaviour
{
    public AudioClip ReleaseSound;
    public AudioClip ImpactSound;
    public AudioClip ArrowMissSound;
    private AudioSource audioSource;
    public float VolLowRange { get; set; }
    public float VolHighRange { get; set; }

    public void PlayReleaseSound()
    {
        audioSource.PlayOneShot(ReleaseSound, 1f);
    }

    public void PlayImpactSound()
    {
        audioSource.PlayOneShot(ImpactSound, 1f);
    }

    public void PlayMissSound()
    {
        audioSource.PlayOneShot(ArrowMissSound, 1f);
    }
    // Use this for initialization

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();

    }
    
	// Update is called once per frame
	void Update () {
		
	}
}
