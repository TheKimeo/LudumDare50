using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_moveSpeed = 2;
    public float m_zoomSpeed = 20;


    void FixedUpdate()
    {
        //Move cam
        float xMov = Input.GetAxis("Horizontal") * m_moveSpeed;
        float zMov = Input.GetAxis("Vertical") * m_moveSpeed;
        Vector3 newPos = new Vector3(transform.position.x + xMov, transform.position.y, transform.position.z + zMov);
        
        //Zoom cam
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f) 
        {
            newPos += transform.forward * m_zoomSpeed;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f) 
        {
            newPos -= transform.forward * m_zoomSpeed;
        }


        transform.position = newPos;
    }
}
