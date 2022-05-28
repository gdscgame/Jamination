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
    private int layerMask = 1 << 6;
    [SerializeField]private float RaycastRange = 0.5f;
    [SerializeField]private float lerpSpeed = 0.01f;
    Vector3 targetPosition;
    private bool isMoved = false;
    private bool isWaiting = false;
    private bool wallRight = false;
    private bool wallLeft = false;
    private bool wallForward = false;
    private bool wallBack = false;
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
        RaycastHit2D hitRight = Physics2D.Raycast(playerTransform.position , playerTransform.right , RaycastRange, layerMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(playerTransform.position , -playerTransform.right , RaycastRange, layerMask);
        RaycastHit2D hitForward = Physics2D.Raycast(playerTransform.position , playerTransform.forward , RaycastRange, layerMask);
        RaycastHit2D hitBack = Physics2D.Raycast(playerTransform.position , -playerTransform.forward , RaycastRange, layerMask);
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
        if(hitRight.collider != null)
        {
            wallRight = true;
            Debug.Log("WallRight");
        }
        else
        {
            wallRight = false;
        }
        if(hitLeft.collider != null)
        {
            wallLeft = true;
            Debug.Log("WallLeft");
        }
        else
        {
            wallLeft = false;
        }
        if(hitForward.collider != null)
        {
            wallForward = true;
            Debug.Log("WallForward");
        }
        else
        {
            wallForward = false;
        }
        if(hitBack.collider != null)
        {
            wallBack = true;
            Debug.Log("WallBack");
        }
        else
        {
            wallBack = false;
        }
  
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        Debug.Log(other.gameObject.tag);
    }
    IEnumerator Move()
    {
        if(Input.GetButtonDown("Horizontal") && isGrounded && !isWaiting)
        {
            if(horizontalMove == 1 && wallRight)
            {
                playerTransform.position += new Vector3(0.3f,0f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(0.3f,0f,0f);
            }
            else if(horizontalMove == -1 && wallLeft)
            {
                playerTransform.position += new Vector3(-0.3f,0f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(-0.3f,0f,0f);
            }
            else
            {
                targetPosition = playerTransform.position + new Vector3(horizontalMove,0f,0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                isMoved = true;
            }
        }
        else if(Input.GetButtonDown("Vertical") && isGrounded && !isWaiting)
        {
            if(verticalMove == 1 && wallForward)
            {
                playerTransform.position += new Vector3(0f,0.3f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(0f,0.3f,0f);
            }
            else if(verticalMove == -1 && wallBack)
            {
                playerTransform.position += new Vector3(0f,-0.3f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(0f,-0.3f,0f);
            }
            else
            {
                targetPosition = playerTransform.position + new Vector3(0f,verticalMove,0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                isMoved = true;
            }
        }
        isWaiting = true;
        yield return new WaitForSeconds(1);
        isWaiting = false;
        if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1].Length > directionCount && isMoved )
        {
            if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == 1 && wallRight)
            {
                playerTransform.position += new Vector3(0.3f,0f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(0.3f,0f,0f);
            }
            else if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == -1 && wallLeft)
            {
                playerTransform.position += new Vector3(-0.3f,0f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(-0.3f,0f,0f);
            }
            else if(Directions.verticalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == 1 && wallForward)
            {
                playerTransform.position += new Vector3(0f,0.3f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(0f,0.3f,0f);
            }
            else if(Directions.verticalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == -1 && wallBack)
            {
                playerTransform.position += new Vector3(0f,-0.3f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(0f,-0.3f,0f);
            }
            else if(isGrounded)
            {
                targetPosition = playerTransform.position + new Vector3(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount],Directions.verticalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                directionCount++;
                isMoved = false;
            }
        }
        if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1].Length <= directionCount)
        {
            directionCount = 0;
            isMoved = false;
        }
    }
    
}
