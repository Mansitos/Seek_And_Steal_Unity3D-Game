using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StealProgressBar : MonoBehaviour
{

    [SerializeField] GameObject sliderRef;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setActive(bool flag)
    {
        sliderRef.SetActive(flag);
    }
}
