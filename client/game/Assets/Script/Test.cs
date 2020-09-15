using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Camera m_Camera;
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponent<Camera>();
    }

    private void OnEnable() {

    }

    private void OnDisable() {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
