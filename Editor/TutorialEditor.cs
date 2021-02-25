﻿using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_Editor
public class TutorialEditor : EditorWindow
{
    string informationString;
    string titleString;
    TutorialScriptableObjects tutorialObject;
    RectTransform maskTransform;
    RectTransform panelTransform;
    RectTransform triangleTransform;
    [SerializeField]
    TMP_Text informationText;
    [SerializeField]
    TMP_Text titleText;

    [MenuItem("Tools/TutorialEditor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TutorialEditor));
    }
    private void OnGUI()
    {
        GUILayout.Label("Save Tutorial Preset", EditorStyles.boldLabel);
        tutorialObject = EditorGUILayout.ObjectField("Tutorial Scriptable Object", tutorialObject, typeof(TutorialScriptableObjects), false) as TutorialScriptableObjects;
        maskTransform = EditorGUILayout.ObjectField("Mask Rect Transform", maskTransform, typeof(RectTransform), true) as RectTransform;
        panelTransform = EditorGUILayout.ObjectField("Panel Rect Transform", panelTransform, typeof(RectTransform), true) as RectTransform;
        triangleTransform = EditorGUILayout.ObjectField("Triangle Rect Transform", triangleTransform, typeof(RectTransform), true) as RectTransform;
        titleString = EditorGUILayout.TextField("Title", titleString);
        titleText = EditorGUILayout.ObjectField("Title Text", titleText, typeof(TMP_Text), true) as TMP_Text;
        informationString = EditorGUILayout.TextField("Information", informationString);
        informationText = EditorGUILayout.ObjectField("Information Text", informationText, typeof(TMP_Text), true) as TMP_Text;


        if (GUI.changed)
        {
            informationText.text = informationString;
            EditorUtility.SetDirty(informationText);
            titleText.text = titleString;
            EditorUtility.SetDirty(titleText);
            EditorUtility.SetDirty(tutorialObject);
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
        SetTringle();
    }
    void SetMaskRect()
    {
        tutorialObject.maskPosition = maskTransform.anchoredPosition;
        tutorialObject.maskWidthAndHeight = maskTransform.sizeDelta;
        tutorialObject.maskAnchorMin = maskTransform.anchorMin;
        tutorialObject.maskAnchorMax = maskTransform.anchorMax;
        tutorialObject.maskPivot = maskTransform.pivot;
    }
    void SetPanelRect()
    {
        tutorialObject.panelPosition = panelTransform.anchoredPosition;
        tutorialObject.panelWidthAndHeight = panelTransform.sizeDelta;
        tutorialObject.panelAnchorMin = panelTransform.anchorMin;
        tutorialObject.panelAnchorMax = panelTransform.anchorMax;
        tutorialObject.panelPivot = panelTransform.pivot;
    }
    void SetTringle()
    {
        tutorialObject.trianglePosition = triangleTransform.anchoredPosition;
        tutorialObject.triangleWidthAndHeight = triangleTransform.sizeDelta;
        tutorialObject.triangleAnchorMin = panelTransform.anchorMin;
        tutorialObject.triangleAnchorMax = triangleTransform.anchorMax;
        tutorialObject.trianglePivot = triangleTransform.pivot;
        tutorialObject.triangleRotation = triangleTransform.localEulerAngles;

    }
    void SetTitleText()
    {
        tutorialObject.titleTextPosition = titleText.rectTransform.anchoredPosition;
        tutorialObject.titleTextWidthAndHeight = titleText.rectTransform.sizeDelta;
        tutorialObject.titleTextAnchorMin = titleText.rectTransform.anchorMin;
        tutorialObject.titleTextAnchorMax = titleText.rectTransform.anchorMax;
        tutorialObject.titleTextPivot = titleText.rectTransform.pivot;
        tutorialObject.titleText = titleText.text;
        tutorialObject.titleFontSize = titleText.fontSize;
    }
    void SetInformationText()
    {
        tutorialObject.informationTextPosition = informationText.rectTransform.anchoredPosition;
        tutorialObject.informationTextWidthAndHeight = informationText.rectTransform.sizeDelta;
        tutorialObject.informationTextAnchorMin = informationText.rectTransform.anchorMin;
        tutorialObject.informationTextAnchorMax = informationText.rectTransform.anchorMax;
        tutorialObject.informationTextPivot = informationText.rectTransform.pivot;
        tutorialObject.informationText = informationText.text;
        tutorialObject.informationFontSize = informationText.fontSize;
    }
}
#endif