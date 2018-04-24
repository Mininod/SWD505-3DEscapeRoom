using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeMonitorControllerScript : MonoBehaviour
{
    public Text[] codeMonitorTimers;
    public GameObject mainMonitor;

    public Sprite mainOverHeat;  //Inital display on the main monitor
    public Sprite mainCode1; //details to solve the first code
    public Sprite mainCode2; //details to solve the second code

    private bool displayCode1 = false;
    private bool displayCode2 = false;

    public GameObject player;

	void Start ()
    {

	}
	
	void Update ()
    {
        //Update main monitor display
        if(displayCode1)
        {
            mainMonitor.GetComponent<Image>().sprite = mainCode1;
        }
        else if(displayCode2)
        {
            mainMonitor.GetComponent<Image>().sprite = mainCode2;
        }
        else
        {
            mainMonitor.GetComponent<Image>().sprite = mainOverHeat;
        }


        //Calculate time
        float levelTimer = player.transform.GetChild(0).gameObject.GetComponent<MenuScript>().getTimeRemaining(); //get time remaining
        int minutes = Mathf.FloorToInt(levelTimer / 60);
        int seconds = Mathf.FloorToInt(levelTimer - (minutes * 60));

        foreach (Text timer in codeMonitorTimers)
        {
            //Display time remaining
            if (levelTimer <= 0)
                timer.text = "00:00:00";
            else
                timer.text = "00:" + minutes.ToString("00") + ":" + seconds.ToString("00");

            //Text colour
            if (player.transform.GetChild(0).gameObject.GetComponent<MenuScript>().isTimerBelowThreshold())
                timer.color = Color.red;
        }

    }

    public void setDisplay1(bool value)
    {
        displayCode1 = value;
    }

    public void setDisplay2(bool value)
    {
        displayCode2 = value;
    }
}
