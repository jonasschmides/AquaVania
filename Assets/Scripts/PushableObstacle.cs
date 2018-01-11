using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObstacle : MonoBehaviour
{
    public AudioClip clip;
    public Rigidbody2D rigidBody;
   
    public AudioSource src;
    private bool _isInWater = false;

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
            _isInWater = true;
            rigidBody.gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("WaterLevel"))
        {
            _isInWater = false;
            rigidBody.gravityScale = 1;
        }
    }
}
