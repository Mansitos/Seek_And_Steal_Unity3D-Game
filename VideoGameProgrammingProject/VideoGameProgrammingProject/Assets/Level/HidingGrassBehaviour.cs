using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingGrassBehaviour : MonoBehaviour
{

    private Renderer grassRenderer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            // Update player hiding status
            PlayerHidingBehaviour player = other.GetComponent<PlayerHidingBehaviour>();
            player.setPlayerIsHiding(true); 
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // Update player hiding statuss
            PlayerHidingBehaviour player = other.GetComponent<PlayerHidingBehaviour>();
            player.setPlayerIsHiding(false);
        }
    }
}
