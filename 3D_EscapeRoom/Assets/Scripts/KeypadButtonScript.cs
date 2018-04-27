using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadButtonScript : MonoBehaviour
{
    public int buttonNumber;
    private string hoverText = "Press button";

    private PincodeInputScript controller;
    private SoundManagerScript soundManager;

	void Start ()
    {
        controller = transform.parent.gameObject.GetComponent<PincodeInputScript>();            //all the buttons are children of the controller object
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManagerScript>();
	}
	
	void Update ()
    {
		
	}

    public void pressButton()
    {
        controller.addInput(buttonNumber);
        soundManager.PlaySFX("Beep1");
    }

    public string getHoverText()
    {
        return hoverText;
    }
}
