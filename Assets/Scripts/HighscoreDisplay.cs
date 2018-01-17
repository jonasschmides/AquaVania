using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreDisplay : MonoBehaviour {

    public Text textOutput;

	// Use this for initialization
	void Start () {
        Dictionary<string, float> times = GameController.GetBestTimes();

        string result = "";

        foreach(KeyValuePair<string, float> entry in times)
        {
            result += entry.Key + ": " + entry.Value + "\n";
        }

        textOutput.text = result;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
