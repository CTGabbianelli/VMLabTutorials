using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//if UNITY_Editor
public class TutorialEditor : EditorWindow
{
    string informationString;
    string titleString;
    TutorialController tutorialController;
    TutorialScriptableObjects tutorialObject;
    RectTransform maskTransform;
    RectTransform secondaryMaskTransform;
    RectTransform panelTransform;
    RectTransform triangleTransform;
    [SerializeField]
    TMP_Text informationText;
    [SerializeField]
    TMP_Text titleText;

    [MenuItem("Tools/TutorialEditor")]

    //show editor window
    public static void ShowWindow()
    {
        GetWindow(typeof(TutorialEditor));
        
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
            tutorialObject = EditorGUILayout.ObjectField("Tutorial Scriptable Object", tutorialObject, typeof(TutorialScriptableObjects), false) as TutorialScriptableObjects;

            //create mask transform inputs
            GUILayout.Label("", EditorStyles.boldLabel);
            GUILayout.Label("Save Mask Transform", EditorStyles.boldLabel);
            maskTransform = EditorGUILayout.ObjectField("Mask Rect Transform", tutorialController.maskTransform, typeof(RectTransform), true) as RectTransform;

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
            secondaryMaskTransform = EditorGUILayout.ObjectField("Mask Rect Transform", tutorialController.secondaryMaskTransform, typeof(RectTransform), true) as RectTransform;

            //button for saving transforms of secondary mask
            if (GUILayout.Button("Save Secondary Mask Transform"))
            {
                SaveSecondaryMask();
            }

            //create secondary panel transform inputs
            GUILayout.Label("", EditorStyles.boldLabel);
            GUILayout.Label("Save Panel Transform", EditorStyles.boldLabel);
            panelTransform = EditorGUILayout.ObjectField("Panel Rect Transform", tutorialController.panelBGTransform, typeof(RectTransform), true) as RectTransform;
            triangleTransform = EditorGUILayout.ObjectField("Triangle Rect Transform", tutorialController.arrowTransform, typeof(RectTransform), true) as RectTransform;

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
            titleText = EditorGUILayout.ObjectField("Title Text", tutorialController.titleText, typeof(TMP_Text), true) as TMP_Text;
            informationString = EditorGUILayout.TextField("Information", informationString);
            informationText = EditorGUILayout.ObjectField("Information Text", tutorialController.informationText, typeof(TMP_Text), true) as TMP_Text;

            //save input text to scriptable object
            if (GUILayout.Button("Save Text"))
            {
                SaveText();
            }

            //update any changes made in the ui window
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
        Undo.RecordObject(tutorialObject, "Set All");
        SaveMaskTransform();
        SetTitleText();
        SetInformationText();
        SetPanelRect();
        SetTriangle();
        EditorUtility.SetDirty(tutorialObject);
    }
    //only save first mask
    private void SaveMaskTransform()
    {
        Undo.RecordObject(tutorialObject, "Set Mask");
        SetMaskRect();
        EditorUtility.SetDirty(tutorialObject);

    }
    //only save secondary mask
    private void SaveSecondaryMask()
    {
        Undo.RecordObject(this, "Secondary Mask Changed");
        SetSecondaryMaskRect();
        EditorUtility.SetDirty(tutorialObject);
    }
    //only save alternate version of first mask
    private void SaveAltMaskTransform()
    {
        Undo.RecordObject(tutorialObject, "Set Alternate Mask");
        SetAltMaskRect();
        EditorUtility.SetDirty(tutorialObject);
    }
    //save all text assets
    public void SaveText()
    {
        Undo.RecordObject(tutorialObject, "Set Text");
        SetTitleText();
        SetInformationText();
        EditorUtility.SetDirty(tutorialObject);
    }
    //save all panel transforms
    public void SavePanelTransforms()
    {
        Undo.RecordObject(tutorialObject, "Set Panel");
        SetPanelRect();
        SetTriangle();
        EditorUtility.SetDirty(tutorialObject);
    }
    //save all alt panel transforms
    public void SaveAltPanelTransforms()
    {
        Undo.RecordObject(tutorialObject, "Set Alternate Panel");
        SetAltPanelRect();
        SetAltTriangle();
        EditorUtility.SetDirty(tutorialObject);
    }

    void SetMaskRect()
    {
        tutorialObject.SetMask(maskTransform);
    }
    void SetSecondaryMaskRect()
    {
        tutorialObject.SetSecondaryMask(secondaryMaskTransform);
    }
    void SetAltMaskRect()
    {
        tutorialObject.SetAlternateMask(maskTransform);
    }
    void SetPanelRect()
    {
        tutorialObject.SetPanelTransform(panelTransform);
    }
    void SetAltPanelRect()
    {
        tutorialObject.SetAlternatePanelTransform(panelTransform);
    }
    void SetTriangle()
    {
        tutorialObject.SetTriangleTransform(triangleTransform);
    }
    void SetAltTriangle()
    {
        tutorialObject.SetAlternateTriangleTransform(triangleTransform);
    }
    void SetTitleText()
    {
        tutorialObject.SetTitleText(titleText);
    }
    void SetInformationText()
    {
        tutorialObject.SetInformationText(informationText);
    }
}
//#endif