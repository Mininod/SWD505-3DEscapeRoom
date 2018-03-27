using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadButtonScript : MonoBehaviour
{
    public int buttonNumber;

    private PincodeInputScript controller;

	void Start ()
    {
        controller = transform.parent.gameObject.GetComponent<PincodeInputScript>();            //all the buttons are children of the controller object
	}
	
	void Update ()
    {
		
	}

    public void pressButton()
    {
        controller.addInput(buttonNumber);
    }
}
