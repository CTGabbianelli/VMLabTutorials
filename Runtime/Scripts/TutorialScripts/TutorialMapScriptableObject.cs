using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial", menuName = "ScriptableObjects/TutorialScriptableObjects/MapTutorial", order = 2)]
public class TutorialMapScriptableObject : TutorialScriptableObjects
{
    [Header("Button Title")]
    public string buttonTitle;

    [Header("Map Button")]
    public Vector3 mapButtonPosition;
    public Vector2 mapButtonWidthAndHeight;
    public Vector2 mapButtonAnchorMin;
    public Vector2 mapButtonAnchorMax;
    public Vector2 mapButtonPivot;
}
