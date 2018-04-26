using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipePuzzleControlScript : MonoBehaviour
{
    public int[] controlPanelCooling;
    public int[] controlPanelCoolingClosed;

    public int[] escapePodCooling;
    public int[] escapePodCoolingClosed;

    public GameObject[] gateDisplays;
    public GameObject terminalSpot;
    public GameObject escapePodSpot;

    private GameLogicScript logicController;

    private bool[] valves;

    private bool controlPanelCooled = false;
    private bool escapePodCooled = false;

	void Start ()
    {
        logicController = GameObject.Find("GameLogicManager").GetComponent<GameLogicScript>();

        //spots are not cooled
        terminalSpot.GetComponent<Image>().color = Color.red;
        escapePodSpot.GetComponent<Image>().color = Color.red;

        valves = new bool[16];      //there are 16 gates in our puzzle
        for (int i = 0; i < valves.Length; ++i)     //set all gates to closed
            valves[i] = false;

        //set specific, starting open gates
        valves[1 - 1] = true;
        valves[3 - 1] = true;
        valves[4 - 1] = true;
        valves[5 - 1] = true;
        valves[7 - 1] = true;
        valves[10 - 1] = true;
        valves[11 - 1] = true;
        valves[13 - 1] = true;
	}
	
	void Update ()
    {
        //Check for puzzle success
		if(checkPuzzle(controlPanelCooling, controlPanelCoolingClosed) && !controlPanelCooled)
        {
            Debug.Log("ControlPanelCooled");
            controlPanelCooled = true;
            terminalSpot.GetComponent<Image>().color = Color.green;

            logicController.setTerminalCooled();
        }

        if(checkPuzzle(escapePodCooling, escapePodCoolingClosed) && !escapePodCooled)
        {
            Debug.Log("EscapePodCooled");
            escapePodCooled = true;
            escapePodSpot.GetComponent<Image>().color = Color.green;

            logicController.setEscapePodCooled();
        }

        //Update valve status display
        for (int i = 0; i < valves.Length; ++i)
        {
            if (valves[i])
                gateDisplays[i].SetActive(false);
            else
                gateDisplays[i].SetActive(true);
        }
	}

    private bool checkPuzzle(int[] valvesToBeOpen, int[] valvesToBeClosed)
    {
        for (int i = 0; i < valves.Length;)
        {
            foreach(int value in valvesToBeOpen)
            {
                if(value == i)
                {
                    if (valves[i])
                    {
                        //++i;     //if true, increment to the next valve to check
                        break;
                    }
                    else return false;      //if false, the puzzle is not solved, return false
                }
            }
            
            foreach(int value in valvesToBeClosed)
            {
                if(value == i)
                {
                    if (!valves[i])
                    {
                        //++i;    //if true (valve is closed), increment to the next valve to check
                        break;
                    }
                    else return false;
                }
            }

            ++i;

            if (i == valves.Length) return true;

            //if(UnityEditor.ArrayUtility.Contains(valvesToBeOpen, i + 1))       //check if the valve is meant to be true
            //{
            //    if (valves[i]) ++i;     //if true, increment to the next valve to check
            //    else return false;      //if false, the puzzle is not solved, return false

            //}
            //else if(UnityEditor.ArrayUtility.Contains(valvesToBeClosed, i + 1))
            //{
            //    if (!valves[i]) ++i;    //if true (valve is closed), increment to the next valve to check
            //    else return false;
            //}
            //else        
            //{
            //    ++i;    //if the valve os not specified open or closed, just increment to the next one
            //}

            //if (i == valves.Length) return true;    //if all the valves are incremented through, the puzzle must be solved
        }

        return false;
    }

    public void toggleValve(int number)
    {
        if(number <= valves.Length)     //valves/gates are labelled as 1-16 but are 0-15 in the array
            valves[number - 1] = !valves[number - 1];
    }
}
