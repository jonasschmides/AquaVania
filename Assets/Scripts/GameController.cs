using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public AudioSource clickSrc;

    public GameObject bgm;
    public AudioSource bgmSrc;

    public AudioClip GameOverSound;
    public AudioClip W1BGM, W2BGM, W3BGM;


    public float warpX, warpY;

    public static GameController Instance
    {
        get;
        set;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(this);
        //DontDestroyOnLoad(transform.gameObject);

    }
    // Use this for initialization
    void Start()
    {

        //Debug.Log("Welcome to AquaVania!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Scene curScene = SceneManager.GetActiveScene();

            switch (curScene.name)
            {
                case "MainMenu":
                    Application.Quit();
                    break;
                default:
                    //Destroy(bgm);
                    //Destroy(this);
                    LoadLevel("MainMenu");
                    break;
            }
        }
        if (Input.GetKey(KeyCode.R))
        {
            LoadLevel(SceneManager.GetActiveScene().name);
        }
    }

    public void LoadLevel(string levelPath)
    {
        //Debug.Log("loading: " + levelPath);
        var oldClip = bgmSrc.clip;
        var newClip = oldClip;
        if (levelPath.StartsWith("MainMenu"))
        {
            bgmSrc.Stop();
            newClip = W1BGM;
        }else if (levelPath.StartsWith("GameOver"))
        {
            bgmSrc.Stop();
            bgmSrc.PlayOneShot(GameOverSound);
        }
        else if (levelPath.StartsWith("L1-"))
        {
            newClip = W1BGM;
        }
        else if (levelPath.StartsWith("L2-"))
        {
            newClip = W2BGM;
        }
        else if (levelPath.StartsWith("L3-"))
        {
            newClip = W3BGM;
        }

        if (bgmSrc.clip != newClip && levelPath != "MainMenu")
        {
            bgmSrc.clip = newClip;
            bgmSrc.Play();
        }
        SceneManager.LoadScene(levelPath);
    }
}