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
    private int actionCount;
    public DoubleAction doubleAction;
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
        Debug.Log(isGrounded);
        RaycastHit2D hitRight = Physics2D.Raycast(new Vector3(playerTransform.position.x,playerTransform.position.y-0.03f,0f), playerTransform.right , RaycastRange, layerMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(new Vector3(playerTransform.position.x,playerTransform.position.y-0.03f,0f) , -playerTransform.right , RaycastRange, layerMask);
        RaycastHit2D hitForward = Physics2D.Raycast(playerTransform.position , playerTransform.up , RaycastRange, layerMask);
        RaycastHit2D hitBack = Physics2D.Raycast(playerTransform.position , -playerTransform.up , RaycastRange, layerMask);
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        if(Input.GetKeyDown(KeyCode.Space) && !isWaiting && isGrounded)
        {
            StartCoroutine(DoubleActions());
        }
        if((Input.GetButtonDown("Horizontal") && isGrounded && !isWaiting) || isCrash)
        {
            StartCoroutine(Move());
        }
        else if((Input.GetButtonDown("Vertical") && isGrounded && !isWaiting) || isCrash)
        {
            StartCoroutine(Move());
        }
        if(hitRight.collider != null)
        {
            wallRight = true;
        }
        else
        {
            wallRight = false;
        }
        if(hitLeft.collider != null)
        {
            wallLeft = true;
        }
        else
        {
            wallLeft = false;
        }
        if(hitForward.collider != null)
        {
            wallForward = true;
        }
        else
        {
            wallForward = false;
        }
        if(hitBack.collider != null)
        {
            wallBack = true;
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
    }
    IEnumerator Move()
    {
        if(Input.GetButtonDown("Horizontal") && isGrounded && !isWaiting)
        {
            if(horizontalMove == 1 && wallRight && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(knockBack,0f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(knockBack,0f,0f);
                isWaiting = false;
                isCrash = true;
            }
            else if(horizontalMove == -1 && wallLeft && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(-knockBack,0f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(-knockBack,0f,0f);
                isWaiting = false;
                isCrash = true;
            }
            else if(isGrounded)
            {
                targetPosition = playerTransform.position + new Vector3(horizontalMove,0f,0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                isMoved = true;
                isCrash =false;
            }
        }
        else if(Input.GetButtonDown("Vertical") && isGrounded && !isWaiting)
        {
            if(verticalMove == 1 && wallForward && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(0f,knockBack,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(0f,knockBack - 0.2f,0f);
                isWaiting = false;
                isCrash = true;
            }
            else if(verticalMove == -1 && wallBack && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(0f,-knockBack,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(0f,-knockBack + 0.2f,0f);
                isWaiting = false;
                isCrash = true;
            }
            else if (isGrounded)
            {
                targetPosition = playerTransform.position + new Vector3(0f,verticalMove,0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                isMoved = true;
                isCrash = false;
            }
        }
        isWaiting = true;
        yield return new WaitForSeconds(1);
        isWaiting = false;
        if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1].Length > directionCount && isMoved && isGrounded || isCrash )
        {
            isCrash = false;
            if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == 1 && wallRight && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(knockBack,0f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(knockBack,0f,0f);
                isWaiting = false;
                isCrash = false;
                directionCount++;
            }
            else if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == -1 && wallLeft && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(-knockBack,0f,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(-knockBack,0f,0f);
                isWaiting = false;
                isCrash = false;
                directionCount++;
            }
            else if(Directions.verticalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == 1 && wallForward && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(0f,knockBack,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(0f,knockBack - 0.2f,0f);
                isWaiting = false;
                isCrash = false;
                directionCount++;
            }
            else if(Directions.verticalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == -1 && wallBack && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(0f,-knockBack,0f);
                yield return new WaitForSeconds(0.5f);
                playerTransform.position -= new Vector3(0f,-knockBack + 0.2f,0f);
                isWaiting = false;
                isCrash = false;
                directionCount++;
            }
            else if(isGrounded)
            {
                targetPosition = playerTransform.position + new Vector3(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount],Directions.verticalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                directionCount++;
                isMoved = false;
                isCrash = false;
            }
        }
        if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1].Length <= directionCount)
        {
            directionCount = 0;
            isMoved = false;
        }
    }
    IEnumerator DoubleActions(){
        if(Input.GetKeyDown(KeyCode.Space) && !isWaiting && isGrounded)
        {
            if(DoubleAction.extraHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1][2] == 0f)
            {
                isWaiting = true;
                targetPosition = playerTransform.position + new Vector3(DoubleAction.extraHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],DoubleAction.extraVerticalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                yield return new WaitForSeconds(0.5f);
                actionCount += 1;
                targetPosition = playerTransform.position + new Vector3(DoubleAction.extraHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],DoubleAction.extraVerticalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                isWaiting = false;
                actionCount = 0;
                doubleAction.bufAmount = 0;
            }
            if(DoubleAction.extraHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1][2] != 0f)
            {
                isWaiting = true;
                targetPosition = playerTransform.position + new Vector3(DoubleAction.extraHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],DoubleAction.extraVerticalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                yield return new WaitForSeconds(0.5f);
                actionCount += 1;
                targetPosition = playerTransform.position + new Vector3(DoubleAction.extraHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],DoubleAction.extraVerticalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                yield return new WaitForSeconds(0.5f);
                actionCount += 1;
                targetPosition = playerTransform.position + new Vector3(DoubleAction.extraHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],DoubleAction.extraVerticalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                actionCount = 0;
                isWaiting = false;
                doubleAction.bufAmount = 0;
            }

        }
    }
    
}
