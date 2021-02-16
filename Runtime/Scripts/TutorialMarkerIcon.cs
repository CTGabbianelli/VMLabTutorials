using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMarkerIcon : MonoBehaviour
{
    public GameObject circlePrefab;
    public GameObject backButton;
    public RectTransform spriteLine;
    public Color inActiveColor;
    public Color activeColor;
    public List<GameObject> markers;
    int localCount;
    // Start is called before the first frame update
    public void Initialize(int count)
    {
        localCount = count;
        if (count > 1)
        {
            float width = spriteLine.sizeDelta.x;
            float distance = width/ (count-1);
            for(int i = 0; i<=count-1;i++)
            {
            
                GameObject tutorialCountMarker = Instantiate(circlePrefab, spriteLine);
                RectTransform markerTransform = tutorialCountMarker.GetComponent<RectTransform>();
                markerTransform.sizeDelta = new Vector2(15, 15);
                markerTransform.anchoredPosition = new Vector2((distance * i)-(width/2), 0);
                markers.Add(tutorialCountMarker);

            }
            markers[0].GetComponent<Image>().color = activeColor;

        }
        else
        {
            spriteLine.gameObject.SetActive(false);
            backButton.SetActive(false);
        }


    }

    public void SetMarkerColor(int index)
    {
        if(localCount > 1)
        {
            foreach(GameObject tutMarker in markers)
            {
                tutMarker.GetComponent<Image>().color = inActiveColor;

            }
            markers[index].GetComponent<Image>().color = activeColor;
        }
    }

    public void ActivateMarkers()
    {
        foreach (GameObject tutMarker in markers)
        {
            tutMarker.GetComponent<Animator>().SetBool("Active",true);

        }
    }
    public void DeactivateMarkers()
    {
        foreach (GameObject tutMarker in markers)
        {
            tutMarker.GetComponent<Animator>().SetBool("Active", false);

        }
    }
}
