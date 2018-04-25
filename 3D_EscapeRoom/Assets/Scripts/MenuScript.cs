using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject menuOverlay;               //the gameobject of the inventory overlay
    public GameObject gameOverPanel;             //the gameobject for the game over screen   
    public GameObject winScreenPanel;            //the gameobject for the win screen
    public Text timerDisplayMenu;                //the text object that displays the timer in the menu
    private bool menuUp = false;                 //whether or not the menu/inventory is currently being displayed   
    private PlayerInventoryScript inventory;     //direct access to the players inventory
    private GameObject reticle;                  //the gameobject that displays the reticle
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController playerController;      //direct reference to the player controller

    public GameObject notesOverlay;              //the gameobject for displaying the collectable notes
    public Sprite[] notes;                        //an array full of notes/images to display
    private bool notesUp = false;                //wehther or not the notes are currently being displayed

    public int timerMax;                    //max time for the level in minutes in minutes
    private float levelTimer;               //actual time left in the level in seconds
    public int shortTimer;                  //time at which the game indicates you are running out of time (changes music etc) in minutes
    private bool atShortTimer = false;      //whether the game has passes the "shorttimer" moment
    private bool gameOver = false;          //whether time has run out or not

    public GameObject[] inventoryDisplay;           //will be the size of the inventory, set by the designer

    private GameObject musicManager;        //Gameobject to switch betweent the two music tracks when the timer reaches the threshold
    private CollectableLibrary collectableLibrary;      //a reference to the library of gameObjects that can be picked up/collected through crafting
    private SoundManagerScript soundManager;        //reference to the sound manager

    void Start()
    {
        inventory = gameObject.GetComponent<PlayerInventoryScript>();
        musicManager = GameObject.Find("MusicManager");
        collectableLibrary = GameObject.Find("CollectableLibrary").GetComponent<CollectableLibrary>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManagerScript>();

        //reticle
        reticle = GameObject.Find("Reticle");
        if (!SettingsMenuScript.reticleOn) reticle.SetActive(false);            //turn off the reticle if it is off in the settings

        //player controller
        playerController = transform.parent.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();        //since the script is on the child of the player controller
        playerController.setSens();     //set desired sens at the start

        levelTimer = timerMax * 60;         //* 60 to turn it from minutes to seconds 



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

                //switchmusic track
                musicManager.transform.GetChild(0).gameObject.SetActive(false);
                musicManager.transform.GetChild(1).gameObject.SetActive(true);

                timerDisplayMenu.color = Color.red;
                atShortTimer = true;
            }
        }

        //Check for time up
        timeUpCheck();              //runs game over if time is up

        //Inventory display
        for (int i = 0; i < inventory.inventorySize; ++i)
        {
            //returns the object at the slot and puts it in the text at the on screen slot - child 1 is the button, child 0 is the text
            inventoryDisplay[i].transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = inventory.getObjectAtSlot(i).ToString();    
        }

        //check to bring up menu
        if (Input.GetButtonDown("Menu") && !notesUp)    //only check to bringup/close menu if there isnt a note on the screen
        {
            if (menuOverlay.activeSelf)                 //if the menu is active
            {
                clearSelections();                      //clear all crafting selections after closing the menu
                menuOverlay.SetActive(false);           //deactivate
                menuUp = false;
                playerController.enabled = true;        //turn the player controls back on when the menu is gone
            }
            else
            {
                menuOverlay.SetActive(true);
                menuUp = true;
                playerController.enabled = false;       //diable all player controls when in the menu
            }
        }

        //check for button presses
        if (menuUp)
        {
            //check for craft button press
            if(Input.GetButtonDown("Craft"))
            {
                craftObject();
            }

            if(Input.GetButtonDown("Drop"))
            {
                dropObject();
            }
        }

        //return button for if the note is on screen - the menu button (tab currently)
        if(notesUp)
        {
            //check for return button
            if(Input.GetButtonDown("Menu"))
            {
                notesOverlay.SetActive(false);
                notesUp = false;
                playerController.enabled = true;
            }
        }
    }

    private void LateUpdate()           
    {
        if (menuUp) playerController.getMouseLook().SetCursorLock(false);               //when the menu is up the cursor needs to be unlocked to be used
        else playerController.getMouseLook().SetCursorLock(true);
    }

    private void craftObject()
    {
        objectType componentA = objectType.None;
        objectType componentB = objectType.None;

        for (int i = 0; i < inventory.inventorySize; ++i)           //for each object in the inventory (even empty slots)
        {
            if (inventoryDisplay[i].GetComponent<ButtonSelectScript>().isSelected())         //If the corresponding button is selected - store the object in that slot
            {
                if (componentA == objectType.None)        //if component A is not stored, store in A
                    componentA = inventory.getObjectAtSlot(i);
                else                                                        //else store in component b
                    componentB = inventory.getObjectAtSlot(i);
            }
        }

        bool isCrafted = false;

        for (int i = 0; i < inventory.recipeList.Length && !isCrafted; ++i)          //check each recipe as long as an item hasnt been crafted
        {
            if (componentA == inventory.recipeList[i].ComponentA && componentB == inventory.recipeList[i].ComponentB ||
                componentB == inventory.recipeList[i].ComponentA && componentA == inventory.recipeList[i].ComponentB)        //check that the two components being used match the recipe
            {
                //if the components match a recipe, remove the selected objects from the inventory
                for (int j = 0; j < inventory.inventorySize; ++j)
                    if (inventoryDisplay[j].GetComponent<ButtonSelectScript>().isSelected()) inventory.removeFromInventorySlot(j);

                inventory.addToInventory(inventory.recipeList[i].Result);           //add the item made to the invetory
                soundManager.PlaySFX("Craft");  //play sound if successful craft
                isCrafted = true;
            }
        }
                
        Debug.Log("Crafting attempted");

        clearSelections();          //clear all the selected objects
        //display craft status

    }

    private void dropObject()
    {
        for (int i = 0; i < inventory.inventorySize; ++i)       //for each slot in the inventory
        {
            if(inventoryDisplay[i].GetComponent<ButtonSelectScript>().isSelected())         //if the slot is selected
            {
                if(inventory.getObjectAtSlot(i) != objectType.None)         //if the object is not nothing
                {
                    GameObject toDrop = collectableLibrary.getGameObject(inventory.getObjectAtSlot(i));         //gets the gameObject from the object type at the selected slot
                    Vector3 dropPos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z) + transform.forward;    //drop pos
                    GameObject drop = Instantiate(toDrop, dropPos, Quaternion.identity);             //Needs a translation on the drop point
                    inventory.removeFromInventorySlot(i);
                    clearSelections();
                }
            }
        }

        Debug.Log("Item dropped");
    }

    private void clearSelections()
    {
        foreach (var button in inventoryDisplay)
            button.GetComponent<ButtonSelectScript>().deselect();
    }

    private void displayTimer()
    {
        if (levelTimer <= 0)
        {
            timerDisplayMenu.text = "00:00";
        }
        else        //updates both timer displays
        {
            int minutes = Mathf.FloorToInt(levelTimer / 60);
            int seconds = Mathf.FloorToInt(levelTimer - (minutes * 60));
            timerDisplayMenu.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    public float getTimeRemaining()
    {
        return levelTimer;
    }

    public bool isTimerBelowThreshold()
    {
        return atShortTimer;
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

    public void triggerWin()
    {
        winScreenPanel.SetActive(true);     //activate the win screen
        StartCoroutine(fadeWinScreen());    //start the fade in
    }

    public void displayNote(int number)
    {
        if (number <= notes.Length - 1)          //if the note number specified is in the array
        {
            notesOverlay.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = notes[number];             //assigns the desired note to the canvas
            notesOverlay.SetActive(true);
            notesUp = true;         //tells the script that a note is currently being displayed
            playerController.enabled = false;
        }
        else Debug.Log("No Note");
    }

    public void returnToMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);      //scene 0 is the main menu
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

    IEnumerator fadeWinScreen()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            Color panelColour = winScreenPanel.GetComponent<Image>().color;
            panelColour.a = i;
            winScreenPanel.GetComponent<Image>().color = panelColour;

            yield return null;
        }

        SceneManager.LoadScene(3);      //Load the win scene once the image has been faded in
    }
}
