using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial", menuName = "ScriptableObjects/TutorialScriptableObjects/BaseTutorial", order = 1)]
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
    [Header("Triangle")]
    public Vector3 trianglePosition;
    public Vector2 triangleWidthAndHeight;
    public Vector2 triangleAnchorMin;
    public Vector2 triangleAnchorMax;
    public Vector2 trianglePivot;
    public Vector3 triangleRotation;
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
}
