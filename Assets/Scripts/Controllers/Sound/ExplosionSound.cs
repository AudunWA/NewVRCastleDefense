using UnityEngine;

public class ExplosionSound : MonoBehaviour {
    public AudioClip bombAudio;
    private AudioSource audioSource;
    public float VolLowRange { get; set; }
    public float VolHighRange { get; set; }
    private float timer = 0.0f;
    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(bombAudio, 0.4f);
    }

    // Update is called once per frame
    void Update ()
    {
    }
}
