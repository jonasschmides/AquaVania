using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public AudioSource clickSrc;

    public GameObject bgm;
    public AudioSource bgmSrc;

    public float warpX, warpY;

    public static GameController Instance
    {
        get;
        set;
    }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(transform.gameObject);
        DontDestroyOnLoad(bgm);
    }
    // Use this for initialization
    void Start () {
        //Debug.Log("Welcome to AquaVania!");
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
