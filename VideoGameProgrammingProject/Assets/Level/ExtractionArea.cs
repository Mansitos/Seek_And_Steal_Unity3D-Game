using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionArea : MonoBehaviour
{

    [SerializeField] GameObject[] treasures;
    [SerializeField] GameObject message;
    [SerializeField] GameObject inGameMenuRef;
    private InGameUIManager inGameUI;

    void Start()
    {
        inGameUI = inGameMenuRef.GetComponent<InGameUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getNumberOfTreasures()
    {
        return treasures.Length;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            verifyWinConditions();
        }
        
    }

    private void verifyWinConditions()
    {
        var allTreasuresPickedUp = true;

        for(int i = 0; i<treasures.Length; i++)
        {
            if(treasures[i]!= null)
            {
                allTreasuresPickedUp = false;
            }
        }

        if(allTreasuresPickedUp == true)
        {
            inGameUI.HandleWin();
        }
        else
        {
            message.SetActive(true);
        }
    }
}
