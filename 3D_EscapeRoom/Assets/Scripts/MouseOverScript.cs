using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseOverScript : MonoBehaviour
{
    public float raycastRange;
    public float clickTooltipMaxDuration;
    private float clickTooltipDuration;
    private Text tooltipText;
    private InteractableScript targetObject;
    private KeypadButtonScript targetKeypadButton;
    private ValveControlScript targetValveControl;
    private string onClickDisplayText;
    private PlayerInventoryScript inventory;
    private MenuScript menuScript;

    private GameLogicScript logicController;
    private CodeMonitorControllerScript codeMonitorController;        //Controller for displaying the codes on the monitors to solve the keycode
    private SoundManagerScript soundManager;

    void Start()
    {
        tooltipText = GameObject.Find("ToolTipText").GetComponent<Text>();
        inventory = gameObject.GetComponent<PlayerInventoryScript>();           //on the same object
        menuScript = gameObject.GetComponent<MenuScript>();                     //on the same object

        logicController = GameObject.Find("GameLogicManager").GetComponent<GameLogicScript>();
        codeMonitorController = GameObject.Find("CodeMonitorControl").GetComponent<CodeMonitorControllerScript>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManagerScript>();
    }

    void Update()
    {
        clickTooltipDuration -= Time.deltaTime;

        tooltipText.text = "";          //clear tooltip
        targetObject = null;            //clear targets
        targetKeypadButton = null;      //clear targets
        targetValveControl = null;      //clear targets

        var hits = Physics.RaycastAll(transform.position, transform.forward, raycastRange);         //raycast for hits
        Debug.DrawRay(transform.position, transform.forward * raycastRange, Color.green);           //Debug draw the raycast

        //check for a mouseover
        if (hits.Length > 0)     //this means there is a hit
        {
            foreach (var item in hits)      //this will iterate through all items the ray hits
            {
                //if (item.transform.gameObject.GetComponent<InteractableScript>()                                //if the object is an interactable and not just a wall etc
                //    && targetObject == null                                                                     //&& we haven't already targeted an interactable in front of this
                //    && !item.transform.gameObject.GetComponent<InteractableScript>().getTriggerStatus())        //&& this interactable hasn't been triggered already
                //{
                //    tooltipText.text = item.transform.gameObject.GetComponent<InteractableScript>().hoverTooltipText;        //set tooltip text
                //    targetObject = item.transform.gameObject.GetComponent<InteractableScript>();                             //set as targeted object
                //    break;
                //}
                //else if ((item.transform.gameObject.GetComponent<KeypadButtonScript>() && targetKeypadButton == null)) //if the object is part of the keypad
                //{
                //    targetKeypadButton = item.transform.gameObject.GetComponent<KeypadButtonScript>();
                //    break;
                //}
                //else if (item.transform.gameObject.GetComponent<ValveControlScript>() && targetValveControl == null)    //if the target is a valve control
                //{
                //    targetValveControl = item.transform.gameObject.GetComponent<ValveControlScript>();
                //    break;
                //}

                if(item.transform.gameObject.GetComponent<InteractableScript>() && targetObject == null && !item.transform.gameObject.GetComponent<InteractableScript>().getTriggerStatus())
                {
                    tooltipText.text = item.transform.gameObject.GetComponent<InteractableScript>().hoverTooltipText;        //set tooltip text
                    targetObject = item.transform.gameObject.GetComponent<InteractableScript>();
                }
                else if(item.transform.gameObject.GetComponent<KeypadButtonScript>() && targetKeypadButton == null)
                {
                    targetKeypadButton = item.transform.gameObject.GetComponent<KeypadButtonScript>();
                }
                else if(item.transform.gameObject.GetComponent<ValveControlScript>() && targetValveControl == null)
                {
                    targetValveControl = item.transform.gameObject.GetComponent<ValveControlScript>();
                }
            }

            //After checking for targets, run through priority
            if(targetValveControl)
            {
                targetKeypadButton = null;
                targetObject = null;
            }
            else if(targetKeypadButton)
            {
                targetObject = null;
            }
            //else the target object will be priority
        }

        //Act on object (if there is one)
        {
            if (Input.GetButtonDown("Action"))  //when you press the "action" button
            {
                if (targetObject)        //if there is an interactable object under the mouse over
                {
                    if (targetObject.collectable)        //if the object can be picked up - pick it up (no text displayed on pickup)
                    {
                        if (inventory.getUsedSlots() < inventory.inventorySize) //only works if there is room in the inventory
                        {
                            inventory.addToInventory(targetObject.myType);      //add the object to the inventory
                            Destroy(targetObject.gameObject);                   //remove the object from the scene, since its been picked up
                        }
                    }
                    else    //if its a normal interactable (not collectable) then act on it
                    {
                        if (!targetObject.getTriggerStatus())      //if the target hasnt been activated, get its default on click text
                        {
                            if(targetObject.clickTooltipText != "")     //if the object has a click text, display it
                            {
                                clickTooltipDuration = clickTooltipMaxDuration;             //start the timer for the click tooltip
                                onClickDisplayText = targetObject.clickTooltipText;         //get the text to be displayed on a click
                            }
                        }

                        switch (targetObject.myType)            //if the clicked object has a requirement/condition that is passed, the text is set to textB instead of A
                        {
                            case objectType.DoorA:
                                if(logicController.openDoorA())     //checks for both the door handle and the fuse box being powered
                                {
                                    //open door
                                    triggerInteractable();
                                    logicController.setDoorAOpen();
                                }
                                break;
                            case objectType.DoorHandleAttachSpot:
                                if(inventory.checkInventory(objectType.DoorHandle))     //if you have the door handle
                                {
                                    triggerInteractable();
                                    targetObject.gameObject.SetActive(false);   //disable attach spot
                                    inventory.removeFromInventory(objectType.DoorHandle);
                                    logicController.setDoorHandleAttached();
                                    soundManager.PlaySFX("HandleAttach");
                                }
                                break;
                            case objectType.FuseBoxA:
                                if(inventory.checkInventory(objectType.SuperChargedPotato))     //if you have the sc potato
                                {
                                    triggerInteractable();
                                    inventory.removeFromInventory(objectType.SuperChargedPotato);
                                    logicController.setPotatoInFuseBox(true);
                                    soundManager.PlaySFX("FuseBoxZap");
                                }
                                break;
                            case objectType.FuseBoxWithPotato:
                                triggerInteractable();
                                inventory.addToInventory(objectType.SuperChargedPotato);
                                logicController.setPotatoInFuseBox(false);
                                break;
                            case objectType.DoorStopSpot:
                                if(inventory.checkInventory(objectType.DoorStop))
                                {
                                    triggerInteractable();
                                    targetObject.gameObject.SetActive(false);   //disable door stop spot
                                    inventory.removeFromInventory(objectType.DoorStop);
                                    logicController.setDoorStopInPlace();
                                    soundManager.PlaySFX("HandleAttach");
                                }
                                break;
                            case objectType.FuseBoxB:
                                if(inventory.checkInventory(objectType.PotatoCircuit))
                                {
                                    triggerInteractable();
                                    inventory.removeFromInventory(objectType.PotatoCircuit);
                                    logicController.setPotatoCircuitInFuseBox();
                                    soundManager.PlaySFX("FuseBoxZap");
                                }
                                break;
                            case objectType.Lever:
                                soundManager.PlaySFX("HandleAttach");
                                switch (logicController.flipLever())
                                {
                                    case 1:     //no potato circuit
                                        Debug.Log("Needs Power");
                                        break;
                                    case 2:     //nothing is cooled
                                        Debug.Log("Terminal needs cooling");
                                        break;
                                    case 3:     //terminal is cooled
                                        Debug.Log("Terminal is cooled");
                                        codeMonitorController.setDisplay1(true);
                                        break;
                                    case 4:     //escape pod is cooled
                                        Debug.Log("Escape Pod is cooled");
                                        codeMonitorController.setDisplay1(false);
                                        codeMonitorController.setDisplay2(true);
                                        break;
                                    case 0:
                                        break;
                                }
                                break;
                            case objectType.PipeWithoutHandle:
                                if(inventory.checkInventory(objectType.Valve))      //if you have the valve
                                {
                                    triggerInteractable();
                                    inventory.removeFromInventory(objectType.Valve);
                                    logicController.setValveAttached();
                                }
                                break;
                            case objectType.EscapePodButton:
                                menuScript.triggerWin();
                                break;
                            case objectType.Journal1:
                                menuScript.displayNote(0);
                                break;
                            case objectType.Journal2:
                                menuScript.displayNote(1);
                                break;
                            case objectType.Journal3:
                                menuScript.displayNote(2);
                                break;
                            case objectType.Journal4:
                                menuScript.displayNote(3);
                                break;
                            case objectType.Journal5:
                                menuScript.displayNote(4);
                                break;
                            case objectType.Journal6:
                                menuScript.displayNote(5);
                                break;
                            case objectType.Journal7:
                                menuScript.displayNote(6);
                                break;
                            case objectType.Journal8:
                                menuScript.displayNote(7);
                                break;
                            case objectType.Journal9:
                                menuScript.displayNote(8);
                                break;
                            case objectType.Journal10:
                                menuScript.displayNote(9);
                                break;
                            case objectType.LockExplosivePlantSpot:             //the spot to plant the explosive for breaking the chest open
                                if (inventory.checkInventory(objectType.LockExplosive))     //if you have the clock explosive
                                {
                                    triggerInteractable();      //so that it can only be done once
                                    inventory.removeFromInventory(objectType.LockExplosive);         //remove the explosive if you use it
                                    targetObject.gameObject.SetActive(false);       //disable placement spot
                                    StartCoroutine(craftedExplosive());
                                }
                                break;
                            case objectType.None:
                                break;
                            default:        //for if the object is neither collectable or has an interaction
                                break;
                        }
                    }
                }
                else if(targetKeypadButton)         //if there is a keypad button under the mouse
                {
                    targetKeypadButton.pressButton();       //puts the input into the controller that checks the pincode
                }
                else if (targetValveControl)
                {
                    targetValveControl.toggleGate();       //toggles that valve
                }
            }
        }

        //on click tooltip
        if(clickTooltipDuration > 0)        //if the timer hasnt expired
        {
            tooltipText.text = onClickDisplayText;
        }
        else if(clickTooltipDuration == 0)
        {
            onClickDisplayText = "";        //when the timer has run out, clear the text so that an on click text is only shown if a new one is input
        }

    }

    private void triggerInteractable()
    {
        targetObject.triggerInteractable();                         //trigger the interactable
        onClickDisplayText = targetObject.clickTooltipTextSuccess;  //get the success on click text
        clickTooltipDuration = clickTooltipMaxDuration;             //start the timer for displaying the text
    }

    private IEnumerator craftedExplosive()
    {
        soundManager.PlaySFX("Beep3");
        logicController.setExplosivePlaced();

        //play ticking sound for countdown to explosion

        yield return new WaitForSeconds(5);

        //play explosion sound
        //trigger explosion animation

        //play explosion sound

        Debug.Log("Boom"); 

        logicController.setLockExploded();
    }
}
