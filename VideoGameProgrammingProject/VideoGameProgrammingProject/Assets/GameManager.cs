using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject PlayerRef;
    [SerializeField] GameObject MainCameraRef;

    void Start()
    {
    }
    void Update()
    {
        
    }

    public GameObject getPlayerRef()
    {
        return PlayerRef;
    }

    public GameObject getMainCameraRef()
    {
        return MainCameraRef;
    }

    public void setGraphicsPreset()
    {
        // get preset index by looking at the slider
        int preset = (int)GameObject.FindGameObjectWithTag("GraphicsSlider").GetComponent<Slider>().value;
        QualitySettings.SetQualityLevel(preset, true);
    }
}
