using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimGuideReferenceHolder : MonoBehaviour
{
    public TutorialScriptableObjects reference;
    public GameObject tutorialPanel;
    public TMPro.TMP_Text buttonTitle;

    public void Select()
    {
        tutorialPanel.SetActive(true);
        TutorialController.instance.SetMapTutorial(reference);
    }
}
