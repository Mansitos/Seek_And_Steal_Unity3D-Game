using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCounter : MonoBehaviour
{
    // Andrea Mansi - Università Degli Studi di Udine

    [Header(" - - - Movement Main Parameters - - - ")]
    [SerializeField] GameObject extractionAreaRef;
    [SerializeField] GameObject treasureCounterUIRef;

    // Internal status
    private int pickedUpTreasures;
    private int numberOfTreasures;

    void Start()
    {
        numberOfTreasures = extractionAreaRef.GetComponent<ExtractionArea>().getNumberOfTreasures();
        pickedUpTreasures = 0;

        treasureCounterUIRef.GetComponent<TreasureUI>().setTextValue(pickedUpTreasures, numberOfTreasures);
    }

    void Update()
    {
    }

    public void addOneTreasureOnCounter()
    {
        pickedUpTreasures += 1;
        treasureCounterUIRef.GetComponent<TreasureUI>().setTextValue(pickedUpTreasures, numberOfTreasures);
    }
}
