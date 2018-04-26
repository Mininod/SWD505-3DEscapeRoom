using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogicScript : MonoBehaviour
{
    private bool doorAhandleAttached = false;       //check if the handle attached to door a

    public GameObject doorHandle;                   //the door handle to be shown, when attached

    private bool potatoInFuseBox = false;           //check if the fuse box is powered in room 1
    private bool doorAOpen = false;                 //check if door a is currently open

    public GameObject fuseBoxA;                     //Fusebox a without the potato
    public GameObject fuseBoxWithPotato;            //The fusebox with the takeable potato
    public GameObject doorStopSpot;                 //The spot where the doorstop can be placed
    public GameObject doorA;                        //The first door, that can be held open by the door stop

    private bool doorStopInPlace = false;           //check if the door stop is used to prop open door a

    public GameObject doorStop;                     //the door stop to keep the door open

    private bool potatoCircuitInFuseBox = false;    //check if the seconds fuse box is on

    public GameObject fuseBoxB;                     //The second fuse box
    public GameObject fuseBoxBWithPotato;           //The second fuse box with the potato circuit in

    private bool terminalCooled = false;            //check for teminal cooled for the level flip

    private bool keypadASuccess = false;            //check if the correct code has been entered into keypad A
    private bool doorBOpen = false;                 //check if boor b has been opened

    public GameObject doorB;                        //the second door, controlled by the keypad

    private bool explosivePlaced = false;           //check when the explosive has been planted
    private bool lockExploded = false;              //check to see when the crate has been opened
    private bool openedCrate = false;               //check to see if the crate has been opened in the code

    public GameObject lockExplosive;                //The explosive to blow the lid off the crate
    public GameObject valveCrateLid;                //the crate with the valve in it - the lid
    public GameObject valveHandle;                  //the valve inside the crate
    public GameObject journal10;                    //the journal inside the crate

    private bool pipeHandleAttached = false;        //check if the handle has been attached to the pipe

    public GameObject pipeValve10;                  //the 10th valve to be activated once the handle has been attached
    public GameObject pipeHandleSpot;               //the spot for the handle, so it can be deactivated once used

    private bool escapePodCooled =  false;          //check if the escape pod has been cooled

    public GameObject escapePodMonitor;             //The monitor showing the status of the escape pod
    public Sprite escapePodReady;                   //The sprite to display on the monitor when the escape pod is cooled

    public GameObject doorC;                        //the final door to be opened

    private bool keypadBSuccess = false;            //check if the correct code has been entered in to keypad B
    private bool doorCOpen = false;                 //check if the final door is open

    private SoundManagerScript soundManager;

	void Start ()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManagerScript>();
	}
	
	void Update ()
    {
        //set door handle to appear
        if(doorAhandleAttached && !doorHandle.activeSelf)
        {
            doorHandle.SetActive(true);
        }

        //Switch potato in for non potato fusebox
        if(potatoInFuseBox && !fuseBoxWithPotato.activeSelf)        //if the potato is placed in the fusebox, switch the box with potato
        {
            fuseBoxWithPotato.SetActive(true);
            fuseBoxWithPotato.GetComponent<InteractableScript>().untriggerInteractable();
            fuseBoxA.SetActive(false);
        }

        //Switch out for normal fusebox after taking potato
        if(!potatoInFuseBox && fuseBoxWithPotato.activeSelf)        //if the potato is taken, switch back to the empty fuse box
        {
            fuseBoxWithPotato.SetActive(false);
            fuseBoxA.SetActive(true);
            fuseBoxA.GetComponent<InteractableScript>().untriggerInteractable();
        }

        //Set door stop to appear
        if(doorStopInPlace && !doorStop.activeSelf)
        {
            doorStop.SetActive(true);
        }

        //Set door stop spot active if door A is open
        if (doorAOpen && !doorStopSpot.activeSelf && !doorStopInPlace)       //when door a is opened, the door stop can now be placed (as long as it hasnt all ready been placed)
        {
            doorStopSpot.SetActive(true);
        }
        else if(!doorAOpen && doorStopSpot.activeSelf)       //if the door is closed, disable the spot
        {
            doorStopSpot.SetActive(false);
        }

        //Lock open door A if door stop is in place
        if (doorAOpen || doorStopInPlace)     //checks if door A has been opened or if it is wedged open by the door stop
        {
            //door in open state
            if (doorA.activeSelf)
            {
                soundManager.PlaySFX("ElectricHum");
                doorA.SetActive(false);
            }
        }

        //Close door A if the door stop is not in place and the fuse box is off
        if(!openDoorA() && !doorStopInPlace)       //if the fuse box is unpowered, the door must be closed (as long as the door stop is not in place)
        {
            //door in close state
            doorAOpen = false;
            doorA.GetComponent<InteractableScript>().untriggerInteractable();
            if (!doorA.activeSelf) doorA.SetActive(true);
        }

        //Check that the circuit is in fuse box b
        if(potatoCircuitInFuseBox && !fuseBoxBWithPotato.activeSelf)    //when the potato is placed, switch models
        {
            fuseBoxB.SetActive(false);
            fuseBoxBWithPotato.SetActive(true);
        }

        //Open door B when the keycode is successful
        if(keypadASuccess && !doorBOpen)
        {
            doorBOpen = true;
            //open door
            soundManager.PlaySFX("ElectricHum");
            doorB.SetActive(false);
        }

        //activate lock explosive
        if(explosivePlaced && !lockExplosive.activeSelf && !lockExploded)   //only until the lock has been blown
        {
            lockExplosive.SetActive(true);
        }

        //Set valve handle in crate
        if(lockExploded && !openedCrate)    //if the lock is exploded but the crate isnt open
        {
            openedCrate = true;
            valveHandle.SetActive(true);
            journal10.SetActive(true);
            valveCrateLid.SetActive(false);
            lockExplosive.SetActive(false);
        }

        //Activate valve10 when the handle is attached
        if(pipeHandleAttached && !pipeValve10.activeSelf)
        {
            pipeValve10.SetActive(true);    //activate the valve
            pipeHandleSpot.SetActive(false);        //deactivate the spot
        }

        //update escape pod monitor
        if(escapePodCooled && (escapePodMonitor.GetComponent<Image>().sprite != escapePodReady))
        {
            escapePodMonitor.GetComponent<Image>().sprite = escapePodReady;
        }

        //Open door c when keycode is successful
        if(keypadBSuccess && !doorCOpen)
        {
            doorCOpen = true;
            //open door
            soundManager.PlaySFX("ElectricHum");
            doorC.SetActive(false);
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
            else if (terminalCooled && !escapePodCooled) return 3;      //3 is the terminal is cooled, the second puzzle wont have been solved yet
            else if (terminalCooled && escapePodCooled) return 4;       //4 if the escape pod is cooled, the terminal puzzle will have been solved
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

    public void setExplosivePlaced()
    {
        explosivePlaced = true;
    }

    public void setLockExploded()
    {
        lockExploded = true;
    }

    public void setEscapePodCooled()
    {
        escapePodCooled = true;
    }

    public void setDoorCOpen()
    {
        doorCOpen = true;
    }

    public void setKeyPadSuccess(char ID)
    {
        if (ID == 'A') keypadASuccess = true;
        if (ID =='B') keypadBSuccess = true;
    }

    public void setValveAttached()
    {
        pipeHandleAttached = true;
    }
}
