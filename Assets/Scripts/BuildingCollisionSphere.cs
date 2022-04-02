using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollisionSphere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SphereCollider>().radius = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
