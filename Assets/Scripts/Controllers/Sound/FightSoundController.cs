using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSoundController : MonoBehaviour {

    public AudioClip swooshSound;
    public AudioClip fistpunchSound;
    public AudioClip stabSound;
    public AudioClip clashSound;
    public AudioClip drawSound;
    private AudioSource audioSource;
    public float VolLowRange { get; set; }
    public float VolHighRange { get; set; }

    private List<AudioClip> audioClips = new List<AudioClip>();

    public void PlayRandomFightSound()
    {        
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)], 1f);
    }
    // Use this for initialization
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        audioClips.Add(swooshSound);
        audioClips.Add(fistpunchSound);
        audioClips.Add(stabSound);
        audioClips.Add(clashSound);
        audioClips.Add(drawSound);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
