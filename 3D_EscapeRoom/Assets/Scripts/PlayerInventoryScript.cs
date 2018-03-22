using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
    public InteractableScript.objectType[] inventory;
    public int inventorySize;

    void Start()
    {
        inventory = new InteractableScript.objectType[inventorySize];

        for (int i = 0; i < inventorySize; ++i)
        {
            inventory[i] = InteractableScript.objectType.None;                  //fill the inventory with blank objects
        }
    }

    public void addToInventory(InteractableScript.objectType addType)
    {
        for (int i = 0; i < inventorySize; ++i)
        {
            if (inventory[i] == InteractableScript.objectType.None)       //at the first 'empty' inventory slot, where the type is set to none
            {
                inventory[i] = addType;
                return;         //once the object has been added to a slot, return so its only been added to the one
            }
        }
    }

    public bool checkInventory(InteractableScript.objectType checkType)       //returns true if the object is in the inventory
    {
        for (int i = 0; i < inventorySize; ++i)
        {
            if (inventory[i] == checkType)       //if the object is found, return true
                return true;
        }

        return false;       //if after checking the whole inventory there is nothing, return false
    }

    public int getUsedSlots()
    {
        int emptySlots = 0;

        for (int i = 0; i < inventorySize; ++i)
            if (inventory[i] == InteractableScript.objectType.None)
                ++emptySlots;

        int fullSlots = inventorySize - emptySlots;
        return fullSlots;
    }

    public InteractableScript.objectType getObjectAtSlot(int slot)
    {
        return inventory[slot];
    }

    public bool craftObject(InteractableScript.objectType a, InteractableScript.objectType b)
    {
        bool itemCrafted = false;

        if (a == InteractableScript.objectType.Key && b == InteractableScript.objectType.TestPickup ||
            b == InteractableScript.objectType.Key && a == InteractableScript.objectType.TestPickup)            //check recipe
        {
            for (int i = 0; i < inventorySize; ++i)         //check all inventory slots
            {
                if (inventory[i] == InteractableScript.objectType.Key)
                    inventory[i] = InteractableScript.objectType.None;          //remove component A

                if (inventory[i] == InteractableScript.objectType.TestPickup)
                    inventory[i] = InteractableScript.objectType.None;          //remove component B

                if(itemCrafted == false && inventory[i] == InteractableScript.objectType.None)
                {
                    inventory[i] = InteractableScript.objectType.Cylinder;      //Add result to inventory
                    itemCrafted = true;
                }
            }
        }

        if (itemCrafted) return true;
        else return false;
    }
}
