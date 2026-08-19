using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class CustomResolution : MonoBehaviour
{
    [Header("Importations")] 
    public Dropdown choice;
    
    [Header("Resolution")]
    public int resX = 1920;
    public int resY = 1080;
    public List<int> resolutionX = new List<int> { 1920, 1440, 1280 };
    public List<int> resolutionY = new List<int> { 1080, 1080, 1024 };
    
    // Update is called once per frame
    void Update()
    {
        Screen.SetResolution(resolutionX[choice.value], resolutionY[choice.value], true);
        resX = resolutionX[choice.value];
        resX = resolutionY[choice.value];
    }
}
