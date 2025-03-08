using System;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    public Camera mainCamera; // Assign your main camera in the Inspector
    public bool isHardMode = false; // Track Hard Mode state
    public Toggle toggle;
    public static Difficulty instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        OnHardModeToggle();
    }

    public void OnHardModeToggle()
    {
        if (toggle.isOn)
        {
            mainCamera.orthographic = true;
        }
        else
        {
            mainCamera.orthographic = false;
        }
    }
    
}
