using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSoundController : MonoBehaviour {
    public AudioClip punchSound;
    public AudioClip swooshSound;
    public AudioClip fistpunchSound;
    private AudioSource audioSource;
    public float VolLowRange { get; set; }
    public float VolHighRange { get; set; }

    private List<AudioClip> audioClips = new List<AudioClip>();

    public void PlayPunchSound()
    {
        audioSource.PlayOneShot(punchSound, 1f);
    }

    public void PlayRandomFightSound()
    {        
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)], 1f);
        /*int x = Random.Range(0, 3);
        if(x == 0)audioSource.PlayOneShot(punchSound);
        else if(x==1)audioSource.PlayOneShot(swooshSound);
        else audioSource.PlayOneShot(fistpunchSound);*/
    }
    // Use this for initialization
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        audioClips.Add(punchSound);
        audioClips.Add(swooshSound);
        audioClips.Add(fistpunchSound);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
