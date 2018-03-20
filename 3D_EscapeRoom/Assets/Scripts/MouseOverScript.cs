using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseOverScript : MonoBehaviour
{
    public float raycastRange;
    private Text tooltipText;
    private InteractableScript targetObject;
    private PlayerInventoryScript inventory;

    void Start()
    {
        tooltipText = GameObject.Find("ToolTipText").GetComponent<Text>();
        inventory = gameObject.GetComponent<PlayerInventoryScript>();
    }

    void Update()
    {
        var hits = Physics.RaycastAll(transform.position, transform.forward, raycastRange);

        tooltipText.text = "";
        targetObject = null;

        //check for a mouseover
        if (hits.Length > 0)     //this means there is a hit
        {
            foreach (var item in hits)
            {
                if (item.transform.gameObject.GetComponent<InteractableScript>())        //if the object is an interactable and not just a wall etc
                {
                    //switch (item.transform.gameObject.GetComponent<InteractableScript>().myType)        //act depending on the object
                    //{
                    //    case InteractableScript.objectType.None:
                    //        break;
                    //    case InteractableScript.objectType.Box:
                    //        tooltipText.text = "It's a box!";
                    //        targetObject = item.transform.gameObject.GetComponent<InteractableScript>();
                    //        break;
                    //    case InteractableScript.objectType.Cylinder:
                    //        tooltipText.text = "Wow, a cylinder!";
                    //        targetObject = item.transform.gameObject.GetComponent<InteractableScript>();
                    //        break;
                    //}

                    tooltipText.text = item.transform.gameObject.GetComponent<InteractableScript>().tooltipText;        //set tooltip text
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
                    if (targetObject.collectable)        //if the object can be picked up
                    {
                        if (inventory.getUsedSlots() < inventory.inventorySize)
                        {
                            inventory.addToInventory(targetObject.myType);      //add the object to the inventory
                            Destroy(targetObject.gameObject);          //remove the object from the scene, since its been picked up
                        }
                    }

                    switch (targetObject.myType)
                    {
                        case InteractableScript.objectType.None:
                            break;
                        case InteractableScript.objectType.Box:
                            if (inventory.checkInventory(InteractableScript.objectType.TestPickup))          //if we have the test pickup
                                Debug.Log("ActionBox");
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
                                Debug.Log("Door Open");
                            break;
                    }
                }
            }
        }
    }
}
