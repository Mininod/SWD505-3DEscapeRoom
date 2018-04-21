using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeMonitorControllerScript : MonoBehaviour
{
    public Text codeMonitor1, codeMonitor2, codeMonitor3, codeMonitor4;

    private bool displayCode1 = false;
    private bool displayCode2 = false;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		if(displayCode1)
        {
            codeMonitor1.text = "code1";
            codeMonitor2.text = "code1";
            codeMonitor3.text = "code1";
            codeMonitor4.text = "code1";
        }
        else if(displayCode2)
        {
            codeMonitor1.text = "code2";
            codeMonitor2.text = "code2";
            codeMonitor3.text = "code2";
            codeMonitor4.text = "code2";
        }
        else
        {
            codeMonitor1.text = "----";
            codeMonitor2.text = "----";
            codeMonitor3.text = "----";
            codeMonitor4.text = "----";
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
