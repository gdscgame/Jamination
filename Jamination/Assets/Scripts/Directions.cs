using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directions : MonoBehaviour
{
    public static float[][] horizontalDirections = new float[][]
    {
        new float[] {1f,0f,1f}
    };
    public static float[][] verticalDirections = new float[][]{
        new float[] {0f,1f,0f}
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(horizontalDirections[0][0]);
    }
}