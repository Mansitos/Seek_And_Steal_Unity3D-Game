using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSlider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Slider>().onValueChanged.AddListener(delegate { GraphicsPresetChanged(); });
        GetComponent<Slider>().value = QualitySettings.GetQualityLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GraphicsPresetChanged()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().setGraphicsPreset();
    }


}