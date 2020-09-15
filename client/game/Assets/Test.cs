using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Camera m_Camera;
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            m_Camera.aspect = m_Camera.aspect - Time.deltaTime;
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            m_Camera.aspect = m_Camera.aspect + Time.deltaTime;
        }
    }
}
