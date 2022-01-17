using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDetectionVolumeEvent : MonoBehaviour
{

    private AINoiseDetection noiseDetector;

    void Start()
    {
        noiseDetector = GetComponentInParent<AINoiseDetection>();
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            // send sound clue to the AI noise detection logic
            noiseDetector.receiveSoundClue();
        }
    }
}
