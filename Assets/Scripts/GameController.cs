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
    public AudioClip W1BGM, W2BGM, W3BGM, W4BGM;


    public float warpX, warpY;

    public string reloadName;

    public static GameController Instance
    {
        get;
        set;
    }

    void Awake()
    {
        Screen.SetResolution(1024, 600, false);
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(this);
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
                    LoadLevel("MainMenu");
                    break;
            }
        }
        if (Input.GetKey(KeyCode.R))
        {
            Scene curScene = SceneManager.GetActiveScene();
            if (curScene.name != "GameOver")
            {
                LoadLevel(SceneManager.GetActiveScene().name);
            }
            else
            {
                LoadLevel(reloadName);
            }
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
        }
        else if (levelPath.StartsWith("GameOver"))
        {
            bgmSrc.Stop();
            clickSrc.PlayOneShot(GameOverSound); //hackyhack
            newClip = null;
        }else if (levelPath.StartsWith("Credits"))
        {
            newClip = null;
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
        else if (levelPath.StartsWith("L4-"))
        {
            newClip = W4BGM;
        }

        if (bgmSrc.clip != newClip && levelPath != "MainMenu")
        {
            bgmSrc.clip = newClip;
            bgmSrc.Play();
        }
        SceneManager.LoadScene(levelPath);
    }
}