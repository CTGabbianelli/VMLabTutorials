using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialSet", menuName = "ScriptableObjects/TutorialScriptableObjectSet", order = 1)]
public class TutorialObjectsList : ScriptableObject
{
    public List<TutorialScriptableObjects> tutorialSets;
}
