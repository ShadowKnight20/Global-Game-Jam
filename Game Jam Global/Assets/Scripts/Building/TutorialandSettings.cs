using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialandSettings : MonoBehaviour
{
    public Canvas targetCanvas; 
    public Button toggleButton; 

    private bool isCanvasActive = true; 

    void Start()
    {
        // Ensure the button has an assigned function
        if (toggleButton != null)
            toggleButton.onClick.AddListener(ToggleCanvas);
    }

    void ToggleCanvas()
    {
        isCanvasActive = !isCanvasActive;
        targetCanvas.gameObject.SetActive(isCanvasActive);
    }
}