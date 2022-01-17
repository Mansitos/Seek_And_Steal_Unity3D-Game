using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Slider>().onValueChanged.AddListener(delegate { MusicVolumeChanged(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MusicVolumeChanged()
    {
        GameObject.FindGameObjectWithTag("AudioSystem").GetComponent<AudioSystemManager>().updateMusicVolume();
    }
   

}
