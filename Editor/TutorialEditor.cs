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
        if (GameObject.FindObjectOfType<TutorialController>())
        {
            GUILayout.Label("Save Tutorial Preset", EditorStyles.boldLabel);

            tutorialObject = EditorGUILayout.ObjectField("Tutorial Scriptable Object", tutorialObject, typeof(TutorialMapScriptableObject), false) as TutorialMapScriptableObject;

            GUILayout.Label("", EditorStyles.boldLabel);
            GUILayout.Label("Save Mask Transform", EditorStyles.boldLabel);
            maskTransform = EditorGUILayout.ObjectField("Mask Rect Transform", maskTransform, typeof(RectTransform), true) as RectTransform;
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
            panelTransform = EditorGUILayout.ObjectField("Panel Rect Transform", panelTransform, typeof(RectTransform), true) as RectTransform;
            triangleTransform = EditorGUILayout.ObjectField("Triangle Rect Transform", triangleTransform, typeof(RectTransform), true) as RectTransform;
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
            titleText = EditorGUILayout.ObjectField("Title Text", titleText, typeof(TMP_Text), true) as TMP_Text;
            informationString = EditorGUILayout.TextField("Information", informationString);
            informationText = EditorGUILayout.ObjectField("Information Text", informationText, typeof(TMP_Text), true) as TMP_Text;
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
                Debug.LogError(SceneManager.GetActiveScene().name);
                Instantiate(tutPrefab);

            }
        }
    }
    private void SaveTutorial()
    {
        SaveMaskTransform();
        SaveText();
        SavePanelTransforms();
    }
    private void SaveMaskTransform()
    {
        SetMaskRect();
    }
    private void SaveAltMaskTransform()
    {
        SetAltMaskRect();
    }
    public void SaveText()
    {
        SetTitleText();
        SetInformationText();
    }
    public void SavePanelTransforms()
    {
        SetPanelRect();
        SetTriangle();
    }
    public void SaveAltPanelTransforms()
    {
        SetAltPanelRect();
        SetAltTriangle();
    }

    void SetMaskRect()
    {
        tutorialObject.maskPosition = maskTransform.anchoredPosition;
        tutorialObject.maskWidthAndHeight = maskTransform.sizeDelta;
        tutorialObject.maskAnchorMin = maskTransform.anchorMin;
        tutorialObject.maskAnchorMax = maskTransform.anchorMax;
        tutorialObject.maskPivot = maskTransform.pivot;
    }

    void SetAltMaskRect()
    {
        tutorialObject.usesAltMask = true;
        tutorialObject.maskAltPosition = maskTransform.anchoredPosition;
        tutorialObject.maskAltWidthAndHeight = maskTransform.sizeDelta;
        tutorialObject.maskAltAnchorMin = maskTransform.anchorMin;
        tutorialObject.maskAltAnchorMax = maskTransform.anchorMax;
        tutorialObject.maskAltPivot = maskTransform.pivot;
    }
    void SetPanelRect()
    {
        tutorialObject.panelPosition = panelTransform.anchoredPosition;
        tutorialObject.panelWidthAndHeight = panelTransform.sizeDelta;
        tutorialObject.panelAnchorMin = panelTransform.anchorMin;
        tutorialObject.panelAnchorMax = panelTransform.anchorMax;
        tutorialObject.panelPivot = panelTransform.pivot;
    }
    void SetAltPanelRect()
    {
        tutorialObject.panelAltPosition = panelTransform.anchoredPosition;
        tutorialObject.panelAltWidthAndHeight = panelTransform.sizeDelta;
        tutorialObject.panelAltAnchorMin = panelTransform.anchorMin;
        tutorialObject.panelAltAnchorMax = panelTransform.anchorMax;
        tutorialObject.panelAltPivot = panelTransform.pivot;
    }
    void SetTriangle()
    {
        tutorialObject.trianglePosition = triangleTransform.anchoredPosition;
        tutorialObject.triangleWidthAndHeight = triangleTransform.sizeDelta;
        tutorialObject.triangleAnchorMin = triangleTransform.anchorMin;
        tutorialObject.triangleAnchorMax = triangleTransform.anchorMax;
        tutorialObject.trianglePivot = triangleTransform.pivot;
        tutorialObject.triangleRotation = triangleTransform.localEulerAngles;

    }
    void SetAltTriangle()
    {
        tutorialObject.triangleAltPosition = triangleTransform.anchoredPosition;
        tutorialObject.triangleAltWidthAndHeight = triangleTransform.sizeDelta;
        tutorialObject.triangleAltAnchorMin = triangleTransform.anchorMin;
        tutorialObject.triangleAltAnchorMax = triangleTransform.anchorMax;
        tutorialObject.triangleAltPivot = triangleTransform.pivot;
        tutorialObject.triangleAltRotation = triangleTransform.localEulerAngles;

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
//#endif