using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
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

    private static Dictionary<string, float> times;

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
        else
        {
            if (PlayerPrefs.HasKey("scores") && PlayerPrefs.GetString("scores").Length > 0)
            {
                times = DeserializeHighscores(PlayerPrefs.GetString("scores"));
            }
            else
            {
                times = new Dictionary<string, float>();
            }
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    public static string SerializeHighscores(Dictionary<string, float> highscoreDict)
    {
        string result = "";

        foreach (KeyValuePair<string, float> t in times)
        {
            result += t.Key + "," + t.Value + ";";
        }

        return result;
    }

    public static Dictionary<string, float> DeserializeHighscores(string highscoreString)
    {
        Dictionary<string, float> result = new Dictionary<string, float>();

        var kvSplit = highscoreString.Split(';');
        foreach (string kv in kvSplit)
        {
            if (kv.Length < 1) continue;
            var kvPair = kv.Split(',');
            string key = kvPair[0];
            float value;

            if (!float.TryParse(kvPair[1], out value)) continue;
            result.Add(key, value);
        }

        return result;
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


    public void LoadLevel(string levelToLoad)
    {
        LoadLevel(levelToLoad, false, "", 0f);
    }

    public void LoadLevel(string levelToLoad, bool highscoreWorthy, string highscoreFor, float timePlayed)
    {
        if (highscoreWorthy)
        {
            highscoreFor = highscoreFor.Split('_')[0];
            float oldValue = 0;
            times.TryGetValue(highscoreFor, out oldValue);

            if (oldValue > timePlayed || oldValue == 0)
            {
                if (!times.ContainsKey(highscoreFor))
                {
                    times.Add(highscoreFor, timePlayed);
                }
                else
                {
                    times[highscoreFor] = timePlayed;
                }
            }
            PlayerPrefs.SetString("scores", SerializeHighscores(times));
            PlayerPrefs.Save();
        }
        timePlayed = 0;


        //Debug.Log("loading: " + levelPath);
        var oldClip = bgmSrc.clip;
        var newClip = oldClip;
        if (levelToLoad.StartsWith("MainMenu"))
        {
            bgmSrc.Stop();
            newClip = W1BGM;
        }
        else if (levelToLoad.StartsWith("GameOver"))
        {
            bgmSrc.Stop();
            clickSrc.PlayOneShot(GameOverSound); //hackyhack
            newClip = null;
        }
        else if (levelToLoad.StartsWith("Credits"))
        {
            newClip = null;
        }
        else if (levelToLoad.StartsWith("L1-"))
        {
            newClip = W1BGM;
        }
        else if (levelToLoad.StartsWith("L2-"))
        {
            newClip = W2BGM;
        }
        else if (levelToLoad.StartsWith("L3-"))
        {
            newClip = W3BGM;
        }
        else if (levelToLoad.StartsWith("L4-"))
        {
            newClip = W4BGM;
        }

        if (bgmSrc.clip != newClip && levelToLoad != "MainMenu")
        {
            bgmSrc.clip = newClip;
            bgmSrc.Play();
        }
        SceneManager.LoadScene(levelToLoad);
    }

    public static Dictionary<string, float> GetBestTimes()
    {
        return times;
    }
}