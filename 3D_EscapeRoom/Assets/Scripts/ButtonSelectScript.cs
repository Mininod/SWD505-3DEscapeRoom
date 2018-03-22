using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectScript : MonoBehaviour
{
    public GameObject outline;
    private bool selected = false;

	void Update ()
    { 
        outline.SetActive(selected);
	}

    public void toggleOutline()
    {
        selected = !selected;      
    }

    public bool isSelected()
    {
        return selected;
    }
}
