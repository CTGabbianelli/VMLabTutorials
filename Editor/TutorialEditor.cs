using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//#if UNITY_EDITOR
public class TutorialEditor : EditorWindow
{
    string informationString;
    string titleString;
    TutorialController tutorialController;
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
        tutorialController = GameObject.FindObjectOfType<TutorialController>();

        if (GameObject.FindObjectOfType<TutorialController>())
        {
            GUILayout.Label("Save Tutorial Preset", EditorStyles.boldLabel);

            tutorialObject = EditorGUILayout.ObjectField("Tutorial Scriptable Object", tutorialObject, typeof(TutorialMapScriptableObject), false) as TutorialMapScriptableObject;

            GUILayout.Label("", EditorStyles.boldLabel);
            GUILayout.Label("Save Mask Transform", EditorStyles.boldLabel);
            maskTransform = EditorGUILayout.ObjectField("Mask Rect Transform", tutorialController.maskTransform, typeof(RectTransform), true) as RectTransform;
            if (GUILayout.Button("Save Mask Transform"))
            {
                SaveMaskTransform();
            }
            if (GUILayout.Button("Save Alternate Mask Transform"))
            {
                SaveAltMaskTransform();
            }

            GUILayout.Label("", EditorStyles.boldLabel);
            GUILayout.Label("Save Panel Transform", EditorStyles.boldLabel);
            panelTransform = EditorGUILayout.ObjectField("Panel Rect Transform", tutorialController.panelBGTransform, typeof(RectTransform), true) as RectTransform;
            triangleTransform = EditorGUILayout.ObjectField("Triangle Rect Transform", tutorialController.arrowTransform, typeof(RectTransform), true) as RectTransform;
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
            titleText = EditorGUILayout.ObjectField("Title Text", tutorialController.titleText, typeof(TMP_Text), true) as TMP_Text;
            informationString = EditorGUILayout.TextField("Information", informationString);
            informationText = EditorGUILayout.ObjectField("Information Text", tutorialController.informationText, typeof(TMP_Text), true) as TMP_Text;
            if (GUILayout.Button("Save Text"))
            {
                SaveText();
            }

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
        Undo.RecordObject(tutorialObject, "Set All");
        SaveMaskTransform();
        SetTitleText();
        SetInformationText();
        SetPanelRect();
        SetTriangle();
    }
    private void SaveMaskTransform()
    {
        Undo.RecordObject(tutorialObject, "Set Mask");
        SetMaskRect();
    }
    private void SaveAltMaskTransform()
    {
        Undo.RecordObject(tutorialObject, "Set Alternate Mask");
        SetAltMaskRect();
    }
    public void SaveText()
    {
        Undo.RecordObject(tutorialObject, "Set Text");
        SetTitleText();
        SetInformationText();
    }
    public void SavePanelTransforms()
    {
        Undo.RecordObject(tutorialObject, "Set Panel");
        SetPanelRect();
        SetTriangle();
    }
    public void SaveAltPanelTransforms()
    {
        Undo.RecordObject(tutorialObject, "Set Alternate Panel");
        SetAltPanelRect();
        SetAltTriangle();
    }

    void SetMaskRect()
    {
        tutorialObject.SetMask(maskTransform);
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