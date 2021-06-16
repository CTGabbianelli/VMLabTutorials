using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Tutorial", menuName = "ScriptableObjects/TutorialScriptableObjects/BaseTutorial", order = 1)]
[System.Serializable]
public class TutorialScriptableObjects : ScriptableObject
{
    [Header("Mask")]
    public Vector3 maskPosition;
    public Vector2 maskWidthAndHeight;
    public Vector2 maskAnchorMin;
    public Vector2 maskAnchorMax;
    public Vector2 maskPivot;
    [Header("Alternate Mask")]
    public bool usesAltMask;
    public Vector3 maskAltPosition;
    public Vector2 maskAltWidthAndHeight;
    public Vector2 maskAltAnchorMin;
    public Vector2 maskAltAnchorMax;
    public Vector2 maskAltPivot;
    [Header("Panel")]
    public Vector3 panelPosition;
    public Vector2 panelWidthAndHeight;
    public Vector2 panelAnchorMin;
    public Vector2 panelAnchorMax;
    public Vector2 panelPivot;
    [Header("Alternate Panel")]
    public Vector3 panelAltPosition;
    public Vector2 panelAltWidthAndHeight;
    public Vector2 panelAltAnchorMin;
    public Vector2 panelAltAnchorMax;
    public Vector2 panelAltPivot;
    [Header("Triangle")]
    public Vector3 trianglePosition;
    public Vector2 triangleWidthAndHeight;
    public Vector2 triangleAnchorMin;
    public Vector2 triangleAnchorMax;
    public Vector2 trianglePivot;
    public Vector3 triangleRotation;
    [Header("Alternate Triangle")]
    public Vector3 triangleAltPosition;
    public Vector2 triangleAltWidthAndHeight;
    public Vector2 triangleAltAnchorMin;
    public Vector2 triangleAltAnchorMax;
    public Vector2 triangleAltPivot;
    public Vector3 triangleAltRotation;
    [Header("Title")]
    public Vector3 titleTextPosition;
    public Vector2 titleTextWidthAndHeight;
    public Vector2 titleTextAnchorMin;
    public Vector2 titleTextAnchorMax;
    public Vector2 titleTextPivot;
    public string titleText;
    public float titleFontSize;
    [Header("Information Text")]
    public Vector3 informationTextPosition;
    public Vector2 informationTextWidthAndHeight;
    public Vector2 informationTextAnchorMin;
    public Vector2 informationTextAnchorMax;
    public Vector2 informationTextPivot;
    public string informationText;
    public float informationFontSize;

    public void SetMask(RectTransform maskTransform)
    {
        maskPosition = maskTransform.anchoredPosition;
        maskWidthAndHeight = maskTransform.sizeDelta;
        maskAnchorMin = maskTransform.anchorMin;
        maskAnchorMax = maskTransform.anchorMax;
        maskPivot = maskTransform.pivot;
    }
    public void SetAlternateMask(RectTransform maskTransform)
    {
        maskAltPosition = maskTransform.anchoredPosition;
        maskAltWidthAndHeight = maskTransform.sizeDelta;
        maskAltAnchorMin = maskTransform.anchorMin;
        maskAltAnchorMax = maskTransform.anchorMax;
        maskAltPivot = maskTransform.pivot;
    }
    public void SetPanelTransform(RectTransform panelTransform)
    {
        panelPosition = panelTransform.anchoredPosition;
        panelWidthAndHeight = panelTransform.sizeDelta;
        panelAnchorMin = panelTransform.anchorMin;
        panelAnchorMax = panelTransform.anchorMax;
        panelPivot = panelTransform.pivot;
    }
    public void SetAlternatePanelTransform(RectTransform panelTransform)
    {
        panelAltPosition = panelTransform.anchoredPosition;
        panelAltWidthAndHeight = panelTransform.sizeDelta;
        panelAltAnchorMin = panelTransform.anchorMin;
        panelAltAnchorMax = panelTransform.anchorMax;
        panelAltPivot = panelTransform.pivot;
    }
    public void SetTriangleTransform(RectTransform triangleTransform)
    {
        trianglePosition = triangleTransform.anchoredPosition;
        triangleWidthAndHeight = triangleTransform.sizeDelta;
        triangleAnchorMin = triangleTransform.anchorMin;
        triangleAnchorMax = triangleTransform.anchorMax;
        trianglePivot = triangleTransform.pivot;
        triangleRotation = triangleTransform.localEulerAngles;
    }
    public void SetAlternateTriangleTransform(RectTransform triangleTransform)
    {
        triangleAltPosition = triangleTransform.anchoredPosition;
        triangleAltWidthAndHeight = triangleTransform.sizeDelta;
        triangleAltAnchorMin = triangleTransform.anchorMin;
        triangleAltAnchorMax = triangleTransform.anchorMax;
        triangleAltPivot = triangleTransform.pivot;
        triangleAltRotation = triangleTransform.localEulerAngles;
    }
    public void SetTitleText(TMPro.TMP_Text tempTitleText)
    {
        titleTextPosition = tempTitleText.rectTransform.anchoredPosition;
        titleTextWidthAndHeight = tempTitleText.rectTransform.sizeDelta;
        titleTextAnchorMin = tempTitleText.rectTransform.anchorMin;
        titleTextAnchorMax = tempTitleText.rectTransform.anchorMax;
        titleTextPivot = tempTitleText.rectTransform.pivot;
        titleText = tempTitleText.text;
        titleFontSize = tempTitleText.fontSize;
    }
    public void SetInformationText(TMPro.TMP_Text tempInformationText)
    {
        informationTextPosition = tempInformationText.rectTransform.anchoredPosition;
        informationTextWidthAndHeight = tempInformationText.rectTransform.sizeDelta;
        informationTextAnchorMin = tempInformationText.rectTransform.anchorMin;
        informationTextAnchorMax = tempInformationText.rectTransform.anchorMax;
        informationTextPivot = tempInformationText.rectTransform.pivot;
        informationText = tempInformationText.text;
        informationFontSize = tempInformationText.fontSize;
    }
}
