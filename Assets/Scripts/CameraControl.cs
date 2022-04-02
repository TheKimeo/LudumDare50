using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_moveSpeed = 2;
    public float m_zoomSpeed = 20;

    public float m_inertiaBase = 1000;
    public float m_inertiaLerpTime = 4f;

    private float m_intertiaTimeStart;
    private float m_scrollInertiaT = 0.0f;
    private float m_scrollInertia = 1000;
    private float m_inertiaDir = 1.0f;



    void ResetInertia(float i_dir)
    {
        m_scrollInertiaT = 1.0f;
        m_scrollInertia = m_inertiaBase;
        m_inertiaDir = i_dir;
        m_intertiaTimeStart = Time.time;
    }

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

            ResetInertia(1.0f);

        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f) 
        {
            newPos -= transform.forward * m_zoomSpeed * Time.deltaTime;

            ResetInertia(-1.0f);

        }
        else if( m_scrollInertiaT > 0.0f)
        {
            newPos += transform.forward * m_scrollInertia * Time.deltaTime * m_inertiaDir ;
            m_scrollInertia = Mathf.Lerp(m_inertiaBase, 0.0f, (Time.time - m_intertiaTimeStart) / m_inertiaLerpTime);
        }
        transform.position = newPos;
    }
}
