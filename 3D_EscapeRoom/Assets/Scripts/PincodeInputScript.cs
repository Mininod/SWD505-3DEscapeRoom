using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PincodeInputScript : MonoBehaviour
{
    public int pincode;     //needs to be a 4 digit number

    private int input1 = 0;
    private int input2 = 0;
    private int input3 = 0;
    private int input4 = 0;
    private int currentInput = 1;       //input starts at 1;  This is the next number to be input

    private Text inputDisplay;

    void Start ()
    {
        inputDisplay = GameObject.Find("PincodeInputDisplay").GetComponent<Text>();
	}
	
	void Update ()
    {
        //string completeInput = input1.ToString() + input2.ToString() + input3.ToString() + input4.ToString();
        //inputDisplay.text = completeInput;

        switch (currentInput)
        {
            case 1:
                inputDisplay.text = "    ";
                break;
            case 2:
                inputDisplay.text = input1.ToString() + "   ";
                break;
            case 3:
                inputDisplay.text = input1.ToString() + input2.ToString() + "  ";
                break;
            case 4:
                inputDisplay.text = input1.ToString() + input2.ToString() + input3.ToString() + " ";
                break;
            case 5:
                inputDisplay.text = input1.ToString() + input2.ToString() + input3.ToString() + input4.ToString();
                break;
        }

    }

    public void addInput(int input)
    {
        switch (currentInput)
        {
            case 1:
                input1 = input;
                ++currentInput;
                break;
            case 2:
                input2 = input;
                ++currentInput;
                break;
            case 3:
                input3 = input;
                ++currentInput;
                break;
            case 4:
                input4 = input;
                string completeInput = input1.ToString() + input2.ToString() + input3.ToString() + input4.ToString();
                if (completeInput == pincode.ToString())
                {
                    ++currentInput;
                    //pass
                    Debug.Log("Input successful");
                }
                else
                {
                    currentInput = 1;       //reset current input
                    input1 = 0;
                    input2 = 0;
                    input3 = 0;
                    input4 = 0;
                }
                break;
        }
    }
}
