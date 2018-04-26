using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
    public objectType[] inventory;
    public int inventorySize;

    [System.Serializable]
    public class recipe
    {
        public objectType ComponentA;
        public objectType ComponentB;
        public objectType Result;
    }

    public recipe[] recipeList;

    void Start()
    {
        inventory = new objectType[inventorySize];

        for (int i = 0; i < inventorySize; ++i)
        {
            inventory[i] = objectType.None;                  //fill the inventory with empty slots
        }
    }
        
    public void addToInventory(objectType addType)      //passes in an object to add
    {
        for (int i = 0; i < inventorySize; ++i)
        {
            if (inventory[i] == objectType.None)       //at the first 'empty' inventory slot
            {
                inventory[i] = addType;
                return;         //once the object has been added to a slot, return so its only been added to the one
            }
        }
    }

    public void removeFromInventory(objectType removeType)          //for removing a specific object type
    {
        for (int i = 0; i < inventorySize; ++i)
        {
            if (inventory[i] == removeType)
            {
                inventory[i] = objectType.None;
                return;
            }
        }
    }

    public void removeFromInventorySlot(int slot)       //for removing an object at a slot in the inventory
    {
        inventory[slot] = objectType.None;
    }

    public bool checkInventory(objectType checkType)       //returns true if the object is in the inventory
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
            if (inventory[i] == objectType.None)
                ++emptySlots;

        int fullSlots = inventorySize - emptySlots;
        return fullSlots;
    }

    public objectType getObjectAtSlot(int slot)
    {
        return inventory[slot];
    }

    //----------------------------------------Testing----------------------------------------
    public void forceSetInventorySize(int size)
    {
        inventory = new objectType[size];

        for (int i = 0; i < size; ++i)
        {
            inventory[i] = objectType.None;                  //fill the inventory with blank objects
        }
    }
}
