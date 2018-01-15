using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObstacle : MonoBehaviour {

    public AudioClip clip;

    public AudioSource audioSrc;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
      
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSrc.Stop();
            audioSrc.pitch = Random.Range(0.9f, 1.2f);
            audioSrc.PlayOneShot(clip, Mathf.Min(0.02f+0.1f * Mathf.Abs(other.relativeVelocity.y), 0.3f));
        }
    }
}