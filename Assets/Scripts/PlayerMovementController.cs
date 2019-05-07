using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The maximum speed the player can travel.")]
    private float maxSpeed;

    [SerializeField]
    [Tooltip("The rate at which the player will accelerate when moving.")]
    private float accelerationForce;

    [SerializeField]
    [Tooltip("The force the player will jump with.")]
    private float playerJumpForce;

    [SerializeField]
    [Tooltip("The detection radius for the groundCheck child of the player")]
    private float groundCheckRadius;

    [SerializeField]
    [Tooltip("The transform of the groundCheck GameObject")]
    private Transform groundCheck;

    #region Non-Serialized Fields
    private Rigidbody2D playerRigidBody;
    private float moveInput;
    private bool jumpInput, canJump, playerIsOnGround;
    private float playerMovement;
    private bool canMove;
    private LayerMask whatIsGround;
    #endregion

    //Made accessable to other 
    public bool PlayerCanMove
    {
        get
        {
            return canMove;
        }
        set
        {
            canMove = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializePlayer();
    }

    private void FixedUpdate()
    {
        CheckIfOnGround();

        if (PlayerCanMove)
        {
            MovementHandler();

            if (playerIsOnGround)
                canJump = true;
            else
                canJump = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCanMove)
        {
            GetMovementInput();

            if (playerIsOnGround)
                canJump = true;
            else
                canJump = false;
        }
    }

    private void InitializePlayer()
    {
        whatIsGround = LayerMask.GetMask("Ground");
        groundCheck = gameObject.transform.GetChild(0); //Retrieves the transform component from the child named GroundCheck
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void CheckIfOnGround()
    {
        playerIsOnGround = Physics2D.OverlapCircle(groundCheck.position,
            groundCheckRadius, whatIsGround);
    }

    /// <summary>
    /// Recieves player input
    /// </summary>
    private void GetMovementInput()
    {
        //Initialize Movement Variables
        moveInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        //TODO: Debug
        Debug.Log("jumpInput = " + jumpInput);
        //Regular Jump
        if (jumpInput && playerIsOnGround && canJump)
        {
            //TODO: Debug
            Debug.Log("Regular Jump check_ jumpInput = " + jumpInput);
            JumpHandler();
        }
    }

    /// <summary>
    /// Adds the jump force
    /// </summary>
    private void JumpHandler()
    {
        playerRigidBody.AddForce(Vector2.up * playerJumpForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Acts on player input
    /// </summary>
    private void MovementHandler()
    {
        playerRigidBody.AddForce(Vector2.right * moveInput * accelerationForce);
        Vector2 clampedVelocity = playerRigidBody.velocity;
        clampedVelocity.x = Mathf.Clamp(playerRigidBody.velocity.x, -maxSpeed, maxSpeed);
        playerRigidBody.velocity = clampedVelocity;

        //Sprite Flipping
        if (playerRigidBody.velocity.x > 0.1)
        {
            transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }
        else if (playerRigidBody.velocity.x < -0.1)
        {
            //TODO: Temporarily Disabling Sprite Flipping
            /*
            transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
            */
        }
        else
        {
            transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }
    }
}
