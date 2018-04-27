using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GameObject settingsMenuPanel;
    public GameObject creditsPanel;

    private void Start()
    {
        //This ensures when loading the menu from the game, that the cursor returns
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //Button functions
    public void startGame()
    {
        //currently loads scene 1, the only other scene
        SceneManager.LoadScene(1);
    }

    public void settings()
    {
        settingsMenuPanel.SetActive(true);
    }

    public void credits()
    {
        creditsPanel.SetActive(true);
    }

    public void leaveCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void quitGame()
    {
        Application.Quit();
    }
    //End
}
