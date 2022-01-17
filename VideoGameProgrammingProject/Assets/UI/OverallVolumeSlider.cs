using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallVolumeSlider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Slider>().onValueChanged.AddListener(delegate { OverallVolumeChanged(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OverallVolumeChanged()
    {
        GameObject.FindGameObjectWithTag("AudioSystem").GetComponent<AudioSystemManager>().updateOverallVolume();
    }


}