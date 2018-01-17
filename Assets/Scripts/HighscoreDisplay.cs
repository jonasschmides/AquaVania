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
        SortedDictionary<string, float> times = GameController.GetBestTimes();

        string result = "Player scores:\n";

        if(times == null)
        {
            textOutput.text = result;
            return;
        }

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

        return string.Format("{0}m {1:0.00}s",mins,secs);
    }
}