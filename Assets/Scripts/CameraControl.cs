using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_moveSpeed = 2;
    public float m_zoomSpeed = 20;


    void Update()
    {
        //Move cam
        float xMov = Input.GetAxis("Horizontal") * m_moveSpeed * Time.deltaTime;
        float zMov = Input.GetAxis("Vertical") * m_moveSpeed * Time.deltaTime;
        Vector3 newPos = new Vector3(transform.position.x + xMov, transform.position.y, transform.position.z + zMov);
        
        //Zoom cam
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f) 
        {
            newPos += transform.forward * m_zoomSpeed * Time.deltaTime;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f) 
        {
            newPos -= transform.forward * m_zoomSpeed * Time.deltaTime;
        }

        transform.position = newPos;
    }
}
