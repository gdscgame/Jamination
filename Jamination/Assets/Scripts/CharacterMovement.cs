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
    private bool isCrash = false;
    [SerializeField]private float knockBack = 0.3f;
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
        RaycastHit2D hitRight = Physics2D.Raycast(new Vector3(playerTransform.position.x,playerTransform.position.y-0.03f,0f), playerTransform.right , RaycastRange, layerMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(new Vector3(playerTransform.position.x,playerTransform.position.y-0.03f,0f) , -playerTransform.right , RaycastRange, layerMask);
        RaycastHit2D hitForward = Physics2D.Raycast(playerTransform.position , playerTransform.up , RaycastRange, layerMask);
        RaycastHit2D hitBack = Physics2D.Raycast(playerTransform.position , -playerTransform.up , RaycastRange, layerMask);
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        if((Input.GetButtonDown("Horizontal") && isGrounded) || isCrash)
        {
            StartCoroutine(Move());
        }
        else if((Input.GetButtonDown("Vertical") && isGrounded) || isCrash)
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
        Debug.Log("15");
        if(Input.GetButtonDown("Horizontal") && isGrounded && !isWaiting)
        {
            Debug.Log(14);
            if(horizontalMove == 1 && wallRight && isGrounded)
            {
                playerTransform.position += new Vector3(knockBack,0f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(knockBack,0f,0f);
                Debug.Log("1");
                isCrash = true;
            }
            else if(horizontalMove == -1 && wallLeft && isGrounded)
            {
                playerTransform.position += new Vector3(-knockBack,0f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(-knockBack,0f,0f);
                Debug.Log("2");
                isCrash = true;
            }
            else if(isGrounded)
            {
                targetPosition = playerTransform.position + new Vector3(horizontalMove,0f,0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                isMoved = true;
                isCrash =false;
                Debug.Log("3");
            }
        }
        else if(Input.GetButtonDown("Vertical") && isGrounded && !isWaiting)
        {
            if(verticalMove == 1 && wallForward && isGrounded)
            {
                playerTransform.position += new Vector3(0f,knockBack,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(0f,knockBack - 0.2f,0f);
                Debug.Log("4");
                isCrash = true;
            }
            else if(verticalMove == -1 && wallBack && isGrounded)
            {
                playerTransform.position += new Vector3(0f,-knockBack,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(0f,-knockBack + 0.2f,0f);
                Debug.Log("5");
                isCrash = true;
            }
            else if (isGrounded)
            {
                targetPosition = playerTransform.position + new Vector3(0f,verticalMove,0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                isMoved = true;
                isCrash = false;
                Debug.Log("6");
            }
        }
        isWaiting = true;
        Debug.Log("13");
        yield return new WaitForSeconds(1);
        isWaiting = false;
        if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1].Length > directionCount && isMoved && isGrounded || isCrash )
        {
            isCrash = false;
            if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == 1 && wallRight && isGrounded)
            {
                playerTransform.position += new Vector3(knockBack,0f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(knockBack,0f,0f);
                isCrash = false;
                directionCount++;
                Debug.Log("7");
            }
            else if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == -1 && wallLeft && isGrounded)
            {
                playerTransform.position += new Vector3(-knockBack,0f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(-knockBack,0f,0f);
                isCrash = false;
                directionCount++;
                Debug.Log("8");
            }
            else if(Directions.verticalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == 1 && wallForward && isGrounded)
            {
                playerTransform.position += new Vector3(0f,knockBack,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(0f,knockBack - 0.2f,0f);
                isCrash = false;
                directionCount++;
                Debug.Log("9");
            }
            else if(Directions.verticalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == -1 && wallBack && isGrounded)
            {
                playerTransform.position += new Vector3(0f,-knockBack,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(0f,-knockBack + 0.2f,0f);
                isCrash = false;
                directionCount++;
                Debug.Log("10");
            }
            else if(isGrounded)
            {
                targetPosition = playerTransform.position + new Vector3(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount],Directions.verticalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                directionCount++;
                isMoved = false;
                isCrash = false;
                Debug.Log("11");
            }
        }
        if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1].Length <= directionCount)
        {
            directionCount = 0;
            isMoved = false;
        }
    }
    
}
