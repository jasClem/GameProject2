using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Public Variables 
    //(Can be changed in Unity)
    public float maxSpeed = 4;
    public float jumpForce = 500;
    public float minHeight, maxHeight;
    public float sleepyTime;
    public float knockBackForce;
    public bool canMove;

    //Attack Boxes
    //public GameObject attack1Box, attack2Box, attack3Box, airAttackBox;
    //public Sprite attack1HitFrame, attack2HitFrame, attack3HitFrame, airAttackHitFrame;
    //public GameObject blockBox;


    //Set Ups
    private Rigidbody rb;
    private Animator anim;
    private Transform groundCheck;
    AnimatorStateInfo currentStateInfo;

    //SpriteRenderer currentSprite;

    //Integers
    static int currentState;
    static int idleState = Animator.StringToHash("Base Layer.playerIdle");
    static int walkState = Animator.StringToHash("Base Layer.playerWalk");
    static int jumpState = Animator.StringToHash("Base Layer.playerJump");
    static int blockState = Animator.StringToHash("Base Layer.playerBlock");
    static int attackJump = Animator.StringToHash("Base Layer.playerJumpAttack");
    static int attack1State = Animator.StringToHash("Base Layer.playerAttack1");
    static int attack1_0State = Animator.StringToHash("Base Layer.playerAttack1 0");
    static int attack2State = Animator.StringToHash("Base Layer.playerAttack2");
    static int attack3State = Animator.StringToHash("Base Layer.playerAttack3");
    static int damageState = Animator.StringToHash("Base Layer.playerDamage");
    static int fallState = Animator.StringToHash("Base Layer.playerFall");
    static int riseState = Animator.StringToHash("Base Layer.playerRise");
    //static int deathState = Animator.StringToHash("Base Layer.playerDeath");

    //Floats
    private float currentSpeed;

    //Bools
    private bool onGround;
    private bool isDead = false;
    private bool facingRight = true;
    private bool jump = false;
    private bool block = false;

    //Strings
    private string playerStateString = "The Player state is currently: ";


	void Start ()
    {
        canMove = true;
        //Set Ups ---------

        //currentSprite = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundCheck = gameObject.transform.Find("GroundCheck");
        currentSpeed = maxSpeed;
		
	}
	

	void Update ()
    {

        //Get info on current AnimationState
        currentStateInfo = anim.GetCurrentAnimatorStateInfo (0);
        currentState = currentStateInfo.fullPathHash;

        //Check if player is on the ground
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        anim.SetBool("OnGround", onGround);

        //Check if player is dead
        anim.SetBool("Dead", isDead);

        //Display debug messages for active player movements
        if (currentState == idleState)
            Debug.Log(playerStateString + "Idle");
        if (currentState == walkState)
            Debug.Log(playerStateString + "Walking");
        if (currentState == jumpState)
            Debug.Log(playerStateString + "Jumping");
        if (currentState == blockState)
            Debug.Log(playerStateString + "Blocking");
        if (currentState == attack1State)
            Debug.Log(playerStateString + "In Attack 1");
        if (currentState == attack1_0State)
            Debug.Log(playerStateString + "In Attack 1_0");
        if (currentState == attack2State)
            Debug.Log(playerStateString + "In Attack 2");
        if (currentState == attack3State)
            Debug.Log(playerStateString + "In Attack 3");
        if (currentState == attackJump)
            Debug.Log(playerStateString + "In Jump Attack");
        if (currentState == damageState)
            Debug.Log(playerStateString + "Getting Hit");
        if (currentState == fallState)
            Debug.Log(playerStateString + "Starring as Michael Douglas in Falling Down");
        if (currentState == riseState)
            Debug.Log(playerStateString + "Is Standing Up");

        //Speed Check
        if (currentState == attack1State || currentState == attack2State || currentState == attack3State || currentState == attack1_0State)
        {
            SlowSpeed();
        }
        else if (currentState == blockState || currentState == damageState || canMove == false)
        {
            ZeroSpeed();
        }
        else
        {
            ResetSpeed();
        }


        //if (attack1HitFrame == currentSprite.sprite)
        //attack1Box.gameObject.SetActive(true);
        //else if (!attack1HitFrame == currentSprite.sprite)
        //attack1Box.gameObject.SetActive(false);


        //JUMPING//-----------------------------------------------------------------------//
        if (jump && onGround && ((currentState == idleState) || (currentState == walkState)))
        {
            //Make character jump using jumpForce
            Vector3 jumpVector = new Vector3(0, jumpForce, 0);
            rb.AddForce(jumpVector, ForceMode.Impulse);
            jump = false;
        }

        //Fall check
        if (Input.GetKeyDown(KeyCode.E) && block != true)
        {
            canMove = false;
            StartCoroutine(FallingDown());
        }

    }

    //Fall check aka Chevy Chase machine
    IEnumerator FallingDown()
    {

        float knockDownTimer = 0;

        anim.Play("playerFall");

        while (.02 > knockDownTimer)
        {
            if (facingRight == false)
            {          
                knockDownTimer += Time.deltaTime;
                rb.AddForce(new Vector3(knockBackForce * 75, knockBackForce * 3, transform.position.z));
            }
            else if (facingRight == true)
            {
                knockDownTimer += Time.deltaTime;
                rb.AddForce(new Vector3(-knockBackForce * 75, knockBackForce * 3, transform.position.z));
            }
        }
        yield return new WaitForSeconds(sleepyTime / 2);
        anim.Play("playerRise");

        yield return new WaitForSeconds(sleepyTime / 2);
        canMove = true;
    }

    private void FixedUpdate()
    {

        //Make sure player is alive.
        if(!isDead)
        {
            
            //Blocking//----------------------------------------------------------------------//
            if (Input.GetButton("Fire2"))
            {
                anim.SetBool("Block", true);
                block = true;
            }
            else if (!Input.GetButton("Fire2"))
            {
                anim.SetBool("Block", false);
                block = false;
            }


            //MOVEMENT//----------------------------------------------------------------------//
            float horizAxis = Input.GetAxis("Horizontal");
            float vertAxis = Input.GetAxis("Vertical");

            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;

            rb.velocity = new Vector3(horizAxis * currentSpeed, rb.velocity.y, vertAxis * currentSpeed);
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth + 1, maxWidth - 1),
                rb.position.y,
                Mathf.Clamp(rb.position.z, minHeight, maxHeight));


            //Ground Check
            if (!onGround)
            {
                vertAxis = 0;

                if (Input.GetButton("Fire1"))
                {
                    anim.SetBool("AirAttack", true);
                }
                else if (!Input.GetButton("Fire1"))
                {
                    anim.SetBool("AirAttack", false);
                }

            }


            if (onGround)
            {
                //Disable ground jump kicks
                anim.SetBool("AirAttack", false);

                anim.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));

                // Jumping Input & Trigger
                if (Input.GetButtonDown("Jump") && ((currentState == idleState) || (currentState == walkState)))
                {
                    anim.SetTrigger("Jump");
                    jump = true;
                }

                // Attacking Input & Trigger
                if (Input.GetButtonDown("Fire1"))
                {
                    anim.SetTrigger("Attack");
                }
            }


            //Sprite Flipper
            if (horizAxis > 0 && !facingRight && canMove == true)
            {
                Flip();
            }
            else if (horizAxis < 0 && facingRight && canMove == true)
            {
                Flip();
            }

            //Hit check
            if (Input.GetKeyDown(KeyCode.Q) && block != true)
            {
                anim.SetBool("IsHit", true);
                anim.SetTrigger("HitDamage");
            }
            else
                anim.SetBool("IsHit", false);

        }
    }

    //Sprite Flipper
    void Flip()
    {
        //Flip player sprite left or right
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    //Speed Checks
    void ZeroSpeed()
    {
        //Sets speed to 0
        currentSpeed = 0;
    }
    void SlowSpeed()
    {
        //Sets speed to maxSpeed / 20
        currentSpeed = maxSpeed / 20;
    }
    void ResetSpeed()
    {
        //Resets speed to maxSpeed
        currentSpeed = maxSpeed;
    }

}
