using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    private GameObject menuOverlay;
    private GameObject gameOverPanel;
    private bool menuUp = false;
    private PlayerInventoryScript inventory;
    private Text timerDisplay;
    private GameObject reticle;

    [Tooltip("Max time for the level, in minutes")]
    public int timerMax;
    private float levelTimer;
    [Tooltip("Time at which the game indicates you are running out of time, in minutes")]
    public int shortTimer;
    private bool atShortTimer = false;
    private bool gameOver = false;

    public GameObject[] inventoryDisplay;           //will be the size of the inventiry, set by the designer

    void Start()
    {
        inventory = gameObject.GetComponent<PlayerInventoryScript>();
        timerDisplay = GameObject.Find("TimerDisplay").GetComponent<Text>();

        //reticle
        reticle = GameObject.Find("Reticle");
        if (!SettingsMenuScript.reticleOn) reticle.SetActive(false);            //turn off the reticle if it is off in the settings

        menuOverlay = GameObject.Find("Canvas").transform.GetChild(0).gameObject;       //child 0 of the canvas is the menu/inventory overlay

        levelTimer = timerMax * 60;         //* 60 to turn it from minutes to seconds 

        //Game over screen
        gameOverPanel = GameObject.Find("Canvas").transform.GetChild(1).gameObject;     //child 1 of the canvas is the game over panel

        //Set game over screen elements to 0 alpha - I could do this in the editor, but then they are hard to edit
        Color panelColor0 = gameOverPanel.GetComponent<Image>().color;
        panelColor0.a = 0;
        gameOverPanel.GetComponent<Image>().color = panelColor0;

        Color textColor0 = gameOverPanel.transform.GetChild(0).gameObject.GetComponent<Text>().color;
        textColor0.a = 0;
        gameOverPanel.transform.GetChild(0).gameObject.GetComponent<Text>().color = textColor0;
    }

    void Update()
    {
        //Timer
        levelTimer -= Time.deltaTime;
        displayTimer();

        //check if timer has reached the lower limit
        if (levelTimer <= (shortTimer * 60))         //level timer is in seconds (* 60) so when the level timer reaches the marker
        {
            if (!atShortTimer)
            {
                //trigger all things to be triggered at this time, music chnage etc
                timerDisplay.color = Color.red;
                atShortTimer = true;
            }
        }

        //Check for time up
        timeUpCheck();

        //Inventory display
        for (int i = 0; i < inventory.inventorySize; ++i)
        {
            //returns the object at the slot and puts it in the text at the on screen slot - child 1 is the button, child 0 is the text
            inventoryDisplay[i].transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = inventory.getObjectAtSlot(i).ToString();    
        }

        //check to bring up menu
        if (Input.GetButtonDown("Menu"))
        {
            if (menuOverlay.activeSelf)                 //if the menu is active
            {
                menuOverlay.SetActive(false);           //deactivate
                menuUp = false;
            }
            else
            {
                menuOverlay.SetActive(true);
                menuUp = true;
            }
        }

        //check for button presses
        if (menuUp)
        {
            //check for exit button press
            if (Input.GetButtonDown("Exit"))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene(0);      //scene 0 is the main menu
            }

            //check for craft button press
            if(Input.GetButtonDown("Craft"))
            {
                craftObject();
            }
        }
    }

    private void LateUpdate()
    {
        if (menuUp) transform.parent.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().getMouseLook().SetCursorLock(false);
        else transform.parent.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().getMouseLook().SetCursorLock(true);
    }

    private void craftObject()
    {
        InteractableScript.objectType componentA = InteractableScript.objectType.None;
        InteractableScript.objectType componentB = InteractableScript.objectType.None;

        for (int i = 0; i < inventory.inventorySize; ++i)           //for each object in the inventory (even empty slots)
        {
            if (inventoryDisplay[i].GetComponent<ButtonSelectScript>().isSelected())         //If the corresponding button is selected - store the object in that slot
            {
                if (componentA == InteractableScript.objectType.None)        //if component A is not stored, store in A
                    componentA = inventory.getObjectAtSlot(i);
                else                                                        //else store in component b
                    componentB = inventory.getObjectAtSlot(i);
            }
        }

        bool craftStatus = inventory.craftObject(componentA, componentB);            //returns whether or not the craft failed/succeeded

        Debug.Log("Crafting attempted");

        //display craft status

    }

    private void displayTimer()
    {
        if (levelTimer <= 0)
            timerDisplay.text = "00:00";
        else
        {
            int minutes = Mathf.FloorToInt(levelTimer / 60);
            int seconds = Mathf.FloorToInt(levelTimer - (minutes * 60));
            timerDisplay.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    private void timeUpCheck()
    {
        if (levelTimer <= 0)     //times up
        {
            if (!gameOver)
            {
                gameOver = true;                    //set to game over

                reticle.SetActive(false);                   //deactivate the reticle
                gameOverPanel.SetActive(true);              //activate the game over screen
                StartCoroutine(fadeGameOverPanel());        //Start the fade in
            }
        }
    }

    IEnumerator fadeGameOverPanel()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            Color panelColour = gameOverPanel.GetComponent<Image>().color;
            panelColour.a = i;
            gameOverPanel.GetComponent<Image>().color = panelColour;

            Color textColour = gameOverPanel.transform.GetChild(0).gameObject.GetComponent<Text>().color;
            textColour.a = i;
            gameOverPanel.transform.GetChild(0).gameObject.GetComponent<Text>().color = textColour;

            yield return null;
        }

        SceneManager.LoadScene(2);      //Load the game over scene once the image has been faded in
    }
}
