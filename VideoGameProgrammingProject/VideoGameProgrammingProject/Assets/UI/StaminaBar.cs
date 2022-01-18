using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Slider>().value = GetComponent<Slider>().maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setValue(float value)
    {
        GetComponent<Slider>().value = Mathf.Clamp(value, GetComponent<Slider>().minValue, GetComponent<Slider>().maxValue);
    }


}
