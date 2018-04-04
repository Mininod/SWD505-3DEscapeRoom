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
    private string onClickDisplayText;
    private PlayerInventoryScript inventory;

    private SoundManagerScript soundManager;

    void Start()
    {
        tooltipText = GameObject.Find("ToolTipText").GetComponent<Text>();
        inventory = gameObject.GetComponent<PlayerInventoryScript>();

        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManagerScript>();
    }

    void Update()
    {
        clickTooltipDuration -= Time.deltaTime;

        tooltipText.text = "";          //clear tooltip
        targetObject = null;            //clear targets
        targetKeypadButton = null;      //clear targets

        var hits = Physics.RaycastAll(transform.position, transform.forward, raycastRange);         //raycast for hits
        Debug.DrawRay(transform.position, transform.forward * raycastRange, Color.green);           //Debug draw the raycast

        //check for a mouseover
        if (hits.Length > 0)     //this means there is a hit
        {
            foreach (var item in hits)      //this will iterate through all items the ray hits
            {
                if (item.transform.gameObject.GetComponent<InteractableScript>()                                //if the object is an interactable and not just a wall etc
                    && targetObject == null                                                                     //&& we haven't already targeted an interactable in front of this
                    && !item.transform.gameObject.GetComponent<InteractableScript>().getTriggerStatus())        //&& this interactable hasn't been triggered already
                {
                    tooltipText.text = item.transform.gameObject.GetComponent<InteractableScript>().hoverTooltipText;        //set tooltip text
                    targetObject = item.transform.gameObject.GetComponent<InteractableScript>();                             //set as targeted object
                }
                else if (item.transform.gameObject.GetComponent<KeypadButtonScript>() && targetKeypadButton == null)   //if the object is part of the keypad
                {
                    targetKeypadButton = item.transform.gameObject.GetComponent<KeypadButtonScript>();
                }
            }
        }

        //Act on object (if there is one)
        {
            if (Input.GetButtonDown("Action"))
            {
                if (targetObject)        //if there is an interactable object under the mouse over
                {
                    if (!targetObject.getTriggerStatus())                           //if the target hasnt been activated, get its default on click text
                    {
                        clickTooltipDuration = clickTooltipMaxDuration;             //start the timer for the click tooltip
                        onClickDisplayText = targetObject.clickTooltipText;         //get the text to be displayed on a click
                    }

                    if (targetObject.collectable)        //if the object can be picked up (does not need a second on click text, uses default)
                    {
                        if (inventory.getUsedSlots() < inventory.inventorySize)
                        {
                            inventory.addToInventory(targetObject.myType);      //add the object to the inventory
                            Destroy(targetObject.gameObject);                   //remove the object from the scene, since its been picked up
                        }
                    }

                    switch (targetObject.myType)            //if the clicked object has a requirement/condition that is passed, the text is set to textB instead of A
                    {
                        case objectType.None:
                            break;
                        case objectType.Box:
                            if (inventory.checkInventory(objectType.TestPickup))          //if we have the test pickup
                            {
                                if(!targetObject.getTriggerStatus())        //if it hasnt been triggered, trigger it
                                {
                                    Debug.Log("ActionBox");
                                    triggerInteractable();
                                }
                            }
                            break;
                        case objectType.Cylinder:
                            targetObject.GetComponent<PasswordInputScript>().displayInputOverlay();
                            break;
                        case objectType.TestPickup:
                            break;
                        case objectType.Key:
                            break;
                        case objectType.FinalDoor:
                            if (inventory.checkInventory(objectType.Key))            //if we have the key
                            {
                                if (!targetObject.getTriggerStatus())        //if it hasnt been triggered, trigger it
                                {
                                    Debug.Log("Door Open");
                                    triggerInteractable();
                                }
                            }
                            break;
                            //Explosive Components
                        case objectType.CraftedExplosive:                           
                            break;
                            /*
                        case objectType.ExplosivePlantSpot:                 //if you click on an explosive spot
                            if(inventory.checkInventory(objectType.CraftedExplosive))
                            {
                                triggerInteractable();      //so that it can only be done once
                                inventory.removeFromInventory(objectType.CraftedExplosive);         //remove the explosive if you use it
                                Debug.Log("Tick Tick");
                                soundManager.PlaySFX("Test1");
                                StartCoroutine(craftedExplosive(targetObject.gameObject));                  
                            }
                            break;
                            */
                        case objectType.LockExplosivePlantSpot:             //the spot to plant the explosive for breaking the chest open
                            if (inventory.checkInventory(objectType.LockExplosive))     //if you have the clock explosive
                            {
                                triggerInteractable();      //so that it can only be done once
                                inventory.removeFromInventory(objectType.LockExplosive);         //remove the explosive if you use it
                                Debug.Log("Tick Tick");     
                                soundManager.PlaySFX("Test1");
                                StartCoroutine(craftedExplosive(targetObject.gameObject));
                            }
                            break;
                        case objectType.Door:
                            break;
                    }
                }
                else if(targetKeypadButton)         //if there is a keypad button under the mouse
                {
                    targetKeypadButton.pressButton();       //puts the input into the controller that checks the pincode
                }
            }
        }

        //on click tooltip
        if(clickTooltipDuration > 0)
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
        clickTooltipDuration = clickTooltipMaxDuration;             //start the timer
    }

    private IEnumerator craftedExplosive(GameObject target)
    {
        GameObject explosiveSpot = target;      //save the target

        //play ticking sound for countdown to explosion

        yield return new WaitForSeconds(5);

        //play explosion sound
        //trigger explosion animation

        soundManager.PlaySFX("Test2");      //play explosion sound

        Debug.Log("Boom");

        //Open the chest (currently set to destroy)
        Destroy(explosiveSpot.transform.parent.gameObject);   
        
        Destroy(explosiveSpot);         //remove the explosive point
    }
}
