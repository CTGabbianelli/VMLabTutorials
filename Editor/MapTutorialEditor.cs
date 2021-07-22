using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

#if UNITY_Editor
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

    public static void ShowMapWindow()
    {
        GetWindow(typeof(MapTutorialEditor));
    }
    private void OnGUI()
    {
        tutorialController = GameObject.FindObjectOfType<TutorialController>();

        if (GameObject.FindObjectOfType<TutorialController>())
        {
            GUILayout.Label("Save Tutorial Preset", EditorStyles.boldLabel);

            tutorialMapObject = EditorGUILayout.ObjectField("Tutorial Scriptable Object", tutorialMapObject, typeof(TutorialMapScriptableObject), false) as TutorialMapScriptableObject;

            GUILayout.Label("", EditorStyles.boldLabel);
            GUILayout.Label("Save Mask Transform", EditorStyles.boldLabel);
            maskTransform = EditorGUILayout.ObjectField("Mask Rect Transform", tutorialController.mapMaskTransform, typeof(RectTransform), true) as RectTransform;

            if (GUILayout.Button("Save Mask Transform"))
            {
                SaveMaskTransform();
            }
            if (GUILayout.Button("Save Alternate Mask Transform"))
            {
                SaveAltMaskTransform();
            }

            GUILayout.Label("", EditorStyles.boldLabel);
            GUILayout.Label("Save Secondary Mask Transform", EditorStyles.boldLabel);
            secondaryMaskTransform = EditorGUILayout.ObjectField("Mask Rect Transform", tutorialController.mapSecondaryMaskTransform, typeof(RectTransform), true) as RectTransform;

            if (GUILayout.Button("Save Secondary Mask Transform"))
            {
                SaveSecondaryMask();
            }

            GUILayout.Label("", EditorStyles.boldLabel);
            GUILayout.Label("Save Panel Transform", EditorStyles.boldLabel);
            panelTransform = EditorGUILayout.ObjectField("Panel Rect Transform", tutorialController.mapPanelBGTransform, typeof(RectTransform), true) as RectTransform;
            triangleTransform = EditorGUILayout.ObjectField("Triangle Rect Transform", tutorialController.mapArrowTransform, typeof(RectTransform), true) as RectTransform;
            if (GUILayout.Button("Save Panel Transform"))
            {
                SavePanelTransforms();
            }
            if (GUILayout.Button("Save Alternate Panel Transform"))
            {
                SaveAltPanelTransforms();
            }

            GUILayout.Label("", EditorStyles.boldLabel);
            GUILayout.Label("Save Panel Text", EditorStyles.boldLabel);
            titleString = EditorGUILayout.TextField("Title", titleString);
            titleText = EditorGUILayout.ObjectField("Title Text", tutorialController.mapTitleText, typeof(TMP_Text), true) as TMP_Text;
            informationString = EditorGUILayout.TextField("Information", informationString);
            informationText = EditorGUILayout.ObjectField("Information Text", tutorialController.mapInformationText, typeof(TMP_Text), true) as TMP_Text;
            if (GUILayout.Button("Save Text"))
            {
                SaveText();
            }

            GUILayout.Label("", EditorStyles.boldLabel);
            GUILayout.Label("Save Map Button", EditorStyles.boldLabel);
            tutorialMapButtonTransform = EditorGUILayout.ObjectField("Map Button Rect Transform", tutorialMapButtonTransform, typeof(RectTransform), true) as RectTransform;
            buttonTitleString = EditorGUILayout.TextField("Button Title", buttonTitleString);
            buttonTitleText = EditorGUILayout.ObjectField("Button Title Text", buttonTitleText, typeof(TMP_Text), true) as TMP_Text;
            if (GUILayout.Button("Save Map Button"))
            {
                SaveMapButton();
            }


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

            GUILayout.Label("", EditorStyles.boldLabel);
            if (GUILayout.Button("Save All"))
            {
                SaveTutorial();
            }
        }
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
    private void SaveMaskTransform()
    {
        Undo.RecordObject(tutorialMapObject,"Set Mask");
        SetMaskRect();
        EditorUtility.SetDirty(tutorialMapObject);
    }
    private void SaveSecondaryMask()
    {
        Undo.RecordObject(this, "Secondary Mask Changed");
        SetSecondaryMaskRect();
        EditorUtility.SetDirty(tutorialMapObject);
    }
    private void SaveAltMaskTransform()
    {
        Undo.RecordObject(tutorialMapObject, "Set Alternate Mask");
        SetAltMaskRect();
        EditorUtility.SetDirty(tutorialMapObject);
    }
    public void SaveText()
    {
        Undo.RecordObject(tutorialMapObject, "Set Text");
        SetTitleText(); 
        SetInformationText();
        EditorUtility.SetDirty(tutorialMapObject);
    }
    public void SavePanelTransforms()
    {
        Undo.RecordObject(tutorialMapObject, "Set Panel");
        SetPanelRect();
        SetTriangle();
        EditorUtility.SetDirty(tutorialMapObject);
    }
    public void SaveAltPanelTransforms()
    {
        Undo.RecordObject(tutorialMapObject, "Set Alternate Panel");
        SetAltPanelRect();
        SetAltTriangle();
        EditorUtility.SetDirty(tutorialMapObject);
    }
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
#endif
