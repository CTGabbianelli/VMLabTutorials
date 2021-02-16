using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenButton : MonoBehaviour
{
    public Image image;
    public Sprite expandSprite;
    public Sprite collapseSprite;

    public Color defaultColor = new Color(1f, 1f, 1f, 1f);
    public Color hoveredColor; // = new Color(235f / 255f, 235f / 255f, 235f / 255f, 1f);

    private int savedWidth;
    private int savedHeight;

    private void Update()
    {
        if (Screen.fullScreen)
        {
            image.sprite = collapseSprite;
        }
        else
        {
            image.sprite = expandSprite;
        }
    }

    public void Click()
    {
        if (Screen.fullScreen)
        {
            // Currently in fullscreen mode, switching to windowed
            Screen.SetResolution(savedWidth, savedHeight, false);
        }
        else
        {
            // Currently in windowed mode, switching to fullscreen
            savedWidth = Mathf.RoundToInt(Screen.height * 1.78f);
            savedHeight = Screen.height;

            Screen.SetResolution(Mathf.RoundToInt(Screen.currentResolution.height * 1.78f), Screen.currentResolution.height, true);
        }
    }

    public void BeginHover()
    {
        image.color = hoveredColor;
    }

    public void EndHover()
    {
        image.color = defaultColor;
    }

}