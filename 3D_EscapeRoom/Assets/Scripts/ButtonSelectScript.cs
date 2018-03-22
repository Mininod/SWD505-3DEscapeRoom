using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectScript : MonoBehaviour
{
    public GameObject outline;
    private bool selected = false;
    private MenuScript playersMenuScript;

    private void Start()
    {
        playersMenuScript = GameObject.Find("Player").transform.GetChild(0).gameObject.GetComponent<MenuScript>();          //the scripts are all on the child of the player
    }

    void Update ()
    { 
        outline.SetActive(selected);
	}

    public void toggleOutline()
    {
        int numberOfSelectedButtons = 0;
        foreach (var button in playersMenuScript.inventoryDisplay)      //for each button in the inventory
        {
            if (button.GetComponent<ButtonSelectScript>().isSelected()) ++numberOfSelectedButtons;          //count how many are selected
        }

        if(numberOfSelectedButtons < 2 || selected) selected = !selected;       //only 2 can be selected at once OR you can deselect 
    }

    public bool isSelected()
    {
        return selected;
    }
}
