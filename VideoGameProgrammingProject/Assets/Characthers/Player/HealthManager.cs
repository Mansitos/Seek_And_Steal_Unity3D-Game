using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    // Andrea Mansi - Università Degli Studi di Udine

    [Header(" - - - Main Settings - - - ")]

    [Tooltip("Entity starting lifes")]
    [SerializeField] int startingLifes = 3;

    [Tooltip("Max lifes this entity has")]
    [SerializeField] int maxLifes = 3;

    [Header(" - - - References - - - ")]
    [Tooltip("Reference to the healthbar UI element for this entity")]
    [SerializeField] GameObject lifeBarRef;
    [SerializeField] GameObject inGameMenuRef;
    private InGameUIManager uiManager;
    private LifesIndicatorBar lifeBar;


    private int lifes; // internal status

    void Start()
    {
        lifeBar = lifeBarRef.GetComponent<LifesIndicatorBar>();
        uiManager = inGameMenuRef.GetComponent<InGameUIManager>();

        lifes = startingLifes;
        if(lifes > maxLifes)
        {
            lifes = maxLifes;
        }

        lifeBar.setStatusValue(lifes);
    }

    void Update()
    {
    }

    private void checkDeathConditions()
    {
        if(lifes <= 0)
        {
            Debug.Log("Player died! Game Over! Get good noob!");
            uiManager.HandleGameOver();
        }
    }

    public int getHealth()
    {
        return lifes;
    }

    public void addLifes(int value)
    {
        lifes = lifes + value;
        if(lifes > maxLifes)
        {
            lifes = maxLifes;
        }
        lifeBar.setStatusValue(lifes);
    }

    public void removeLifes(int value)
    {
        lifes = lifes - value;
        lifeBar.setStatusValue(lifes);
        checkDeathConditions();
    }
}
