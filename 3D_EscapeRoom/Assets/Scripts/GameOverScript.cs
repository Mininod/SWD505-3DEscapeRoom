using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    private GameObject gameOverPanel;

    void Start()
    {
        gameOverPanel = GameObject.Find("Canvas").transform.GetChild(0).gameObject;     //child 0 of the canvas is the game over panel

        //Set game over screen elements to 0 alpha - I could do this in the editor, but then they are hard to edit
        Color panelColor0 = gameOverPanel.GetComponent<Image>().color;
        panelColor0.a = 0;
        gameOverPanel.GetComponent<Image>().color = panelColor0;

        Color textColor0 = gameOverPanel.transform.GetChild(0).gameObject.GetComponent<Text>().color;
        textColor0.a = 0;
        gameOverPanel.transform.GetChild(0).gameObject.GetComponent<Text>().color = textColor0;

        //Start the fade in
        StartCoroutine(fadeGameOverPanel());
    }

    void Update()
    {

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
    }
}
