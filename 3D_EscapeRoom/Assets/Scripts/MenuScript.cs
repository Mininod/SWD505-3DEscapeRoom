﻿using System.Collections;
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
        reticle = GameObject.Find("Reticle");

        menuOverlay = GameObject.Find("Canvas").transform.GetChild(0).gameObject;       //child 0 of the canvas is the menu/inventory overlay

        levelTimer = timerMax;         //* 60 to turn it from minutes to seconds 

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
        if(gameOver) Debug.Log("Game over");

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
            inventoryDisplay[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = inventory.getObjectAtSlot(i).ToString();    //returns the object at the slot at puts it in the text at the on screen slot
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

        //check for exit button press
        if (menuUp)
        {
            if (Input.GetButtonDown("Exit"))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene(0);      //scene 0 is the main menu
            }
        }
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
                reticle.SetActive(false);           //deactivate the reticle

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
