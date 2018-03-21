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
    private string onClickDisplayText;
    private PlayerInventoryScript inventory;

    void Start()
    {
        tooltipText = GameObject.Find("ToolTipText").GetComponent<Text>();
        inventory = gameObject.GetComponent<PlayerInventoryScript>();
    }

    void Update()
    {
        clickTooltipDuration -= Time.deltaTime;

        tooltipText.text = "";          //clear tooltip
        targetObject = null;            //clear targets

        var hits = Physics.RaycastAll(transform.position, transform.forward, raycastRange);         //raycast for hits

        //check for a mouseover
        if (hits.Length > 0)     //this means there is a hit
        {
            foreach (var item in hits)
            {
                if (item.transform.gameObject.GetComponent<InteractableScript>())        //if the object is an interactable and not just a wall etc
                {                                                                                                                      
                    tooltipText.text = item.transform.gameObject.GetComponent<InteractableScript>().hoverTooltipText;        //set tooltip text
                    targetObject = item.transform.gameObject.GetComponent<InteractableScript>();                        //set as targeted object
                }
            }
        }

        //Act on object (if there is one)
        {
            if (Input.GetButtonDown("Action"))
            {
                if (targetObject)        //if there is an object under the mouse over
                {
                    clickTooltipDuration = clickTooltipMaxDuration;         //start the timer for the click tooltip
                    if (targetObject.getTriggerStatus())
                        onClickDisplayText = targetObject.clickTooltipTextPostSuccess;
                    else
                        onClickDisplayText = targetObject.clickTooltipText;    //get the text to be displayed on a click

                    if (targetObject.collectable)        //if the object can be picked up (does not need a second on click text)
                    {
                        if (inventory.getUsedSlots() < inventory.inventorySize)
                        {
                            inventory.addToInventory(targetObject.myType);      //add the object to the inventory
                            Destroy(targetObject.gameObject);                   //remove the object from the scene, since its been picked up
                        }
                    }

                    switch (targetObject.myType)            //if the clicked object has a requirement/condition that is passed, the text is set to textB instead of A
                    {
                        case InteractableScript.objectType.None:
                            break;
                        case InteractableScript.objectType.Box:
                            if (inventory.checkInventory(InteractableScript.objectType.TestPickup))          //if we have the test pickup
                            {
                                Debug.Log("ActionBox");
                                targetObject.triggerInteractable();
                                onClickDisplayText = targetObject.clickTooltipTextSuccess;
                            }
                            break;
                        case InteractableScript.objectType.Cylinder:
                            Debug.Log("ActionCylinder");
                            break;
                        case InteractableScript.objectType.TestPickup:
                            break;
                        case InteractableScript.objectType.Key:
                            break;
                        case InteractableScript.objectType.FinalDoor:
                            if (inventory.checkInventory(InteractableScript.objectType.Key))            //if we have the key
                            {
                                Debug.Log("Door Open");
                                targetObject.triggerInteractable();
                                onClickDisplayText = targetObject.clickTooltipTextSuccess;
                            }
                            break;
                    }
                }
            }
        }

        //on click tooltip
        if(clickTooltipDuration > 0)
        {
            tooltipText.text = onClickDisplayText;
        }

    }
}
