using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //keeps variable private but allows it to be manually changed
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    private ParticleSystem confetti;
    public bool grounded;
    public bool walled;
    public bool leftFacing;

    public float lastGroundedTime;
    public float lastJumpTime;
    public float jumpBufferTime;
    public float jumpCoyoteTime;

    public bool isJumping;

    public Transform frontCheck;
    public bool wallSliding;
    public float wallSlidingSpeed;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        confetti = GetComponent<ParticleSystem>();
        WelcomeMessage();
    }
    
    void FixedUpdate()
    {
#region Timer
        
        lastGroundedTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;

#endregion

#region Jump

        if(lastGroundedTime > 0 && lastJumpTime > 0 && isGrounded())
        {
            Jump();
        }

#endregion
    }

    public void OnJump()
    {
        lastJumpTime = jumpBufferTime;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput*speed, body.velocity.y);
        grounded = isGrounded();

        //face LEFT when pressing left, face RIGHT when pressing right
#region Facing Direction
        if (horizontalInput < -0.01f)
        {
            transform.localScale = Vector3.one;
            leftFacing = true;
        }
        else if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            leftFacing = false;
        }

        #endregion

        walled = onWall(leftFacing);

        //if (walled == true && isGrounded() == false && horizontalInput != 0 && !(Input.GetKey(KeyCode.Space)))
        //{
           // wallSliding = true;
        //}
        //else
        //{
            //wallSliding = false;
        //}

        //if (wallSliding)
        //{
            //body.velocity = new Vector2(body.velocity.x, -wallSlidingSpeed); //Mathf.Clamp(body.velocity.y, -wallSlidingSpeed, float.MaxValue));
        //}

        if((onWall(leftFacing) && horizontalInput < 0) || (onWall(!leftFacing) && horizontalInput > 0))
        {
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, 0, 0));
            print("facing left is " + onWall(leftFacing));
        }
        else
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y);
        }
        
        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            Jump();
        }

        if (body.velocity.y <= 0)
        {
            isJumping = false;
        }

        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall(leftFacing) && !isGrounded())
            {
                body.gravityScale = 0.2f;
                body.velocity = Vector2.zero;
                //body.velocity = new Vector2(0, body.velocity.y);
                print("touching wall");
            }
            else
            {
                body.gravityScale = 1;
            }

            if (Input.GetKey(KeyCode.Space) && !isJumping)
            {
                //WallJump();
                Jump();
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }


    }

    //private void WallJump()
    //{
        //jumpPower = 16;
        //Jump();
        //jumpPower = 8;
    //}

    private void Jump()
    {
        if (isGrounded())
        {
            
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            //body.AddForce(Vector2.up * jumpPower);
            //lastGroundedTime = 0;
            //lastJumpTime = 0;
            //isJumping = true;
            print("successful ground jump");
            //lastGroundedTime = jumpCoyoteTime;
        }
        else if (onWall(leftFacing) && !isGrounded())
        {
            if(horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 10);
            }
            wallJumpCooldown = 0;
        }   
    }

    private void OnTriggerEnter2D(Collider2D goal)
    {
        print("Goal reached!");
        confetti.Play();
    }

    //raycasts to check if player is touching a "GROUND LAYER" object
    public bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        
        //returns TRUE if collider is NOT null, so if player IS touching ground
        return raycastHit.collider != null;
    }

    public bool onWall(bool facingLeft)
    {
        RaycastHit2D raycastHit;

        if (facingLeft)
        {
            raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 0.01f, wallLayer);
        }
        else
        {
            raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, (Vector2.left * -1), 0.01f, wallLayer);
        }
        
        return raycastHit.collider != null;
    }

    private void WelcomeMessage()
    {
        print("Welcome to Fetch Quest!");
        print("To move Slime Dog, use your WASD or arrow keys.");
        print("To jump, press SPACE.");
        print("It looks like you can't jump high enough to reach the next platform... I wonder how sticky your slimey body is?");
    }


}
