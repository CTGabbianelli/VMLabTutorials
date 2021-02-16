using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationButton : MonoBehaviour
{
    public Image image;
    public GameObject tutorialButtons;
    public Sprite infoSprite;
    public Sprite closeSprite;

    public Color defaultColor = new Color(1f, 1f, 1f, 1f);
    public Color hoveredColor;

    public GameObject menu;
    public GameObject deviceInfoPanel;
    public bool isOpen;
    public TutorialController tutorial;
    public bool secondaryUsed;

    public static InformationButton instance;

    private void Awake()
    {
        instance = this;
    }
    public void Click()
    {

        if (isOpen == false)
        {
            image.sprite = closeSprite;
            OpenMenu();
        }
        else
        {
            image.sprite = infoSprite;
            CloseMenu();
        }
    }

    public void BeginHover()
    {
        image.color = hoveredColor;
    }

    public void EndHover()
    {
        image.color = defaultColor;
    }

    private void OpenMenu()
    {
        isOpen = true;
        TutorialController.instance.isOpen = true;
        menu.SetActive(true);
        tutorialButtons.SetActive(true);
        deviceInfoPanel.SetActive(false);
    }

    private void CloseMenu()
    {
        isOpen = false;
        TutorialController.instance.isOpen = false;
        menu.SetActive(false);
        tutorialButtons.SetActive(false);
        deviceInfoPanel.SetActive(false);
    }

    public void SelectTutorial()
    {
        tutorial.ReplayTutorial();
        Click();
    }

    public void SelectDeviceInformation()
    {
        menu.SetActive(false);
        tutorialButtons.SetActive(false);
        deviceInfoPanel.SetActive(true);
    }

    public void CloseDeviceInformation()
    {
        deviceInfoPanel.SetActive(false);
        tutorialButtons.SetActive(false);
        isOpen = false;
        image.sprite = infoSprite;
    }

}