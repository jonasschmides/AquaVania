using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private AudioSource source;

    public AudioClip btn_click;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        source = GetComponent<AudioSource>();
        source.volume = 0.1f;
    }
    // Use this for initialization
    void Start () {
        Debug.Log("Welcome to AquaVania!");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Scene curScene = SceneManager.GetActiveScene();

            switch (curScene.name)
            {
                case "MainMenu":
                    Application.Quit();
                    break;
                case "Level_BasicControls_Edi":
                case "Level_Babsi":
                default:
                    LoadLevel("MainMenu");
                    break;
            }
        }
	}

    public void LoadLevel(string levelPath)
    {
        SceneManager.LoadScene(levelPath);
    }
}
