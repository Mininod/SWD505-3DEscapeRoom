using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    public GameObject fullscreenButton;
    public GameObject qualityButton;
    public GameObject reticleButton;
    public GameObject sensSlider;

    static public bool reticleOn = true;
    static public int sensitivity = 2;          //deafult sens is 2

    void Start()
    {
        fullscreenButton.GetComponent<Toggle>().isOn = Screen.fullScreen;
        if (QualitySettings.GetQualityLevel() == 0) qualityButton.GetComponent<Toggle>().isOn = true;
        else qualityButton.GetComponent<Toggle>().isOn = false;
        reticleButton.GetComponent<Toggle>().isOn = reticleOn;          //if the reticle is on, the toggle is on
        sensSlider.GetComponent<Slider>().value = sensitivity;          //set the displayed sens to the actual current sens
    }

    //Button functions
    public void setFullscreen(bool toggle)
    {
        Screen.fullScreen = toggle;
    }

    public void setLowQuality(bool toggle)
    {
        if (toggle) QualitySettings.SetQualityLevel(0);
        else QualitySettings.SetQualityLevel(5);
    }

    public void showReticle(bool toggle)
    {
        if (toggle) reticleOn = true;
        else reticleOn = false;
    }

    public void adjustSens(float slider)
    {
        sensitivity = Mathf.RoundToInt(slider);
    }

    public void returnToMenu()
    {
        gameObject.SetActive(false);
    }
    //End
}