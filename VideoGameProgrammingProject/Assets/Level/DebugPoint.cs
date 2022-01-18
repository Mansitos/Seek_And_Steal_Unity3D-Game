using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var MeshRenderer = GetComponent<MeshRenderer>();
        if (MeshRenderer)
        {
            MeshRenderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
