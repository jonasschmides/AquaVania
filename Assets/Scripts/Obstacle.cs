using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public AudioClip clip;
    private AudioSource src;

    private void Start()
    {
        src = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            src.pitch = Random.Range(0.9f, 1.3f);
            src.volume = other.relativeVelocity.magnitude * 0.12f;

            src.PlayOneShot(clip);
        }
    }

}
