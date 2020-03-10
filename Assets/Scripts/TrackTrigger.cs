using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackTrigger : MonoBehaviour
{
    public UnityEvent m_ActionList;
    public UnityEvent m_EndActionList;
    public Collider m_triggerBox;
    public bool m_triggered;
    public float m_endTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_triggered && (m_endTimer > 0f))
        {
            m_endTimer -= Time.deltaTime;
            if(m_endTimer < 0f)
            {
                m_EndActionList.Invoke(); 
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_triggered)
        {
            m_ActionList.Invoke();
            m_triggered = true;
        }
        //Debug.Log("ENTERED" + other.name);
    }
}
