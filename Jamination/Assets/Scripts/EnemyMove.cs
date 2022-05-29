using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMove : MonoBehaviour
{
    private int moveIndex = 0;
    Vector3 targetEnemyPosition;
    private float lerpSpeed = 1f;
    public CharacterMovement characterMovement;
    private bool isMove = false;
    public static float[][] enemyHorizontalDirections = new float[][]
    {
        new float[]{},
        new float[]{},
        new float[]{},
        new float[]{},
        new float[]{},
        new float[]{},
        new float[]{-1f,0f,0f,-1f,-1f,-1f,-1f,0f,-1f,0f,0f,0f,1f,1f,1f},
        new float[]{0f,-1f,-1f,0f,-1f,-1f,0f,-1f,-1f,0f,0f,-1f},
        new float[]{0f,0f,-1f,-1f,0f,0f,0f,-1f,-1f,0f,0f},
        new float[]{}
    };
    public static float[][] enemyVerticalDirections = new float[][]
    {
        new float[]{},
        new float[]{},
        new float[]{},
        new float[]{},
        new float[]{},
        new float[]{},
        new float[]{0f,1f,1f,0f,0f,0f,0f,1f,0f,1f,1f,1f,0f,0f,0f},
        new float[]{-1f,0f,0f,1f,0f,0f,-1f,0f,0f,1f,1f,0f},
        new float[]{-1f,-1f,0f,0f,1f,1f,1f,0f,0f,1f,1f},
        new float[]{}
    };
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(characterMovement.startGame)
        {
            StartCoroutine(EnemyMovement());
        }
    }
    IEnumerator EnemyMovement()
    {
        if(!isMove)
        {
            isMove = true;
            targetEnemyPosition = gameObject.transform.position + new Vector3(enemyHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1][moveIndex],enemyVerticalDirections[SceneManager.GetActiveScene().buildIndex-1][moveIndex],0f);
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,targetEnemyPosition,lerpSpeed);
            moveIndex++;
            yield return new WaitForSeconds(1f);
            isMove = false;
            if(moveIndex <=enemyHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1].Length){
                StartCoroutine(EnemyMovement());
            }
        }
    }
}
