using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType { NONE, LEARN_SPEECH, SHRINK };

public class Powerup : MonoBehaviour {

    public PowerupType pType;
    private AudioSource src;

    public AudioClip clip;

    void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    public void Collect()
    {
        src.PlayOneShot(clip, 0.05f);
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, clip.length);
    }
}
