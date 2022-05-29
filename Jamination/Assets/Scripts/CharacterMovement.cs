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
    int animIndex;
    AnimManager animManager;
    public bool isWin;
    [SerializeField] ParticleSystem loseParticle;
    [SerializeField] SpriteRenderer spriteRenderer;

    private int portalIndex = 0;
    private bool isOnPortal = false;
    private bool groundCheck = false;
    private bool isOnExitPortal = false;
    public bool startGame = false;
    private bool crashControl = false;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject spaceImage;
    private int portalsound = 1;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0F;
        animManager = GameObject.Find("AnimManager").GetComponent<AnimManager>();
        animManager.SetAnim(animIndex);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isOnPortal);
        RaycastHit2D hitRight = Physics2D.Raycast(new Vector3(playerTransform.position.x,playerTransform.position.y-0.03f,0f), playerTransform.right , RaycastRange, layerMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(new Vector3(playerTransform.position.x,playerTransform.position.y-0.03f,0f) , -playerTransform.right , RaycastRange, layerMask);
        RaycastHit2D hitForward = Physics2D.Raycast(playerTransform.position , playerTransform.up , RaycastRange, layerMask);
        RaycastHit2D hitBack = Physics2D.Raycast(playerTransform.position , -playerTransform.up , RaycastRange, layerMask);
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        if(Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical") || Input.GetKeyDown(KeyCode.Space))
        {
            startGame = true;
            animIndex++;
            animManager.SetAnim(animIndex);
        }
        if(crashControl)
        {
            gameManager.Crash();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        if(isOnPortal)
        {
            StartCoroutine(OnPortal());
        }
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
        if(isOnExitPortal)
        {
            groundCheck = false;
        }
        if(doubleAction.bufAmount == 0 && spaceImage != null){
            spaceImage.SetActive(false);
        }
  
    }
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Award"){
            isGrounded = false;
            gameManager.Win();
            animManager.ChangeLevel();
            StartCoroutine(WaitNextStage());
            isWin = true;
        }
        if(other.gameObject.tag == "Ground")
        {
            groundCheck = true;
        }
        if(other.gameObject.tag == "Enemy")
        {
            GameOver();
        }
    }
    IEnumerator WaitNextStage()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);


    }
    void GameOver()
    {
        if(!isOnPortal && !isOnExitPortal)
        {
            gameManager.GameOver();
            loseParticle.Play();
            spriteRenderer.enabled = false;
            StartCoroutine(WaitGameOver());
        }
    }
    IEnumerator WaitGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        Restart();
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            if(groundCheck)
            {
                isGrounded = true;
            }
            else 
            {
                isGrounded = false;
                isOnExitPortal = false;
                isOnPortal = false;
            }
            if(!isWin){
                GameOver();
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        if(other.gameObject.tag == "Ground")
        {
        
            if(playerTransform.position.x <= Portal.portalEntry[SceneManager.GetActiveScene().buildIndex-1].x + 0.5f && Portal.portalEntry[SceneManager.GetActiveScene().buildIndex-1].x - 0.5f <= playerTransform.position.x &&SceneManager.GetActiveScene().buildIndex != 10)
            {
                if(playerTransform.position.y <= Portal.portalEntry[SceneManager.GetActiveScene().buildIndex-1].y + 0.5f && Portal.portalEntry[SceneManager.GetActiveScene().buildIndex-1].y - 0.5f <= playerTransform.position.y)
                {
                    portalIndex = SceneManager.GetActiveScene().buildIndex-1;
                    isOnPortal = true;
                    isOnExitPortal = false;
                }
            }
            if(playerTransform.position.x <= Portal.portalExit[SceneManager.GetActiveScene().buildIndex-1].x + 0.5f && Portal.portalExit[SceneManager.GetActiveScene().buildIndex-1].x - 0.5f <= playerTransform.position.x &&SceneManager.GetActiveScene().buildIndex != 10)
            {
                if(playerTransform.position.y <= Portal.portalExit[SceneManager.GetActiveScene().buildIndex-1].y + 0.5f && Portal.portalExit[SceneManager.GetActiveScene().buildIndex-1].y - 0.5f <= playerTransform.position.y)
                {
                    isOnPortal = false;
                    isOnExitPortal = true;
                }
            }
            if(SceneManager.GetActiveScene().buildIndex == 10)
            {
                if(playerTransform.position.x <= Portal.portalEntry[SceneManager.GetActiveScene().buildIndex-1].x + 0.5f && Portal.portalEntry[SceneManager.GetActiveScene().buildIndex-1].x - 0.5f <= playerTransform.position.x)
                {
                    if(playerTransform.position.y <= Portal.portalEntry[SceneManager.GetActiveScene().buildIndex-1].y + 0.5f && Portal.portalEntry[SceneManager.GetActiveScene().buildIndex-1].y - 0.5f <= playerTransform.position.y)
                        {
                            portalIndex = SceneManager.GetActiveScene().buildIndex-1;
                            isOnPortal = true;
                            isOnExitPortal = false;
                        }
                }
                else if(playerTransform.position.x <= Portal.portalEntry[SceneManager.GetActiveScene().buildIndex].x + 0.5f && Portal.portalEntry[SceneManager.GetActiveScene().buildIndex].x - 0.5f <= playerTransform.position.x)
                {
                    if(playerTransform.position.y <= Portal.portalEntry[SceneManager.GetActiveScene().buildIndex].y + 0.5f && Portal.portalEntry[SceneManager.GetActiveScene().buildIndex].y - 0.5f <= playerTransform.position.y)
                        {
                            portalIndex = SceneManager.GetActiveScene().buildIndex;
                            isOnPortal = true;
                            isOnExitPortal = false;
                        }
                }
                if(playerTransform.position.x <= Portal.portalExit[SceneManager.GetActiveScene().buildIndex-1].x + 0.5f && Portal.portalExit[SceneManager.GetActiveScene().buildIndex-1].x - 0.5f <= playerTransform.position.x)
                {
                    if(playerTransform.position.y <= Portal.portalExit[SceneManager.GetActiveScene().buildIndex-1].y + 0.5f && Portal.portalExit[SceneManager.GetActiveScene().buildIndex-1].y - 0.5f <= playerTransform.position.y)
                        {
                            isOnExitPortal = true;
                            isOnPortal = false;
                        }
                }
                else if(playerTransform.position.x <= Portal.portalExit[SceneManager.GetActiveScene().buildIndex].x + 0.5f && Portal.portalExit[SceneManager.GetActiveScene().buildIndex].x - 0.5f <= playerTransform.position.x)
                {
                    if(playerTransform.position.y <= Portal.portalExit[SceneManager.GetActiveScene().buildIndex].y + 0.5f && Portal.portalExit[SceneManager.GetActiveScene().buildIndex].y - 0.5f <= playerTransform.position.y)
                        {
                            isOnExitPortal = true;
                            isOnPortal = false;
                        }
                }
            }
        }
    }
    IEnumerator Move()
    {

        if(Input.GetButtonDown("Horizontal") && isGrounded && !isWaiting)
        {
            startGame = true;
            if(horizontalMove == 1 && wallRight && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(knockBack,0f,0f);
                yield return new WaitForSeconds(0.3f);
                playerTransform.position -= new Vector3(knockBack,0f,0f);
                isWaiting = false;
                isCrash = true;
                gameManager.Crash();
            }
            else if(horizontalMove == -1 && wallLeft && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(-knockBack,0f,0f);
                yield return new WaitForSeconds(0.3f);
                playerTransform.position -= new Vector3(-knockBack,0f,0f);
                isWaiting = false;
                isCrash = true;
                gameManager.Crash();
            }
            else if(isGrounded)
            {
                targetPosition = playerTransform.position + new Vector3(horizontalMove,0f,0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                isMoved = true;
                isCrash =false;
                gameManager.Move();
            }
        }
        else if(Input.GetButtonDown("Vertical") && isGrounded && !isWaiting)
        {
            startGame = true;
            if(verticalMove == 1 && wallForward && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(0f,knockBack,0f);
                yield return new WaitForSeconds(0.3f);
                playerTransform.position -= new Vector3(0f,knockBack,0f);
                isWaiting = false;
                isCrash = true; 
                gameManager.Crash();
            }
            else if(verticalMove == -1 && wallBack && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(0f,-knockBack,0f);
                yield return new WaitForSeconds(0.3f);
                playerTransform.position -= new Vector3(0f,-knockBack,0f);
                isWaiting = false;
                isCrash = true;
                gameManager.Crash();
            }
            else if (isGrounded)
            {
                targetPosition = playerTransform.position + new Vector3(0f,verticalMove,0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                isMoved = true;
                isCrash = false;
                gameManager.Move();
            }
        }
        isWaiting = true;

        yield return new WaitForSeconds(0.6f);

        isWaiting = false;
        if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1].Length > directionCount && isMoved && isGrounded || isCrash )
        {
            isCrash = false;
            if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == 1 && wallRight && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(knockBack,0f,0f);
                yield return new WaitForSeconds(0.3f);
                playerTransform.position -= new Vector3(knockBack,0f,0f);
                isWaiting = false;
                isCrash = false;
                directionCount++;
                gameManager.Crash();
            }
            else if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == -1 && wallLeft && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(-knockBack,0f,0f);
                yield return new WaitForSeconds(0.3f);
                playerTransform.position -= new Vector3(-knockBack,0f,0f);
                isWaiting = false;
                isCrash = false;
                directionCount++;
                gameManager.Crash();
            }
            else if(Directions.verticalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == 1 && wallForward && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(0f,knockBack,0f);
                yield return new WaitForSeconds(0.3f);
                playerTransform.position -= new Vector3(0f,knockBack,0f);
                isWaiting = false;
                isCrash = false;
                directionCount++;
                gameManager.Crash();
            }
            else if(Directions.verticalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount] == -1 && wallBack && isGrounded)
            {
                isWaiting = true;
                playerTransform.position += new Vector3(0f,-knockBack,0f);
                yield return new WaitForSeconds(0.3f);
                playerTransform.position -= new Vector3(0f,-knockBack,0f);
                isWaiting = false;
                isCrash = false;
                directionCount++;
                gameManager.Crash();
            }
            else if(isGrounded && !isOnPortal)
            {
                targetPosition = playerTransform.position + new Vector3(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount],Directions.verticalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                directionCount++;
                isMoved = false;
                isCrash = false;
                gameManager.Move();
            }
            else
            {
                directionCount++;
            }
        }
        if(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1].Length <= directionCount)
        {
            directionCount = 0;
            isMoved = false;
        }
    }
    IEnumerator DoubleActions()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isWaiting && isGrounded && doubleAction.bufAmount > 0)
        {
            if(DoubleAction.extraHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1][2] == 0f)
            {
                isWaiting = true;
                targetPosition = playerTransform.position + new Vector3(DoubleAction.extraHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],DoubleAction.extraVerticalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                gameManager.Move();
                yield return new WaitForSeconds(0.3f);
                actionCount += 1;
                targetPosition = playerTransform.position + new Vector3(DoubleAction.extraHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],DoubleAction.extraVerticalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                actionCount = 0;
                gameManager.Move();
                yield return new WaitForSeconds(0.3F);
                targetPosition = playerTransform.position + new Vector3(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount],Directions.verticalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                gameManager.Move();
                directionCount++;
                isMoved = false;
                isCrash = false;
                isWaiting = false;
                doubleAction.bufAmount = 0;
            }
            if(DoubleAction.extraHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1][2] != 0f)
            {
                isWaiting = true;
                targetPosition = playerTransform.position + new Vector3(DoubleAction.extraHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],DoubleAction.extraVerticalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                gameManager.Move();
                yield return new WaitForSeconds(0.3f);
                actionCount += 1;
                targetPosition = playerTransform.position + new Vector3(DoubleAction.extraHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],DoubleAction.extraVerticalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                gameManager.Move();
                yield return new WaitForSeconds(0.3f);
                actionCount += 1;
                targetPosition = playerTransform.position + new Vector3(DoubleAction.extraHorizontalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],DoubleAction.extraVerticalDirections[SceneManager.GetActiveScene().buildIndex-1][actionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                gameManager.Move();
                actionCount = 0;
                yield return new WaitForSeconds(0.3F);
                targetPosition = playerTransform.position + new Vector3(Directions.horizontalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount],Directions.verticalDirections[SceneManager.GetActiveScene().buildIndex-1][directionCount],0f);
                playerTransform.position = Vector3.Lerp(playerTransform.position,targetPosition,lerpSpeed);
                gameManager.Move();
                directionCount++;
                isMoved = false;
                isCrash = false;
                isWaiting = false;
                doubleAction.bufAmount = 0;
            }

        }
    }
    IEnumerator OnPortal()
    {
        if(isOnPortal)
        {
            if(portalsound == 1){
                portalsound--;
                gameManager.Portal();
            }
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(1f);
            playerTransform.position = Portal.portalExit[portalIndex];
            spriteRenderer.enabled = true;
        }
    }
    
}
