using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSystemManager : MonoBehaviour
{
    [SerializeField] float musicVolume = 1.0f; // must be from 0 to 1
    [SerializeField] float overallVolume = 1.0f; // must be from 0 to 1
    private AudioSource musicPlayer;
    [SerializeField] GameObject musicVolumeSlider;
    [SerializeField] GameObject overallVolumeSlider;
    private GameManager gm;

    private void Awake()
    {
        var instance = GameObject.FindGameObjectsWithTag("AudioSystem");

        if(instance.Length > 1) // there is already one audio system, so destroy this
        {
            Debug.Log("Deleting the new AudioSystem");
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);

        musicVolume = Mathf.Clamp(musicVolume, 0.0f, 1.0f);
        overallVolume = Mathf.Clamp(overallVolume, 0.0f, 1.0f);

        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.volume = musicVolume;

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        AudioListener.volume = overallVolume;
    }

    void Update()
    {
        
    }

    public void updateMusicVolume()
    {
        musicVolume = musicVolumeSlider.GetComponent<Slider>().value;
        musicPlayer.volume = musicVolume;
    }

    public void updateOverallVolume()
    {
        overallVolume = overallVolumeSlider.GetComponent<Slider>().value;
        AudioListener.volume = overallVolume;
        
    }

    public void updateRefs()
    {
        if (musicVolumeSlider == null)
        {
            musicVolumeSlider = GameObject.FindGameObjectWithTag("MusicVolumeSlider");
        }

        if (overallVolumeSlider == null)
        {
            overallVolumeSlider = GameObject.FindGameObjectWithTag("VolumeSlider");
        }

        musicVolumeSlider.GetComponent<Slider>().value = musicVolume;
        overallVolumeSlider.GetComponent<Slider>().value = overallVolume;
    }



}
