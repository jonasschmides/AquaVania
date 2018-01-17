using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreDisplay : MonoBehaviour
{

    public Text textOutput;

    // Use this for initialization
    void Start()
    {
        Dictionary<string, float> times = GameController.GetBestTimes();

        string result = "";

        foreach (KeyValuePair<string, float> entry in times)
        {
            result += entry.Key + ": " + TimeString(entry.Value) + "\n";
        }

        textOutput.text = result;
    }

    public static string TimeString(float seconds)
    {
        var mins = (int)(seconds / 60);
        var secs = Mathf.Round(100*(seconds -60*mins))/100;
        //var split = Mathf.Round(100*(seconds - mins * 60 - secs))/100;

        return mins + "m " + secs+ "s";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
