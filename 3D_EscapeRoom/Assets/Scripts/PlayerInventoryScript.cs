using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
    public InteractableScript.objectType[] inventory;
    public int inventorySize;

    [System.Serializable]
    public class recipe
    {
        public InteractableScript.objectType ComponentA;
        public InteractableScript.objectType ComponentB;
        public InteractableScript.objectType Result;
    }

    public recipe[] recipeList;

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

        for(int i = 0; i < recipeList.Length; ++i)          //check each recipe
        {
            if(a == recipeList[i].ComponentA && b == recipeList[i].ComponentB ||
                b == recipeList[i].ComponentA && a == recipeList[i].ComponentB)        //check that the two components being used match the recipe
            {
                for (int j = 0; j < inventorySize; ++j)
                {
                    if (inventory[j] == recipeList[i].ComponentA || inventory[j] == recipeList[i].ComponentB)
                        inventory[j] = InteractableScript.objectType.None;              //if the object in the inventory is one of the two used, it will be removed

                    if(itemCrafted == false && inventory[j] == InteractableScript.objectType.None)
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
}
