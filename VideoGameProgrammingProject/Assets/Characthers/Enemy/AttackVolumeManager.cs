using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackVolumeManager : MonoBehaviour
{
    private bool playerInDamageZone = false;

    public bool isPlayerInsideDamageVolume()
    {
        return playerInDamageZone;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInDamageZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInDamageZone = false;
        }
    }
}
