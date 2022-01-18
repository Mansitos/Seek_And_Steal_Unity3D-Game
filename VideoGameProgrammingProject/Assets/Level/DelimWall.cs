using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelimWall : MonoBehaviour
{
    void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
