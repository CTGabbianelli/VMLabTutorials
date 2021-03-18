using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialController : MonoBehaviour
{
    public delegate void TutorialIndexChanged();
    public static event TutorialIndexChanged tutorialIndexChanged;

    public delegate void MapOpened();
    public static event MapOpened mapOpened;

    public static TutorialController instance;

    public string playerPrefsString;

    public int index;

    public GameObject tutorialBG;
    public GameObject baseParent;
    public GameObject mapParent;

    public GameObject mapDotPrefab;
    public bool isOpen;
    public TutorialMarkerIcon tutorialMarkerIcon;
    public List<TutorialObjectsList> tutorialSets;
    enum Tutorial
    {
        Replay,
        SimGuide
    }
    Tutorial tutorial;
    [SerializeField]
    List<TutorialScriptableObjects> baseTutorialPresets;
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
    public RectTransform panelBGTransform;
    public RectTransform arrowTransform;
    public TMP_Text titleText;
    public TMP_Text informationText;
    [Header("Map Tutorial Components")]
    public RectTransform mapMaskTransform;
    public RectTransform mapPanelBGTransform;
    public RectTransform mapArrowTransform;
    public TMP_Text mapTitleText;
    public TMP_Text mapInformationText;

    // Start is called before the first frame update
    private void Awake()
    {
        //set global reference
        instance = this;
    }
    public void Initialize()
    {
        AddFirstTutorialSet();

        if (mapTutorials.Count != 0)
        {
            CreateMap();
        }
        if (PlayerPrefs.GetString(playerPrefsString) != "Completed")
        {
            StartTutorial();
        }
        else
        {

        }


    }
    void AddFirstTutorialSet()
    {
        AddTutorials(0);
    }
    public void AddTutorials(int index)
    {
        if(tutorialSets.Count > 0)
        {
            foreach (TutorialScriptableObjects o in tutorialSets[index].tutorialSets)
            {
                baseTutorialPresets.Add(o);
            }
            tutorialMarkerIcon.Initialize(baseTutorialPresets.Count);
        }


    }

    void CreateMap()
    {
        GameObject tempMapDot;
        foreach (TutorialMapScriptableObject mapTutorial in mapTutorials)
        {
            tempMapDot = Instantiate(mapDotPrefab, mapButtonParent.transform);

            tempMapDot.GetComponent<RectTransform>().anchoredPosition = mapTutorial.mapButtonPosition;
            tempMapDot.GetComponent<SimGuideReferenceHolder>().reference = mapTutorial;
            tempMapDot.GetComponent<SimGuideReferenceHolder>().tutorialPanel = mapPanelBGTransform.gameObject;
            tempMapDot.GetComponent<SimGuideReferenceHolder>().buttonTitle.text = mapTutorial.buttonTitle;

            mapButtons.Add(tempMapDot);

        }
    }


    //advance through tutorial sequence
    public void AdvanceToNextTutorial()
    {
        if (index < baseTutorialPresets.Count - 1)
        {

            index++;
            SetTutorial(baseTutorialPresets[index]);
            tutorialMarkerIcon.SetMarkerColor(index);
        }
        else
        {
            SetTutorialActivity(false);
            StartCoroutine(DisableBaseElements());

            tutorialMarkerIcon.DeactivateMarkers();

            PlayerPrefs.SetString(playerPrefsString, "Completed");


            if (PlayerPrefs.GetString(playerPrefsString) != "Completed")
            {
                print("not completed");
            }
        }
        tutorialIndexChanged();


    }
    //skip tutorial by setting index to length of list
    public void SkipTutorial()
    {
        index = baseTutorialPresets.Count;
        AdvanceToNextTutorial();

    }
    public void SetMapTutorial(TutorialScriptableObjects tut)
    {
        SetMapMask(tut);
        SetMapPanel(tut);
        SetMapArrow(tut);
        SetMapTitle(tut);
        SetMapInformation(tut);
        mapTutorialPanelAnimator.SetBool("Active", true);
        StartCoroutine(SetMapButtonActivity(false));

    }

    public void StartTutorial()
    {
        if(baseTutorialPresets.Count > 0)
        {
            index = 0;
            SetTutorial(baseTutorialPresets[index]);
            tutorialMarkerIcon.SetMarkerColor(index);
            SetTutorialActivity(true);
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

            StartCoroutine(SetMapButtonActivity(false));
            mapMaskAnimator.SetBool("Active", false);
            StartCoroutine(DisableMapElements());

        }
        else
        {
            mapMaskAnimator.SetBool("Active", true);
            mapMaskTransform.sizeDelta = Vector2.zero;
            tutorialBG.SetActive(false);
            StartCoroutine(SetMapButtonActivity(true));
        }
        InformationButton.instance.Click();
    }
    public void BringBackButtons()
    {
        mapMaskAnimator.SetBool("Active", true);
        mapMaskTransform.sizeDelta = Vector2.zero;
        StartCoroutine(SetMapButtonActivity(true));
        mapTutorialPanelAnimator.SetBool("Active", false);
    }
    //activate or deactive map buttons
    IEnumerator SetMapButtonActivity(bool isActive)
    {
        //set button active if true
        if (isActive == true)
        {
            mapOpened();

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
            mapButton.GetComponent<Button>().interactable = true;

            yield return wait;
            buttonIndex++;

        }
        if (isActive == false)
        {
            mapExitButton.SetBool("Active", false);
            yield return new WaitForSeconds(1f);
            foreach (GameObject mapButton in mapButtons)
            {
                mapButton.GetComponent<Button>().interactable = false;
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
        if (PlayerPrefs.GetString(playerPrefsString) == "Completed")
        {
            InformationButton.instance.Click();
        }



        tutorialMarkerIcon.ActivateMarkers();
        replayTutorialAnimator.SetBool("Active", isActive);
    }

    void SetTutorial(TutorialScriptableObjects tut)
    {
        SetMask(tut);
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
    void SetMask(TutorialScriptableObjects tut)
    {
        maskTransform.anchoredPosition = tut.maskPosition;
        maskTransform.sizeDelta = tut.maskWidthAndHeight;
        maskTransform.anchorMin = tut.maskAnchorMin;
        maskTransform.anchorMax = tut.maskAnchorMax;
        maskTransform.pivot = tut.maskPivot;
    }
    void SetPanel(TutorialScriptableObjects tut)
    {
        panelBGTransform.anchoredPosition = tut.panelPosition;
        panelBGTransform.sizeDelta = tut.panelWidthAndHeight;
        panelBGTransform.anchorMin = tut.panelAnchorMin;
        panelBGTransform.anchorMax = tut.panelAnchorMax;
        panelBGTransform.pivot = tut.panelPivot;
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
    void SetMapPanel(TutorialScriptableObjects tut)
    {
        mapPanelBGTransform.anchoredPosition = tut.panelPosition;
        mapPanelBGTransform.sizeDelta = tut.panelWidthAndHeight;
        mapPanelBGTransform.anchorMin = tut.panelAnchorMin;
        mapPanelBGTransform.anchorMax = tut.panelAnchorMax;
        mapPanelBGTransform.pivot = tut.panelPivot;
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
