using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DoubleAction : MonoBehaviour
{
    public int bufAmount = 0;
    public static float[][] extraHorizontalDirections = new float[][]
    {
        new float[] {0f,0f,0f},
        new float[] {0f,0f,0f},
        new float[] {0f,0f,0f},
        new float[] {1f,1f,0f},
        new float[] {0f,0f,0f},
        new float[] {0f,0f,0f},
        new float[] {1f,1f,1f},
        new float[] {0f,0f,0f},
        new float[] {1f,1f,0f},
    };

    public static float[][] extraVerticalDirections = new float[][]
    {
        new float[] {0f,0f,0f},
        new float[] {0f,0f,0f},
        new float[] {0f,0f,0f},
        new float[] {0f,0f,0f},
        new float[] {1f,1f,0f},
        new float[] {-1f,-1f,0f},
        new float[] {0f,0f,0f},
        new float[] {0f,0f,0f},
        new float[] {0f,0f,0f},
    };
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex >= 3)
        {
            bufAmount += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
