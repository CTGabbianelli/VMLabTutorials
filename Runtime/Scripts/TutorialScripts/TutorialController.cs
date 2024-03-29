﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialController : MonoBehaviour
{

    public delegate void TutorialIndexChanged();
    public static event TutorialIndexChanged tutorialIndexChanged;

    public delegate void TutorialCompletedFirstTime();
    public static event TutorialCompletedFirstTime tutorialCompletedFirstTime;

    public delegate void MapOpened();
    public static event MapOpened mapOpened;

    public delegate void TutorialReplayed();
    public static event TutorialReplayed tutorialReplayed;

    public delegate void MapButtonSelected();
    public static event MapButtonSelected mapButtonSelected;

    public static TutorialController instance;

    public TutorialScriptableObjects currentTutorial;

    public string playerPrefsString;

    public int index;
    public int skippedIndex;
    bool skipped;
    private int addedTutorialsIndex;

    public GameObject tutorialBG;
    public GameObject baseParent;
    public GameObject mapParent;

    public GameObject mapDotPrefab;
    public bool isOpen;
    bool mapElementIsOpen;
    public TutorialMarkerIcon tutorialMarkerIcon;
    public List<TutorialObjectsList> tutorialSets;
    enum Tutorial
    {
        Sequence,
        Map,
        Inactive
    }
    Tutorial tutorial;
    [SerializeField]
    public List<TutorialScriptableObjects> baseTutorialPresets;
    [SerializeField]
    List<TutorialMapScriptableObject> mapTutorials;
    public List<GameObject> mapButtons;

    public GameObject mapButtonParent;
    public float mapButtonDelay;
    [Header("Animators")]
    public Animator replayTutorialAnimator;
    public Animator mapExitButton;
    public Animator mapTutorialPanelAnimator;
    public Animator mapMaskAnimator;

    [Header("Base Tutorial Components")]
    public RectTransform maskTransform;
    public RectTransform secondaryMaskTransform;
    public RectTransform panelBGTransform;
    public RectTransform arrowTransform;
    public TMP_Text titleText;
    public TMP_Text informationText;
    [Header("Map Tutorial Components")]
    public RectTransform mapMaskTransform;
    public RectTransform mapSecondaryMaskTransform;
    public RectTransform mapPanelBGTransform;
    public RectTransform mapArrowTransform;
    public TMP_Text mapTitleText;
    public TMP_Text mapInformationText;

    // Start is called before the first frame update
    private void Awake()
    {
        addedTutorialsIndex = 0;
        //set global reference
        instance = this;
    }
    public void Initialize()
    {
        //check tutorial set to be used based on url 
        CheckTutorialUrl();
        
        //if the map has buttons create them
        if (mapTutorials.Count != 0)
        {
            CreateMap();
        }

        //if user has already finished the tutorial don't automatically start
        if (PlayerPrefs.GetString(playerPrefsString + addedTutorialsIndex.ToString()) != "Completed")
        {
            StartTutorial();

        }
        else
        {

        }


    }

    //check which tutorial to use
    void CheckTutorialUrl()
    {
        // Read any parameters included in URL
        Dictionary<string, string> searchParameters = URLParameters.GetSearchParameters();
        //https://some.domain.com/index.html?parameterName=parameterValue
        if (searchParameters.ContainsKey("tutorialMode"))
        {
            string tutorialMode = searchParameters["tutorialMode"];
            if (tutorialMode == "SequenceTutorial")
            {
                AddTutorials(0);
            }
            else if (tutorialMode == "FullTutorial")
            {
                AddTutorials(1);

            }
            else if (tutorialMode == "PICTutorial")
            {
                AddTutorials(2);
            }
            else
            {
                AddTutorials(1);
            }
        }
        else
        {
            AddTutorials(1);
        }
    }
    //add specified tutorial
    public void AddTutorials(int index)
    {
        if (tutorialSets.Count > 0)
        {
            foreach (TutorialScriptableObjects o in tutorialSets[index].tutorialSets)
            {
                baseTutorialPresets.Add(o);
            }
            tutorialMarkerIcon.Initialize(baseTutorialPresets.Count);
        }
        addedTutorialsIndex++;
        skipped = false;


    }

    //create all tutorial map buttons
    void CreateMap()
    {
        GameObject tempMapDot;
        foreach (TutorialMapScriptableObject mapTutorial in mapTutorials)
        {
            tempMapDot = Instantiate(mapDotPrefab, mapButtonParent.transform);

            tempMapDot.GetComponent<RectTransform>().anchoredPosition = mapTutorial.mapButtonPosition;
            tempMapDot.GetComponent<RectTransform>().sizeDelta = mapTutorial.mapButtonWidthAndHeight;
            tempMapDot.GetComponent<SimGuideReferenceHolder>().reference = mapTutorial;
            tempMapDot.GetComponent<SimGuideReferenceHolder>().tutorialPanel = mapPanelBGTransform.gameObject;
            tempMapDot.GetComponent<SimGuideReferenceHolder>().buttonTitle.text = mapTutorial.buttonTitle;

            mapButtons.Add(tempMapDot);

        }
    }


    //advance through tutorial sequence
    public void AdvanceToNextTutorial()
    {
        index++;
        secondaryMaskTransform.gameObject.SetActive(false);
        if (index < baseTutorialPresets.Count)
        {

            SetTutorial(baseTutorialPresets[index]);
            tutorialMarkerIcon.SetMarkerColor(index);

            tutorial = Tutorial.Sequence;
        }
        else
        {
            SetTutorialActivity(false);
            StartCoroutine(DisableBaseElements());

            tutorialMarkerIcon.DeactivateMarkers();



            if (PlayerPrefs.GetString(playerPrefsString + addedTutorialsIndex.ToString()) != "Completed" && skipped == false)
            {
                PlayerPrefs.SetString(playerPrefsString + addedTutorialsIndex.ToString(), "Completed");
            }

            tutorial = Tutorial.Inactive;
        }
        tutorialIndexChanged();





    }
    //skip tutorial by setting index to length of list
    public void SkipTutorial()
    {
        skipped = true;
        skippedIndex = index;
        index = baseTutorialPresets.Count - 1;
        AdvanceToNextTutorial();

    }
    //show the activated map tutorial
    public void SetMapTutorial(TutorialScriptableObjects tut)
    {

        currentTutorial = tut;
        SetMapMask(tut);
        if (tut.usesSecondaryMask == true)
        {
            SetSecondaryMapMask(tut);
        }
        SetMapPanel(tut);
        SetMapArrow(tut);
        SetMapTitle(tut);
        SetMapInformation(tut);
        mapTutorialPanelAnimator.SetBool("Active", true);

        mapButtonSelected();

        StartCoroutine(SetMapButtonLayoutActivity(false));

    }

    //get name of a specific button
    public string GetNameOfCurrentButton()
    {
        if (currentTutorial.GetType() == typeof(TutorialMapScriptableObject))
        {

            return (currentTutorial as TutorialMapScriptableObject).buttonTitle;
        }
        else
        {
            return "Error : not a button";
        }
    }

    //start the tutorial
    public void StartTutorial()
    {
        if (baseTutorialPresets.Count > 0)
        {
            index = 0;
            SetTutorial(baseTutorialPresets[index]);
            tutorialMarkerIcon.SetMarkerColor(index);
            SetTutorialActivity(true);
            //InformationButton.instance.Click();
            tutorialIndexChanged();
        }

    }

    //Decrement index and move tutorial back
    public void BackTutorial()
    {
        if (index > 0)
        {
            index--;
            SetTutorial(baseTutorialPresets[index]);
            tutorialMarkerIcon.SetMarkerColor(index);

            tutorialIndexChanged();
        }

    }
    //get tutorials completion state
    public bool CheckTutorialStateCompletion()
    {
        if (PlayerPrefs.GetString(playerPrefsString + addedTutorialsIndex.ToString()) != "Completed")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //go to a specified tutorial in the sequence
    public void JumpToSpecificTutorial(int tempIndex)
    {
        StartTutorial();
        index = tempIndex;
        SetTutorial(baseTutorialPresets[index]);
        tutorialMarkerIcon.SetMarkerColor(index);

        tutorialIndexChanged();
    }
    //set index to 0 and reset tutorial
    public void ReplayTutorial()
    {
        tutorialReplayed();
        InformationButton.instance.Click();
        StartTutorial();
    }
    //turn off map tutorial
    public void DeactivateMap()
    {
        ActivateMap();
    }

    //turn on tutorial map
    public void ActivateMap()
    {


        //enable map object and disable info panel object
        mapParent.SetActive(true);

        //start each map button timers to turn on

        //set map tutorial fade in animation
        if (isOpen != true)
        {
            tutorial = Tutorial.Inactive;
            StartCoroutine(SetMapButtonLayoutActivity(false));
            mapMaskAnimator.SetBool("Active", false);
            StartCoroutine(DisableMapElements());

        }
        else
        {
            tutorial = Tutorial.Map;
            mapMaskAnimator.SetBool("Active", true);
            mapMaskTransform.sizeDelta = Vector2.zero;
            tutorialBG.SetActive(false);
            StartCoroutine(SetMapButtonLayoutActivity(true));
            InformationButton.instance.Click();
        }

    }
    public void BringBackButtons()
    {
        mapMaskAnimator.SetBool("Active", true);
        mapMaskTransform.sizeDelta = Vector2.zero;
        StartCoroutine(SetMapButtonLayoutActivity(true));
        mapTutorialPanelAnimator.SetBool("Active", false);
    }
    //activate or deactive map buttons
    IEnumerator SetMapButtonLayoutActivity(bool isActive)
    {



        //set button active if true
        if (isActive == true)
        {
            mapSecondaryMaskTransform.gameObject.SetActive(false);

            mapElementIsOpen = false;
            //mapOpened();

            mapButtonParent.SetActive(true);
            mapExitButton.SetBool("Active", true);
            yield return new WaitForSeconds(.5f);
            mapPanelBGTransform.gameObject.SetActive(false);
        }
        int buttonIndex = 0;

        //create timer based on set delay time
        WaitForSeconds wait = new WaitForSeconds(mapButtonDelay);

        //set each buttons animator activity
        foreach (GameObject mapButton in mapButtons)
        {
            mapButtons[buttonIndex].GetComponent<Animator>().SetBool("Active", isActive);
            mapButton.GetComponent<Image>().raycastTarget = true;
            mapButton.GetComponent<Button>().interactable = true;

            yield return wait;
            buttonIndex++;

        }
        if (isActive == false)
        {
            mapElementIsOpen = true;

            mapExitButton.SetBool("Active", false);
            yield return new WaitForSeconds(1f);
            foreach (GameObject mapButton in mapButtons)
            {
                mapButton.GetComponent<Button>().interactable = false;
                mapButton.GetComponent<Image>().raycastTarget = false;
            }
        }
    }
    //Set Base tutorial activity
    public void SetTutorialActivity(bool isActive)
    {
        if (isActive == true)
        {
            baseParent.SetActive(true);
        }
        else
        {

        }




        tutorialMarkerIcon.ActivateMarkers();
        replayTutorialAnimator.SetBool("Active", isActive);
    }

    void SetTutorial(TutorialScriptableObjects tut)
    {
        secondaryMaskTransform.gameObject.SetActive(false);
        currentTutorial = tut;
        SetMask(tut);
        if (currentTutorial.usesSecondaryMask == true)
        {
            SetSecondaryMask(tut);
        }
        SetPanel(tut);
        SetArrow(tut);
        SetTitle(tut);
        SetInformation(tut);
    }

    //disable map tutorial
    IEnumerator DisableMapElements()
    {
        yield return new WaitForSeconds(1f);
        mapParent.SetActive(false);

    }
    //disable base tutorial
    IEnumerator DisableBaseElements()
    {
        yield return new WaitForSeconds(1f);
        baseParent.SetActive(false);

    }

    //dictate which mask should be active and used
    public void SetActiveMask(bool useAltMask)
    {

        if (currentTutorial != null)
        {
            if (currentTutorial.usesAltMask)
            {
                if (tutorial == Tutorial.Sequence)
                {
                    if (useAltMask == true)
                    {
                        SetAltMask(currentTutorial);
                        SetAltPanel(currentTutorial);
                        SetAltArrow(currentTutorial);
                    }
                    else
                    {
                        SetMask(currentTutorial);
                        SetPanel(currentTutorial);
                        SetArrow(currentTutorial);
                    }
                }
                else if (tutorial == Tutorial.Map)
                {
                    if (mapElementIsOpen)
                    {
                        if (useAltMask == true)
                        {
                            SetAltMapMask(currentTutorial);
                            SetAltMapPanel(currentTutorial);
                            SetAltMapArrow(currentTutorial);
                        }
                        else
                        {
                            SetMapMask(currentTutorial);
                            SetMapPanel(currentTutorial);
                            SetMapArrow(currentTutorial);
                        }
                    }

                }
            }

        }


    }

    /// <summary>
    /// Set all tutorial ui values
    /// </summary>
    /// <param name="tut"></param>
    void SetMask(TutorialScriptableObjects tut)
    {
        maskTransform.anchoredPosition = tut.maskPosition;
        maskTransform.sizeDelta = tut.maskWidthAndHeight;
        maskTransform.anchorMin = tut.maskAnchorMin;
        maskTransform.anchorMax = tut.maskAnchorMax;
        maskTransform.pivot = tut.maskPivot;
    }
    void SetSecondaryMask(TutorialScriptableObjects tut)
    {
        secondaryMaskTransform.gameObject.SetActive(true);
        secondaryMaskTransform.anchoredPosition = tut.secondaryMaskPosition;
        secondaryMaskTransform.sizeDelta = tut.secondaryMaskWidthAndHeight;
        secondaryMaskTransform.anchorMin = tut.secondaryMaskAnchorMin;
        secondaryMaskTransform.anchorMax = tut.secondaryMaskAnchorMax;
        secondaryMaskTransform.pivot = tut.secondaryMaskPivot;
    }
    void SetAltMask(TutorialScriptableObjects tut)
    {
        maskTransform.anchoredPosition = tut.maskAltPosition;
        maskTransform.sizeDelta = tut.maskAltWidthAndHeight;
        maskTransform.anchorMin = tut.maskAltAnchorMin;
        maskTransform.anchorMax = tut.maskAltAnchorMax;
        maskTransform.pivot = tut.maskAltPivot;
    }
    void SetPanel(TutorialScriptableObjects tut)
    {
        panelBGTransform.anchoredPosition = tut.panelPosition;
        panelBGTransform.sizeDelta = tut.panelWidthAndHeight;
        panelBGTransform.anchorMin = tut.panelAnchorMin;
        panelBGTransform.anchorMax = tut.panelAnchorMax;
        panelBGTransform.pivot = tut.panelPivot;
    }
    void SetAltPanel(TutorialScriptableObjects tut)
    {
        panelBGTransform.anchoredPosition = tut.panelAltPosition;
        panelBGTransform.sizeDelta = tut.panelAltWidthAndHeight;
        panelBGTransform.anchorMin = tut.panelAltAnchorMin;
        panelBGTransform.anchorMax = tut.panelAltAnchorMax;
        panelBGTransform.pivot = tut.panelAltPivot;
    }
    void SetArrow(TutorialScriptableObjects tut)
    {
        arrowTransform.anchoredPosition = tut.trianglePosition;
        arrowTransform.sizeDelta = tut.triangleWidthAndHeight;
        arrowTransform.anchorMin = tut.triangleAnchorMin;
        arrowTransform.anchorMax = tut.triangleAnchorMax;
        arrowTransform.pivot = tut.trianglePivot;
        arrowTransform.localEulerAngles = tut.triangleRotation;
    }
    void SetAltArrow(TutorialScriptableObjects tut)
    {
        arrowTransform.anchoredPosition = tut.triangleAltPosition;
        arrowTransform.sizeDelta = tut.triangleAltWidthAndHeight;
        arrowTransform.anchorMin = tut.triangleAltAnchorMin;
        arrowTransform.anchorMax = tut.triangleAltAnchorMax;
        arrowTransform.pivot = tut.triangleAltPivot;
        arrowTransform.localEulerAngles = tut.triangleAltRotation;
    }
    void SetTitle(TutorialScriptableObjects tut)
    {
        titleText.rectTransform.anchoredPosition = tut.titleTextPosition;
        titleText.rectTransform.sizeDelta = tut.titleTextWidthAndHeight;
        titleText.rectTransform.anchorMin = tut.titleTextAnchorMin;
        titleText.rectTransform.anchorMax = tut.titleTextAnchorMax;
        titleText.rectTransform.pivot = tut.titleTextPivot;
        titleText.text = tut.titleText;
        titleText.fontSize = tut.titleFontSize;
    }
    void SetInformation(TutorialScriptableObjects tut)
    {
        informationText.rectTransform.anchoredPosition = tut.informationTextPosition;
        informationText.rectTransform.sizeDelta = tut.informationTextWidthAndHeight;
        informationText.rectTransform.anchorMin = tut.informationTextAnchorMin;
        informationText.rectTransform.anchorMax = tut.informationTextAnchorMax;
        informationText.rectTransform.pivot = tut.informationTextPivot;
        informationText.text = tut.informationText;
        informationText.fontSize = tut.informationFontSize;
    }
    void SetMapButton(TutorialMapScriptableObject tutButton, RectTransform mapButtonPrefab)
    {
        mapButtonPrefab.anchoredPosition = tutButton.mapButtonPosition;
    }
    void SetMapMask(TutorialScriptableObjects tut)
    {
        mapMaskTransform.anchoredPosition = tut.maskPosition;
        mapMaskTransform.sizeDelta = tut.maskWidthAndHeight;
        mapMaskTransform.anchorMin = tut.maskAnchorMin;
        mapMaskTransform.anchorMax = tut.maskAnchorMax;
        mapMaskTransform.pivot = tut.maskPivot;
    }
    void SetSecondaryMapMask(TutorialScriptableObjects tut)
    {
        mapSecondaryMaskTransform.gameObject.SetActive(true);
        mapSecondaryMaskTransform.anchoredPosition = tut.secondaryMaskPosition;
        mapSecondaryMaskTransform.sizeDelta = tut.secondaryMaskWidthAndHeight;
        mapSecondaryMaskTransform.anchorMin = tut.secondaryMaskAnchorMin;
        mapSecondaryMaskTransform.anchorMax = tut.secondaryMaskAnchorMax;
        mapSecondaryMaskTransform.pivot = tut.secondaryMaskPivot;
    }
    void SetAltMapMask(TutorialScriptableObjects tut)
    {
        mapMaskTransform.anchoredPosition = tut.maskAltPosition;
        mapMaskTransform.sizeDelta = tut.maskAltWidthAndHeight;
        mapMaskTransform.anchorMin = tut.maskAltAnchorMin;
        mapMaskTransform.anchorMax = tut.maskAltAnchorMax;
        mapMaskTransform.pivot = tut.maskAltPivot;
    }
    void SetMapPanel(TutorialScriptableObjects tut)
    {
        mapPanelBGTransform.anchoredPosition = tut.panelPosition;
        mapPanelBGTransform.sizeDelta = tut.panelWidthAndHeight;
        mapPanelBGTransform.anchorMin = tut.panelAnchorMin;
        mapPanelBGTransform.anchorMax = tut.panelAnchorMax;
        mapPanelBGTransform.pivot = tut.panelPivot;
    }
    void SetAltMapPanel(TutorialScriptableObjects tut)
    {
        mapPanelBGTransform.anchoredPosition = tut.panelAltPosition;
        mapPanelBGTransform.sizeDelta = tut.panelAltWidthAndHeight;
        mapPanelBGTransform.anchorMin = tut.panelAltAnchorMin;
        mapPanelBGTransform.anchorMax = tut.panelAltAnchorMax;
        mapPanelBGTransform.pivot = tut.panelAltPivot;
    }
    void SetMapArrow(TutorialScriptableObjects tut)
    {
        mapArrowTransform.anchoredPosition = tut.trianglePosition;
        mapArrowTransform.sizeDelta = tut.triangleWidthAndHeight;
        mapArrowTransform.anchorMin = tut.triangleAnchorMin;
        mapArrowTransform.anchorMax = tut.triangleAnchorMax;
        mapArrowTransform.pivot = tut.trianglePivot;
        mapArrowTransform.localEulerAngles = tut.triangleRotation;
    }
    void SetAltMapArrow(TutorialScriptableObjects tut)
    {
        mapArrowTransform.anchoredPosition = tut.triangleAltPosition;
        mapArrowTransform.sizeDelta = tut.triangleAltWidthAndHeight;
        mapArrowTransform.anchorMin = tut.triangleAltAnchorMin;
        mapArrowTransform.anchorMax = tut.triangleAltAnchorMax;
        mapArrowTransform.pivot = tut.triangleAltPivot;
        mapArrowTransform.localEulerAngles = tut.triangleAltRotation;
    }
    void SetMapTitle(TutorialScriptableObjects tut)
    {
        mapTitleText.rectTransform.anchoredPosition = tut.titleTextPosition;
        mapTitleText.rectTransform.sizeDelta = tut.titleTextWidthAndHeight;
        mapTitleText.rectTransform.anchorMin = tut.titleTextAnchorMin;
        mapTitleText.rectTransform.anchorMax = tut.titleTextAnchorMax;
        mapTitleText.rectTransform.pivot = tut.titleTextPivot;
        mapTitleText.text = tut.titleText;
        mapTitleText.fontSize = tut.titleFontSize;
    }
    void SetMapInformation(TutorialScriptableObjects tut)
    {
        mapInformationText.rectTransform.anchoredPosition = tut.informationTextPosition;
        mapInformationText.rectTransform.sizeDelta = tut.informationTextWidthAndHeight;
        mapInformationText.rectTransform.anchorMin = tut.informationTextAnchorMin;
        mapInformationText.rectTransform.anchorMax = tut.informationTextAnchorMax;
        mapInformationText.rectTransform.pivot = tut.informationTextPivot;
        mapInformationText.text = tut.informationText;
        mapInformationText.fontSize = tut.informationFontSize;
    }
}
