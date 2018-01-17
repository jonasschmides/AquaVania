using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoadText : MonoBehaviour
{

    public Canvas canvasRef;
    public Text title;
    public Text subTitle;
    public Text timer;
    float displayTime;
    string recordTime;
    string levelTime;

    // Use this for initialization
    void Start()
    {
        displayTime = 2.5f;
        canvasRef.enabled = true;

        var times = GameController.GetBestTimes();
        var sceneKey = SceneManager.GetActiveScene().name.Split('_')[0];
        if (times != null && times.ContainsKey(sceneKey))
        {
            recordTime = "Record time: " + HighscoreDisplay.TimeString(times[sceneKey]);
        }
        else
        {
            recordTime = "Record time: " + "-";
        }

    }

    // Update is called once per frame
    void Update()
    {
        levelTime = "Current time: " + HighscoreDisplay.TimeString(PlayerController.levelTime);

        timer.text = recordTime + "\n" + levelTime;

        if (title.enabled)
        {
            Color col = title.color;
            displayTime -= Time.deltaTime;
            if (displayTime <= 0)
            {
                col.a -= 0.35f * Time.deltaTime;
                title.color = col;
                subTitle.color = col;
            }
            if (col.a <= 0)
            {
                col.a = 1;
                title.color = col;
                subTitle.color = col;
                title.enabled = false;
                subTitle.enabled = false;
            }
        }
    }
}