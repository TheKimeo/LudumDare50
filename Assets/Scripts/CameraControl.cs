using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_moveSpeed = 2;
    public float m_zoomSpeed = 20;
    public float m_inertiaBase = 1000;
    public float m_inertiaLerpTime = 4f;

    public float m_mapRadius = 70;


    public float m_minDistToTerrain = 5.0f;
    public float m_maxDistToTerrain = 50.0f;

    private float m_intertiaTimeStart;
    private float m_scrollInertiaT = 0.0f;
    private float m_scrollInertia = 1000;
    private float m_inertiaDir = 1.0f;


    enum ZoomState
    {
        OK,
        TOO_CLOSE,
        TOO_FAR
    }

    void ResetInertia(float i_dir)
    {
        m_scrollInertiaT = 1.0f;
        m_scrollInertia = m_inertiaBase;
        m_inertiaDir = i_dir;
        m_intertiaTimeStart = Time.time;
    }

    ZoomState CheckZoom()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("PlaceableGround")))
        {
            if(hit.distance <= m_minDistToTerrain)
            {
                return ZoomState.TOO_CLOSE;
            }
            if(hit.distance >= m_maxDistToTerrain)
            {
                return ZoomState.TOO_FAR;
            }
            return ZoomState.OK;
        }
        return ZoomState.OK;
    }


    void Update()
    {
        //Move cam
        float xMov = Input.GetAxis("Horizontal") * m_moveSpeed * Time.deltaTime;
        float zMov = Input.GetAxis("Vertical") * m_moveSpeed * Time.deltaTime;
        Vector3 newPos = new Vector3(transform.position.x + xMov, transform.position.y, transform.position.z + zMov);

        //Clamp pos
        Vector2 pos2d = new Vector2(newPos.x, newPos.z);
        float distance = Vector2.Distance(pos2d, Vector2.zero);
        if (distance > m_mapRadius)
        {
            Vector2 dirVec = pos2d * (m_mapRadius / distance);
            pos2d = dirVec;
            newPos.x = pos2d.x;
            newPos.z = pos2d.y;
        }

        ZoomState zoomState = CheckZoom();

        //Zoom cam
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f && zoomState != ZoomState.TOO_CLOSE) 
        {
            newPos += transform.forward * m_zoomSpeed * Time.deltaTime;

            ResetInertia(1.0f);

        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f && zoomState != ZoomState.TOO_FAR) 
        {
            newPos -= transform.forward * m_zoomSpeed * Time.deltaTime;

            ResetInertia(-1.0f);

        }
        else if( m_scrollInertiaT > 0.0f && zoomState == ZoomState.OK)
        {
            newPos += transform.forward * m_scrollInertia * Time.deltaTime * m_inertiaDir ;
            m_scrollInertia = Mathf.Lerp(m_inertiaBase, 0.0f, (Time.time - m_intertiaTimeStart) / m_inertiaLerpTime);
        }
        transform.position = newPos;
    }
}
