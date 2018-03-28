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
            inventory[i] = objectType.None;                  //fill the inventory with blank objects
        }
    }

    public void addToInventory(objectType addType)
    {
        for (int i = 0; i < inventorySize; ++i)
        {
            if (inventory[i] == objectType.None)       //at the first 'empty' inventory slot, where the type is set to none
            {
                inventory[i] = addType;
                return;         //once the object has been added to a slot, return so its only been added to the one
            }
        }
    }

    public void removeFromInventory(objectType removeType)
    {
        for (int i = 0; i < inventorySize; ++i)
        {
            if(inventory[i] == removeType)
            {
                inventory[i] = objectType.None;
                return;
            }
        }
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

    public bool craftObject(objectType a, objectType b)
    {
        bool itemCrafted = false;

        for(int i = 0; i < recipeList.Length; ++i)          //check each recipe
        {
            if(a == recipeList[i].ComponentA && b == recipeList[i].ComponentB ||
                b == recipeList[i].ComponentA && a == recipeList[i].ComponentB)        //check that the two components being used match the recipe
            {
                for (int j = 0; j < inventorySize; ++j)
                {
                    if (inventory[j] == recipeList[i].ComponentA || inventory[j] == recipeList[i].ComponentB)
                        inventory[j] = objectType.None;              //if the object in the inventory is one of the two used, it will be removed

                    if(itemCrafted == false && inventory[j] == objectType.None)
                    {
                        inventory[j] = recipeList[i].Result;        //add the result to the first empty slot
                        itemCrafted = true; 
                    }
                }
            }
        }

        if (itemCrafted) return true;
        else return false;
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
