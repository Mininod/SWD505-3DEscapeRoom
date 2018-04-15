﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicScript : MonoBehaviour
{
    private bool doorAhandleAttached = false;       //check if the handle attached to door a
    private bool potatoInFuseBox = false;           //check if the fuse box is powered in room 1
    private bool doorAOpen = false;                 //check if door a is currently open

    public GameObject fuseBoxA;                     //Fusebox a without the potato
    public GameObject fuseBoxWithPotato;            //The fusebox with the takeable potato
    public GameObject doorStopSpot;                 //The spot where the doorstop can be placed
    public GameObject doorA;                        //The first door, that can be held open by the door stop

    public GameObject fuseBoxB;                     //The second fuse box
    public GameObject fuseBoxBWithPotato;           //The second fuse box with the potato circuit in

    private bool doorStopInPlace = false;           //check if the door stop is used to prop open door a
    private bool potatoCircuitInFuseBox = false;    //check if the seconds fuse box is on

    private bool terminalCooled = false;            //check for teminal cooled for the level flip

    private bool doorBOpen = false;                 //check if boor b has been opened

    private bool escapePodCooled=  false;           //check if the escape pod has been cooled
    private bool doorCOpen = false;                 //check if the final door is open

	void Start ()
    {
		
	}
	
	void Update ()
    {
        if(potatoInFuseBox && !fuseBoxWithPotato.activeSelf)        //if the potato is placed in the fusebox, switch the box with potato
        {
            fuseBoxWithPotato.SetActive(true);
            fuseBoxWithPotato.GetComponent<InteractableScript>().untriggerInteractable();
            fuseBoxA.SetActive(false);
        }

        if(!potatoInFuseBox && fuseBoxWithPotato.activeSelf)        //if the potato is taken, switch back to the empty fuse box
        {
            fuseBoxWithPotato.SetActive(false);
            fuseBoxA.SetActive(true);
            fuseBoxA.GetComponent<InteractableScript>().untriggerInteractable();
        }

        if (doorAOpen && !doorStopSpot.activeSelf)       //when door a is opened, the door stop can now be placed
        {
            doorStopSpot.SetActive(true);
        }
        else if(!doorAOpen)       //if the door is closed, disable the spot
        {
            if (doorStopSpot.activeSelf) doorStopSpot.SetActive(false);
        }

        if (doorAOpen || doorStopInPlace)     //checks if door A has been opened or if it is wedged open by the door stop
        {
            //door in open state
            if (doorA.activeSelf) doorA.SetActive(false);
        }

        if(!openDoorA() && !doorStopInPlace)       //if the fuse box is unpowered, the door must be closed (as long as the door stop is not in place)
        {
            //door in close state
            doorAOpen = false;
            doorA.GetComponent<InteractableScript>().untriggerInteractable();
            if (!doorA.activeSelf) doorA.SetActive(true);
        }

        if(potatoCircuitInFuseBox && !fuseBoxBWithPotato.activeSelf)    //when the potato is placed, switch models
        {
            fuseBoxB.SetActive(false);
            fuseBoxBWithPotato.SetActive(true);
        }



	}

    public bool openDoorA()     //to open door A, the potato must be in the fuse box and the handle must be attached
    {
        if (doorAhandleAttached && potatoInFuseBox)
            return true;
        else return false;
    }

    public int flipLever()
    {
        if (!potatoCircuitInFuseBox) return 1;      //1 if potato circuit is not in fuse box
        else
        {
            if (!terminalCooled && !escapePodCooled) return 2;          //2 if circuit is in but nothing is cooled
            else if (terminalCooled && !escapePodCooled) return 3;      //3 is the terminal is cooled
            else if (terminalCooled && escapePodCooled) return 4;       //4 if the escape pod is cooled
        }

        return 0;
    }

    public void setDoorHandleAttached()
    {
        doorAhandleAttached = true;
    }

    public void setPotatoInFuseBox(bool state)
    {
        potatoInFuseBox = state;
    }

    public void setDoorAOpen()
    {
        doorAOpen = true;
    }

    public void setDoorStopInPlace()
    {
        doorStopInPlace = true;
    }

    public void setPotatoCircuitInFuseBox()
    {
        potatoCircuitInFuseBox = true;
    }

    public void setTerminalCooled()
    {
        terminalCooled = true;
    }

    public void setDoorBOpen()
    {
        doorBOpen = true;
    }

    public void setEscapePodCooled()
    {
        escapePodCooled = true;
    }

    public void setDoorCOpen()
    {
        doorCOpen = true;
    }
}