using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class ImprovedPlayerMovement : MonoBehaviour
{
    #region References
    [Header("For Movement")]
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float airMovementSpeed = 10;
    private float horizontalInput;
    private bool facingLeft = true;
    private bool isMoving;
    private bool inputAllowed = true;

    [Header("For Jumping")]
    [SerializeField] float jumpPower = 8;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Vector2 groundCheckSize;
    private bool grounded;
    private bool canJump;
    private bool downJump;

    [Header("For Wall Sliding")]
    [SerializeField] float slidingSpeed = 0;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] Vector2 wallCheckSize;
    private bool walled;
    private bool isSliding;

    [Header("For Wall Jumping")]
    [SerializeField] float wallJumpPower = 12;
    [SerializeField] float wallJumpDirection = 1;
    [SerializeField] Vector2 wallJumpAngle;

    [Header("Other")]
    //commented out animator for future use,
    //felt like it made sense to add it now
    //and fill in animations later when we have them
    public Animator animator;
    private Rigidbody2D rb;
    public bool hasBall = false;
    private Vector2 respawnPoint;
    public GameObject player;
    private bool resetSound;
    #endregion

    #region Basics (awake, update, etc.)
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        wallJumpAngle.Normalize();
        WelcomeMessage();
        respawnPoint = transform.position;
    }

    private void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
        animator.SetBool("Walled", walled);
        animator.SetBool("Grounded", grounded);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Wall"))
        {
            //print("walled");
        }
        Inputs();
        CheckWorld();
    }

    private void FixedUpdate()
    {
        Jump();
        Movement();
        WallJump();
        WallSlide();
        //AnimationControl();
    }
    #endregion

    #region Game State (inputs, world check)
    
    void Inputs()
    {
        if (inputAllowed)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
        }
        if ((Input.GetKeyDown(KeyCode.Space)||(Input.GetKeyDown(KeyCode.W))||(Input.GetKeyDown(KeyCode.UpArrow))) && (grounded||walled||isSliding))
        {
            canJump = true;
            StartCoroutine(JumpDelayCoroutine());
        }
        if (Input.GetKey(KeyCode.S))
        {
            downJump = true;
        }
        else
        {
            downJump = false;
        }
    }

    void CheckWorld()
    {
        grounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);
        walled = Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, wallLayer);
    }
    #endregion

    #region Movement
    void Movement()
    {
        AudioManager am = FindObjectOfType<AudioManager>();
        //for animation, to be added later
        if(horizontalInput != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (grounded)
        {
            rb.velocity = new Vector2(horizontalInput * movementSpeed, rb.velocity.y);
            if (horizontalInput != 0 && !am.IsPlaying("Running"))
            {
                //am.Play("Running");
                print("should be playing running sound");
            }
            else if (horizontalInput == 0)
            {
                //am.Stop("Running");
            }
            
        }
        //airborne movement
        else if(!grounded && !isSliding && horizontalInput != 0)
        {
            rb.AddForce(new Vector2(airMovementSpeed * horizontalInput,0));
            if (Mathf.Abs(rb.velocity.x) > movementSpeed)
            {
                rb.velocity = new Vector2(horizontalInput * movementSpeed, rb.velocity.y);
            }
        }
        
        
        if(horizontalInput < 0 && !facingLeft)
        {
            Flip();
        }
        else if (horizontalInput > 0 && facingLeft)
        {
            Flip();
        }
    }
    #endregion

    #region Jumping
    void Jump()
    {
        if(canJump && grounded)
        {
            resetSound = false;
            canJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            print("regular jump");
            FindObjectOfType<AudioManager>().Play("Jump");

        }
    } 
    
    void WallJump()
    {
        if ((isSliding||walled) && canJump && !downJump && rb.velocity.y <= 0)
        {
            resetSound = false;
            print("wall jump");
            //the below section is basically identical to Flip(), except Flip() is locked if you're sliding.
            //since the WallJump can only be activated whilst sliding, I've had to copy the code.
            //if I just call Flip(), you can flip the sprite whilst stuck to the wall which doesn't make sense.
            animator.Play("Player_Jump");
            
            facingLeft = !facingLeft;
            if (facingLeft)
            {
                transform.localScale = Vector3.one;
            }
            else if (!facingLeft)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            //this bit is the main part of the function, providing the wall jump

            inputAllowed = false;
            horizontalInput = 0;
            rb.AddForce(new Vector2(wallJumpPower * wallJumpDirection * wallJumpAngle.x, wallJumpPower * wallJumpAngle.y), ForceMode2D.Impulse);
            
            StartCoroutine("WallJumpDelayCoroutine");

            FindObjectOfType<AudioManager>().Play("Jump");
            canJump = false;
            //this bit is in a different position to Flip, and must be at the end.
            //if it's BEFORE the force is applied, the direction is incorrect.
            wallJumpDirection *= -1;
        }
        else if ((isSliding || walled) && canJump && downJump)
        {
            resetSound = false;
            animator.Play("Player_Jump");
            facingLeft = !facingLeft;
            if (facingLeft)
            {
                transform.localScale = Vector3.one;
            }
            else if (!facingLeft)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            //this bit is the main part of the function, providing the wall jump
            rb.AddForce(new Vector2(wallJumpPower * wallJumpDirection * wallJumpAngle.x, wallJumpPower * -wallJumpAngle.y), ForceMode2D.Impulse);
            print("down jump");
            FindObjectOfType<AudioManager>().Play("Jump");
            canJump = false;

            //this bit is in a different position to Flip, and must be at the end.
            //if it's BEFORE the force is applied, the direction is incorrect.
            wallJumpDirection *= -1;
        }
    }
#endregion

    #region Sliding
    void WallSlide()
    {
        if (walled && !grounded && rb.velocity.y < 0)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
            //FindObjectOfType<AudioManager>().Stop("Splat");
        }

        if (isSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, slidingSpeed);
        }
    }
    
    #endregion

    #region Respawn
    private void Respawn()
    {
        hasBall = false;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        StartCoroutine("RespawnCoroutine");
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public IEnumerator RespawnCoroutine()
    {
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.75f);
        transform.position = respawnPoint;
        GetComponent<Renderer>().enabled = true;
    }
    #endregion

    #region Animation (WIP - currently only Flip() is implemented, to flip sprite based on movement direction)  
    //void AnimationControl()
    //{
    //anim.SetBool("isMoving", isMoving);
    //anim.SetBool("isGrounded", grounded);
    //}

    
    
    void Flip()
    {
        if (!isSliding)
        {
            wallJumpDirection *= -1;
            facingLeft = !facingLeft;
            if (facingLeft)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
    #endregion

    #region Other

    //called via animation event in the Player Animator
    void SplatSound()
    {
        if (!resetSound)
        {
            FindObjectOfType<AudioManager>().Play("Splat");
            resetSound = true;
        }
    }
    //called via animation event in the Player Animator
    void RunningSound()
    {
        FindObjectOfType<AudioManager>().Play("Running");
    }

    //called via animation event in the Player Animator
    void RunningSoundCancel()
    {
        FindObjectOfType<AudioManager>().Stop("Running");
    }
    
    public IEnumerator JumpDelayCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        canJump = false;
    }
    public IEnumerator WallJumpDelayCoroutine()
    {
        inputAllowed = false;
        print("controls locked");
        yield return new WaitForSeconds(0.5f);
        print("controls unlocked");
        inputAllowed = true;
    }
    
    //shows the player hitboxes in scene view - blue for ground check and red for wall check
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(groundCheckPoint.position, groundCheckSize);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(wallCheckPoint.position, wallCheckSize);
    }

    private void WelcomeMessage()
    {
        print("Welcome to Fetch Quest!");
        print("To move Slime Dog, use your WASD or arrow keys.");
        print("To jump, press SPACE.");
        print("It looks like you can't jump high enough to reach the next platform... I wonder how sticky your slimy body is?");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ball")
        {
            hasBall = true;
        }
        else if (collision.tag == "Owner" && !hasBall)
        {
            FindObjectOfType<Owner>().EnableSpeechBubble();
        }
        else if(collision.tag == "Owner" && hasBall == true)
        {
            FindObjectOfType<AudioManager>().Play("CompleteSound");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            hasBall = false;
            respawnPoint = transform.position;
        }
        else if (collision.tag == "FallZone")
        {
            FindObjectOfType<AudioManager>().Play("Death");
            Respawn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Owner" && !hasBall)
        {
            FindObjectOfType<Owner>().DisableSpeechBubble();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            player.transform.parent = other.gameObject.transform;
        }
        if (other.gameObject.CompareTag("TriggeredPlatform"))
        {
            player.transform.parent = other.gameObject.transform;
            FindObjectOfType<TriggeredPlatform>().StartPlatform();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            player.transform.parent = null;
        }

        if (other.gameObject.CompareTag("TriggeredPlatform"))
        {
            player.transform.parent = null;
        }
    }
    #endregion
}
