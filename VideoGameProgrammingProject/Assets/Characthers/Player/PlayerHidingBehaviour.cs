using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHidingBehaviour : MonoBehaviour
{   
    // Andrea Mansi - Università Degli Studi di Udine

    private bool isHiding = false;

    void Start()
    {
    }

    void Update()
    {
    }

    public void setPlayerIsHiding(bool status)
    {
        isHiding = status;
    }

    public bool getPlayerIsHiding()
    {
        return isHiding;
    }
}
