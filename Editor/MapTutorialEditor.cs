using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//#if UNITY_Editor
[System.Serializable]
public class MapTutorialEditor : EditorWindow
{
    string informationString;
    string titleString;
    string buttonTitleString;
    [SerializeField]
    TutorialController tutorialController;
    TutorialMapScriptableObject tutorialMapObject;
    RectTransform tutorialMapButtonTransform;
    RectTransform maskTransform;
    RectTransform secondaryMaskTransform;
    RectTransform panelTransform;
    RectTransform triangleTransform;
    [SerializeField]
    TMP_Text informationText;
    [SerializeField]
    TMP_Text titleText;
    [SerializeField]
    TMP_Text buttonTitleText;
    [MenuItem("Tools/MapTutorialEditor")]

    //show editor window
    public static void ShowMapWindow()
    {
        GetWindow(typeof(MapTutorialEditor));
    }
    //Create All GUI elements
    private void OnGUI()
    {

        //check if a tutorial controller already exists
        if (GameObject.FindObjectOfType<TutorialController>())
        {
            //localize the tutorialController
            tutorialController = GameObject.FindObjectOfType<TutorialController>();

            //name for the window
            GUILayout.Label("Save Tutorial Preset", EditorStyles.boldLabel);

            //input field for the scriptable object to be saved to and limit to TutorialMapScriptableObject type
            tutorialMapObject = EditorGUILayout.ObjectField("Tutorial Scriptable Object", tutorialMapObject, typeof(TutorialMapScriptableObject), false) as TutorialMapScriptableObject;

            //create mask transform inputs
            GUILayout.Label("", EditorStyles.boldLabel);
            GUILayout.Label("Save Mask Transform", EditorStyles.boldLabel);
            maskTransform = EditorGUILayout.ObjectField("Mask Rect Transform", tutorialController.mapMaskTransform, typeof(RectTransform), true) as RectTransform;

            //buttons for saving transforms of normal mask or alternate mask
            //alternate mask is used for changing size in certain interactions
            if (GUILayout.Button("Save Mask Transform"))
            {
                SaveMaskTransform();
            }
            if (GUILayout.Button("Save Alternate Mask Transform"))
            {
                SaveAltMaskTransform();
            }

            //create secondary mask transform inputs
            GUILayout.Label("", EditorStyles.boldLabel);
            GUILayout.Label("Save Secondary Mask Transform", EditorStyles.boldLabel);
            secondaryMaskTransform = EditorGUILayout.ObjectField("Mask Rect Transform", tutorialController.mapSecondaryMaskTransform, typeof(RectTransform), true) as RectTransform;

            //button for saving transforms of secondary mask
            if (GUILayout.Button("Save Secondary Mask Transform"))
            {
                SaveSecondaryMask();
            }

            //create secondary panel transform inputs
            GUILayout.Label("", EditorStyles.boldLabel);
            GUILayout.Label("Save Panel Transform", EditorStyles.boldLabel);
            panelTransform = EditorGUILayout.ObjectField("Panel Rect Transform", tutorialController.mapPanelBGTransform, typeof(RectTransform), true) as RectTransform;
            triangleTransform = EditorGUILayout.ObjectField("Triangle Rect Transform", tutorialController.mapArrowTransform, typeof(RectTransform), true) as RectTransform;

            //buttons for saving transforms of normal panel or alternate panel
            //alternate panel is used for changing size in certain interactions
            if (GUILayout.Button("Save Panel Transform"))
            {
                SavePanelTransforms();
            }
            if (GUILayout.Button("Save Alternate Panel Transform"))
            {
                SaveAltPanelTransforms();
            }

            //create inputs for tutorial text
            GUILayout.Label("", EditorStyles.boldLabel);
            GUILayout.Label("Save Panel Text", EditorStyles.boldLabel);
            titleString = EditorGUILayout.TextField("Title", titleString);
            titleText = EditorGUILayout.ObjectField("Title Text", tutorialController.mapTitleText, typeof(TMP_Text), true) as TMP_Text;
            informationString = EditorGUILayout.TextField("Information", informationString);
            informationText = EditorGUILayout.ObjectField("Information Text", tutorialController.mapInformationText, typeof(TMP_Text), true) as TMP_Text;
            
            //save input text to scriptable object
            if (GUILayout.Button("Save Text"))
            {
                SaveText();
            }

            
            //create inputs for the map guide buttons
            GUILayout.Label("", EditorStyles.boldLabel);
            GUILayout.Label("Save Map Button", EditorStyles.boldLabel);
            tutorialMapButtonTransform = EditorGUILayout.ObjectField("Map Button Rect Transform", tutorialMapButtonTransform, typeof(RectTransform), true) as RectTransform;
            buttonTitleString = EditorGUILayout.TextField("Button Title", buttonTitleString);
            buttonTitleText = EditorGUILayout.ObjectField("Button Title Text", buttonTitleText, typeof(TMP_Text), true) as TMP_Text;

            //save button values to scriptable object
            if (GUILayout.Button("Save Map Button"))
            {
                SaveMapButton();
            }

            //update any changes made in the ui window
            if (GUI.changed)
            {
                informationText.text = informationString;
                EditorUtility.SetDirty(informationText);
                titleText.text = titleString;
                EditorUtility.SetDirty(titleText);
                EditorUtility.SetDirty(tutorialMapObject);
                EditorUtility.SetDirty(maskTransform);
                EditorUtility.SetDirty(panelTransform);
                EditorUtility.SetDirty(triangleTransform);

                if(buttonTitleText!=null && tutorialMapButtonTransform != null)
                {
                    buttonTitleText.text = buttonTitleString;
                    EditorUtility.SetDirty(buttonTitleText);
                    EditorUtility.SetDirty(tutorialMapButtonTransform);
                }
            }


            //button for saving all tutorial aspects
            GUILayout.Label("", EditorStyles.boldLabel);
            if (GUILayout.Button("Save All"))
            {
                SaveTutorial();
            }
        }
        //if a tutorial controller does not exist Add button for user to create one
        else
        {
            GUILayout.Label("There is currently no tutorial system in the scene", EditorStyles.boldLabel);
            if (GUILayout.Button("Add Tutorial System to Scene"))
            {
                GameObject tutPrefab = PrefabUtility.LoadPrefabContents("Packages/com.vmlab.tutorialslibrary/Runtime/Prefabs/TitleAndTutorialCanvas.prefab") as GameObject;
                Instantiate(tutPrefab);
            }
        }

    }
    //save all aspects of tutorial
    private void SaveTutorial()
    {
        Undo.RecordObject(tutorialMapObject, "Set All");
        SaveMaskTransform();
        SetTitleText();
        SetInformationText();
        SetPanelRect();
        SetTriangle();
        SetButtonTitle();
        SetMapButtonRect();
        EditorUtility.SetDirty(tutorialMapObject);
    }
    //only save first mask
    private void SaveMaskTransform()
    {
        Undo.RecordObject(tutorialMapObject,"Set Mask");
        SetMaskRect();
        EditorUtility.SetDirty(tutorialMapObject);
    }
    //only save secondary mask
    private void SaveSecondaryMask()
    {
        Undo.RecordObject(this, "Secondary Mask Changed");
        SetSecondaryMaskRect();
        EditorUtility.SetDirty(tutorialMapObject);
    }
    //only save alternate version of first mask
    private void SaveAltMaskTransform()
    {
        Undo.RecordObject(tutorialMapObject, "Set Alternate Mask");
        SetAltMaskRect();
        EditorUtility.SetDirty(tutorialMapObject);
    }
    //save all text assets
    public void SaveText()
    {
        Undo.RecordObject(tutorialMapObject, "Set Text");
        SetTitleText(); 
        SetInformationText();
        EditorUtility.SetDirty(tutorialMapObject);
    }
    //save all panel transforms
    public void SavePanelTransforms()
    {
        Undo.RecordObject(tutorialMapObject, "Set Panel");
        SetPanelRect();
        SetTriangle();
        EditorUtility.SetDirty(tutorialMapObject);
    }
    //save all alt panel transforms
    public void SaveAltPanelTransforms()
    {
        Undo.RecordObject(tutorialMapObject, "Set Alternate Panel");
        SetAltPanelRect();
        SetAltTriangle();
        EditorUtility.SetDirty(tutorialMapObject);
    }
    //save aspects of the map button
    public void SaveMapButton()
    {
        Undo.RecordObject(tutorialMapObject, "Set Map Button");
        SetButtonTitle();
        SetMapButtonRect();
        EditorUtility.SetDirty(tutorialMapObject);
    }
    void SetMapButtonRect()
    {
        tutorialMapObject.SetMapButton(tutorialMapButtonTransform);
    }

    void SetMaskRect()
    {
        tutorialMapObject.SetMask(maskTransform);
    }
    void SetSecondaryMaskRect()
    {
        tutorialMapObject.SetSecondaryMask(secondaryMaskTransform);
    }
    void SetAltMaskRect()
    {
        tutorialMapObject.SetAlternateMask(maskTransform);
    }
    void SetPanelRect()
    {
        tutorialMapObject.SetPanelTransform(panelTransform);
    }
    void SetAltPanelRect()
    {
        tutorialMapObject.SetAlternatePanelTransform(panelTransform);
    }
    void SetTriangle()
    {
        tutorialMapObject.SetTriangleTransform(triangleTransform);
    }
    void SetAltTriangle()
    {
        tutorialMapObject.SetAlternateTriangleTransform(triangleTransform);
    }
    void SetTitleText()
    {
        tutorialMapObject.SetTitleText(titleText);
    }
    void SetInformationText()
    {
        tutorialMapObject.SetInformationText(informationText);
    }
    void SetButtonTitle()
    {
        tutorialMapObject.SetMapTitle(buttonTitleText.text);
    }
}
//#endif
