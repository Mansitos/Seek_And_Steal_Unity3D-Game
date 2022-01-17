using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnWalkableArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       // The mesh is useless after navmesh computation
       GameObject.Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
