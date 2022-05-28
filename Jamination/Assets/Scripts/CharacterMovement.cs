using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Transform playerTransform;
    float horizontalMove;
    float verticalMove;
    bool isGrounded = true;
    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        if(Input.GetButtonDown("Horizontal") && isGrounded)
        {
            playerTransform.position += new Vector3(horizontalMove,0f,0f);
        }
        else if(Input.GetButtonDown("Vertical") && isGrounded)
        {
            playerTransform.position += new Vector3(0f,verticalMove,0f);
        }
        
        if(!isGrounded)
        {
            playerTransform.localScale -= new Vector3(0.2f,0.2f,0f);
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
}
