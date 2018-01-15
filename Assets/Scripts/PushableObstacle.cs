using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObstacle : MonoBehaviour
{
    public AudioClip clip;
    public Rigidbody2D rigidBody;
   
    public AudioSource src;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            src.pitch = Random.Range(0.9f, 1.3f);
            src.volume = other.relativeVelocity.magnitude * 0.12f;

            src.PlayOneShot(clip);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("WaterLevel"))
        { 
            rigidBody.gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("WaterLevel"))
        {
            rigidBody.gravityScale = 1;
        }
    }
}