using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public float m_displayTime = 1.0f;
    [SerializeField] TMPro.TextMeshProUGUI m_Text;
    public Image m_icon;
    public AudioClip m_alertSound;
    Queue<Notification> m_notifQueue;
    string m_curNotif = null;
    float m_curNotDisTimer;

    AudioSource m_audioSource;

    public void Start()
    {
        m_curNotDisTimer = 0;
        m_notifQueue = new Queue<Notification>();
        m_audioSource = GetComponent<AudioSource>();
    }

    public void PushNotif(Notification i_notif)
    {
        if (!m_notifQueue.Contains(i_notif))
        {
            m_notifQueue.Enqueue(i_notif);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_curNotif != null)
        {
            m_curNotDisTimer += Time.deltaTime;
        }

        if (m_curNotDisTimer >= m_displayTime || m_curNotif == null)
        {
            if (m_notifQueue.Count != 0)
            {
                Notification notif = m_notifQueue.Dequeue();
                m_curNotif = notif.m_text;
                m_Text.text = m_curNotif;
                m_icon.enabled = true;
                m_icon.sprite = notif.m_UI_Icon;
                m_audioSource.PlayOneShot(m_alertSound);
            }
            else
            {
                m_Text.text = "";
                m_curNotif = null;
                m_icon.enabled = false;
            }

            m_curNotDisTimer = 0.0f;
        }

        //TODO? Fade out one notif to transition to the next?
    }
}
