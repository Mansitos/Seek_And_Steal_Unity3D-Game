using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvenTrigger_1 : MonoBehaviour
{

    [SerializeField] GameObject hint;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            hint.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            hint.SetActive(false);
        }
    }
}
