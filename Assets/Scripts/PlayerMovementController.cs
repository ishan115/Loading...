using System;
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

    [SerializeField]
    [Tooltip("The amount of time to wait before respawning.")]
    private float spawnDelay;

    #region Non-Serialized Fields
    private Rigidbody2D playerRigidBody;
    private Renderer playerSpriteRend;
    private Collider2D playerCollider;
    private float moveInput;
    private bool jumpInput, canJump, playerIsOnGround;
    private float playerMovement;
    private bool canMove;
    private bool inDialogue;
    private bool isAlive;
    private LayerMask whatIsGround;
    private CheckpointController currentCheckpoint;
    private Transform currentCheckpointLocation, spawnPointLocation;
    private GameObject spawnPoint;
    private Animator playerAnimator;
    #endregion

    #region Properties
    public bool InDialogue
    {
        get
        {
            return inDialogue;
        }
        set
        {
            inDialogue = value;
        }
    }

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

    public CheckpointController CurrentCheckpoint
    {
        get
        {
            return currentCheckpoint;
        }
        set
        {
            if (currentCheckpoint == null)
            {
                currentCheckpoint = value;
                currentCheckpoint.IsActivated = true;
            }
            else
            {
                currentCheckpoint.IsActivated = false;
                currentCheckpoint = value;
                currentCheckpoint.IsActivated = true;
            }
        }
    }
    #endregion

    /// <summary>
    /// Timer to create a delay between when the player dies and when they respawn
    /// </summary>
    /// <returns></returns>
    IEnumerator RespawnPlayerWithDelay()
    {
        isAlive = false;
        CheckForCheckpoint();
        playerSpriteRend.enabled = false;
        playerRigidBody.velocity = Vector2.zero;
        yield return new WaitForSecondsRealtime(spawnDelay);
        isAlive = true;
        //playerCollider.enabled = true;
        playerSpriteRend.enabled = true;
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
        //TODO: Debug.Log for player movement
        Debug.Log($"playerIsOnGround == {playerIsOnGround}");
        Debug.Log($"player can move == {PlayerCanMove}");

        CheckForPlayerDeath();

        playerAnimator.SetFloat("Speed", Mathf.Abs(playerRigidBody.velocity.x));

        if (isAlive)
        {
            PlayerCanMove = true;
        }
        if (inDialogue || !isAlive)
        {
            PlayerCanMove = false;
        }

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
        isAlive = true;
        // Game Object References
        whatIsGround = LayerMask.GetMask("Ground");
        groundCheck = gameObject.transform.GetChild(0); //Retrieves the transform component from the child named GroundCheck
        playerRigidBody = GetComponent<Rigidbody2D>();
        spawnPoint = GameObject.Find("SpawnPoint");
        spawnPointLocation = spawnPoint.transform;
        playerSpriteRend = GetComponent<Renderer>();
        playerAnimator = GetComponent<Animator>();
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
        jumpInput = Input.GetButtonDown("Fire1"); // Jump is now fire1
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
            //transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }
        else
        {
            //transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "KillZone":
                isAlive = false;
                break;

            default:
                break;
        }
    }

    private void CheckForPlayerDeath()
    {
        if (!isAlive)
            StartCoroutine(RespawnPlayerWithDelay());
    }


    private void CheckForCheckpoint()
    {
        if (CurrentCheckpoint == null)
            transform.position = spawnPointLocation.position;
        else
        {
            //playerSpriteRend.enabled = false;
            //playerCollider.enabled = false;
            //playerRigidBody.velocity = Vector2.zero;
            transform.position = CurrentCheckpoint.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    //********THIS IS FOR WHEN FOOTSTEP SFX ARE ADDED AND READY**************
    //private void AudioHandler()
    //{
    //    if (myRigidBody.velocity.x > 0.1 && grounded)
    //    {
    //        FootstepFX.UnPause();
    //    }
    //    else if (myRigidBody.velocity.x < -0.1 && grounded)
    //    {
    //        FootstepFX.UnPause();
    //    }
    //    else if (myRigidBody.velocity.x == 0 || !grounded)
    //    {
    //        FootstepFX.Pause();
    //    }
    //}
}
