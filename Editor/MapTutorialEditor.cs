using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

public class MapTutorialEditor : EditorWindow
{
    string informationString;
    string titleString;
    string buttonTitleString;
    TutorialMapScriptableObject tutorialMapObject;
    RectTransform tutorialMapButtonTransform;
    RectTransform maskTransform;
    RectTransform panelTransform;
    RectTransform triangleTransform;
    [SerializeField]
    TMP_Text informationText;
    [SerializeField]
    TMP_Text titleText;
    [SerializeField]
    TMP_Text buttonTitleText;
    [MenuItem("Tools/MapTutorialEditor")]

    public static void ShowMapWindow()
    {
        GetWindow(typeof(MapTutorialEditor));
    }
    private void OnGUI()
    {
        GUILayout.Label("Save Tutorial Preset", EditorStyles.boldLabel);
        tutorialMapObject = EditorGUILayout.ObjectField("Tutorial Scriptable Object", tutorialMapObject, typeof(TutorialMapScriptableObject), false) as TutorialMapScriptableObject;
        tutorialMapButtonTransform = EditorGUILayout.ObjectField("Map Button Rect Transform", tutorialMapButtonTransform, typeof(RectTransform), true) as RectTransform;
        maskTransform = EditorGUILayout.ObjectField("Mask Rect Transform", maskTransform, typeof(RectTransform), true) as RectTransform;
        panelTransform = EditorGUILayout.ObjectField("Panel Rect Transform", panelTransform, typeof(RectTransform), true) as RectTransform;
        triangleTransform = EditorGUILayout.ObjectField("Triangle Rect Transform", triangleTransform, typeof(RectTransform), true) as RectTransform;
        titleString = EditorGUILayout.TextField("Title", titleString);
        titleText = EditorGUILayout.ObjectField("Title Text", titleText, typeof(TMP_Text), true) as TMP_Text;
        informationString = EditorGUILayout.TextField("Information", informationString);
        informationText = EditorGUILayout.ObjectField("Information Text", informationText, typeof(TMP_Text), true) as TMP_Text;
        buttonTitleString = EditorGUILayout.TextField("Button Title", buttonTitleString);
        buttonTitleText = EditorGUILayout.ObjectField("Button Title Text", buttonTitleText, typeof(TMP_Text), true) as TMP_Text;


        if (GUI.changed)
        {
            informationText.text = informationString;
            EditorUtility.SetDirty(informationText);
            titleText.text = titleString;
            EditorUtility.SetDirty(titleText);
            buttonTitleText.text = buttonTitleString;
            EditorUtility.SetDirty(buttonTitleText);
            EditorUtility.SetDirty(tutorialMapObject);
            EditorUtility.SetDirty(maskTransform);
            EditorUtility.SetDirty(panelTransform);
            EditorUtility.SetDirty(triangleTransform);
        }
        if (GUILayout.Button("Save Tutorial Preset"))
        {
            SaveTutorial();
        }
    }
    private void SaveTutorial()
    {
        SetMaskRect();
        SetPanelRect();
        SetTitleText();
        SetInformationText();
        SetButtonTitle();
        SetTriangle();
        SetMapRect();
    }
    void SetMapRect()
    {
        tutorialMapObject.mapButtonPosition = tutorialMapButtonTransform.anchoredPosition;
        tutorialMapObject.mapButtonWidthAndHeight = tutorialMapButtonTransform.sizeDelta;
        tutorialMapObject.mapButtonAnchorMin = tutorialMapButtonTransform.anchorMin;
        tutorialMapObject.mapButtonAnchorMax = tutorialMapButtonTransform.anchorMax;
        tutorialMapObject.mapButtonPivot = tutorialMapButtonTransform.pivot;
    }

    void SetMaskRect()
    {
        tutorialMapObject.maskPosition = maskTransform.anchoredPosition;
        tutorialMapObject.maskWidthAndHeight = maskTransform.sizeDelta;
        tutorialMapObject.maskAnchorMin = maskTransform.anchorMin;
        tutorialMapObject.maskAnchorMax = maskTransform.anchorMax;
        tutorialMapObject.maskPivot = maskTransform.pivot;
    }
    void SetPanelRect()
    {
        tutorialMapObject.panelPosition = panelTransform.anchoredPosition;
        tutorialMapObject.panelWidthAndHeight = panelTransform.sizeDelta;
        tutorialMapObject.panelAnchorMin = panelTransform.anchorMin;
        tutorialMapObject.panelAnchorMax = panelTransform.anchorMax;
        tutorialMapObject.panelPivot = panelTransform.pivot;
    }
    void SetTriangle()
    {
        tutorialMapObject.trianglePosition = triangleTransform.anchoredPosition;
        tutorialMapObject.triangleWidthAndHeight = triangleTransform.sizeDelta;
        tutorialMapObject.triangleAnchorMin = panelTransform.anchorMin;
        tutorialMapObject.triangleAnchorMax = triangleTransform.anchorMax;
        tutorialMapObject.trianglePivot = triangleTransform.pivot;
        tutorialMapObject.triangleRotation = triangleTransform.localEulerAngles;

    }
    void SetTitleText()
    {
        tutorialMapObject.titleTextPosition = titleText.rectTransform.anchoredPosition;
        tutorialMapObject.titleTextWidthAndHeight = titleText.rectTransform.sizeDelta;
        tutorialMapObject.titleTextAnchorMin = titleText.rectTransform.anchorMin;
        tutorialMapObject.titleTextAnchorMax = titleText.rectTransform.anchorMax;
        tutorialMapObject.titleTextPivot = titleText.rectTransform.pivot;
        tutorialMapObject.titleText = titleText.text;
        tutorialMapObject.titleFontSize = titleText.fontSize;
    }
    void SetInformationText()
    {
        tutorialMapObject.informationTextPosition = informationText.rectTransform.anchoredPosition;
        tutorialMapObject.informationTextWidthAndHeight = informationText.rectTransform.sizeDelta;
        tutorialMapObject.informationTextAnchorMin = informationText.rectTransform.anchorMin;
        tutorialMapObject.informationTextAnchorMax = informationText.rectTransform.anchorMax;
        tutorialMapObject.informationTextPivot = informationText.rectTransform.pivot;
        tutorialMapObject.informationText = informationText.text;
        tutorialMapObject.informationFontSize = informationText.fontSize;
    }
    void SetButtonTitle()
    {
        tutorialMapObject.buttonTitle = buttonTitleText.text;
    }
}
