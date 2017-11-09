using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private AudioSource source;

    public AudioClip btn_click;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        source = GetComponent<AudioSource>();
        source.volume = 0.5f;
    }
    // Use this for initialization
    void Start () {
        Debug.Log("Welcome to AquaVania!");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
