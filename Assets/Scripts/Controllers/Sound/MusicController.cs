using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
    public AudioClip backgroundMusic;
    private AudioSource audioSource;
    public float VolLowRange { get; set; }
    public float VolHighRange { get; set; }
    public void PlayMusic()
    {
        audioSource.PlayOneShot(backgroundMusic, 1f);
        audioSource.loop = true;
    }

    public void StopMusic()
    {
        audioSource.Stop();
        audioSource.loop = false;
    }
    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
