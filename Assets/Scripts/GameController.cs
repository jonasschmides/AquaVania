using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    // Use this for initialization
    void Start () {
        Debug.Log("Welcome to AquaVania!");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
