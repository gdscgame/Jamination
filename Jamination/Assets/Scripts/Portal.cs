using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public static Vector3[] portalEntry = new Vector3[]
    {
        new Vector3(0f,0f,0f),
        new Vector3(0f,0f,0f),
        new Vector3(0f,0f,0f),
        new Vector3(0f,0f,0f),
        new Vector3(-0.5f,-1.3f,0f),
        new Vector3(0.5f,-1.3f,0f),
        new Vector3(0f,0f,0f),
        new Vector3(-2.5f,0.8f,0f),
        new Vector3(0f,0f,0f),
        new Vector3(-6.5f,2.8f,0f),
        new Vector3(0.5f,-1.3f,0f)
    };
    public static Vector3[] portalExit = new Vector3[]
    {
        new Vector3(0f,0f,0f),
        new Vector3(0f,0f,0f),
        new Vector3(0f,0f,0f),
        new Vector3(0f,0f,0f),
        new Vector3(0.5f,0.8f,0f),
        new Vector3(2.5f,0.8f,0f),
        new Vector3(0f,0f,0f),
        new Vector3(0.5f,2.8f,0f),
        new Vector3(0f,0f,0f),
        new Vector3(-5.5f,-0.3f,0f),
        new Vector3(0.5f,1.8f,0f)
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
