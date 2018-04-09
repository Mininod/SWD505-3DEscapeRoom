using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipePuzzleControlScript : MonoBehaviour
{
    public int[] controlPanelCooling;
    public int[] escapePodCooling;

    public Text[] statusDisplays;

    private bool[] valves;

    private bool controlPanelCooled = false;
    private bool escapePodCooled = false;

	void Start ()
    {
        valves = new bool[15];      //there are 15 valves in our puzzle
        for (int i = 0; i < valves.Length; ++i)
            valves[i] = false;
	}
	
	void Update ()
    {
        //Check for puzzle success
		if(checkPuzzle(controlPanelCooling) && !controlPanelCooled)
        {
            Debug.Log("ControlPanelCooled");
            controlPanelCooled = true;
        }

        if(checkPuzzle(escapePodCooling) && !escapePodCooled)
        {
            Debug.Log("EscapePodCooled");
            escapePodCooled = true;
        }

        //Update valve status display
        for (int i = 0; i < valves.Length; ++i)
        {
            if (valves[i])
                statusDisplays[i].text = i + 1 + " = Open";
            else
                statusDisplays[i].text = i + 1 + " = Close";
        }
	}

    private bool checkPuzzle(int[] valvesToBeOpen)
    {
        for (int i = 0; i < valves.Length;)
        {
            if(UnityEditor.ArrayUtility.Contains(valvesToBeOpen, i + 1))       //check if the valve is meant to be true
            {
                if (valves[i]) ++i;     //if true, increment to the next valve to check
                else return false;      //if false, the puzzle is not solved, return false

            }
            else        //if the valve is not meant to be true/open
            {
                if (!valves[i]) ++i;    //if false, increment to the next valve to check
                else return false;      //if true, the puzzle is not solved
            }

            if (i == valves.Length) return true;    //if all the valves are incremented through, the puzzle must be solved
        }

        return false;
    }

    public void toggleValve(int number)
    {
        if(number <= valves.Length)     //valves are labelled as 1-15 but are 0-14 in the array
            valves[number - 1] = !valves[number - 1];
    }
}
