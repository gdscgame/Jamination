using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
    Transform playerTransform;
    float horizontalMove;
    float verticalMove;
    bool isGrounded = true;
    Rigidbody2D rb;
    public Directions directions;
    private int directionCount = 0;
    [SerializeField]private float lerpSpeed = 0.01f;
    Vector3 targetPosition;
    private bool isMoved = false;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0F;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        if(Input.GetButtonDown("Horizontal") && isGrounded)
        {
            StartCoroutine(Move());
        }
        else if(Input.GetButtonDown("Vertical") && isGrounded)
        {
            StartCoroutine(Move());
        }
        
        Debug.Log(isMoved);
        
        // if(!isGrounded && playerTransform.localScale.x > 0f && playerTransform.localScale.y > 0)
        // {
        //     playerTransform.localScale += new Vector3(-0.2f * Time.deltaTime,-0.2f * Time.deltaTime ,0f);
        // }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            isGrounded = false;
            Debug.Log("Exit");
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            isGrounded = true;
            Debug.Log("Enter");
        }
    }
    IEnumerator Move()
    {
        if(Input.GetButtonDown("Horizontal") && isGrounded)
        {
            targetPosition = playerTransform.position + new Vector3(horizontalMove,0f,0f);
            Debug.Log(targetPosition);
            playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
            isMoved = true;
        }
        else if(Input.GetButtonDown("Vertical") && isGrounded)
        {
            targetPosition = playerTransform.position + new Vector3(0f,verticalMove,0f);
            Debug.Log(targetPosition);
            playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
            isMoved = true;
        }
        yield return new WaitForSeconds(1);
        if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1].Length > directionCount && isMoved )
        {
            targetPosition = playerTransform.position + new Vector3(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount],Directions.verticalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount],0f);
            Debug.Log("Done");
            playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
            directionCount++;
            isMoved = false;
        }
        else
        {
            directionCount = 0;
            isMoved = false;
        }
    }
    
}
