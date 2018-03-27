using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordInputScript : MonoBehaviour
{
    public GameObject inputOverlay;
    public string requiredPass;

    private GameObject player;              //to disable the mouse lock
    private bool correctPass = false;
    private bool overlayUp = false;

	void Start ()
    {
		
	}

    void Update()
    {
        if (overlayUp)
        {
            //check for exit button press
            if (Input.GetButtonDown("Exit"))
            {
                inputOverlay.SetActive(false);
                overlayUp = false;
            }
        }
    }

    private void LateUpdate()
    {
        //if (overlayUp) transform.parent.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().getMouseLook().SetCursorLock(false);
        //else transform.parent.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().getMouseLook().SetCursorLock(true);
    }

    public void displayInputOverlay()
    {
        inputOverlay.SetActive(true);
    }

    public void onInput()       //to be attached to the input field
    {
        if(requiredPass == inputOverlay.GetComponent<InputField>().text)
        {
            correctPass = true;
        }
    }

    public bool successfulInput()
    {
        return correctPass;
    }
}
