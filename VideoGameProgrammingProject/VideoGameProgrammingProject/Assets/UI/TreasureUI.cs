using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureUI : MonoBehaviour
{
    // Andrea Mansi - Università Degli Studi di Udine

    [Header(" - - - Movement Main Parameters - - - ")]
    [SerializeField] GameObject textRef;

    void Start()
    {
    }

    void Update()
    { 
    }

    public void setTextValue(int value, int max)
    {
        textRef.GetComponent<Text>().text = "" + value + "/" + max;
    }
}
