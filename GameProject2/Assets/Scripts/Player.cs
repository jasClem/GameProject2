using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Various Public Variables - can be changed in unity
    public float maxSpeed = 4;
    public float jumpForce = 500;
    public float minHeight, maxHeight;
    
    public float knockBackForce;
    public bool canMove;
    public bool tookDamage;
    public bool knockedDown;
    public float stunTime;
    public float knockDownTime;

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
    static int deathState = Animator.StringToHash("Base Layer.playerDeath");

    //Floats
    private float currentSpeed;

    //Bools
    private bool onGround;
    private bool isDead = false;
    private bool facingRight = true;
    private bool jump = false;
    private bool block = false;

    //Strings
    //for state machine
    //private string playerStateString = "The Player state is currently: ";

    //Attack Boxes
    //public GameObject attack1Box, attack2Box, attack3Box, airAttackBox;
    //public Sprite attack1HitFrame, attack2HitFrame, attack3HitFrame, airAttackHitFrame;
    //public GameObject blockBox;

    //Various Others
    private Rigidbody rb;
    private Animator anim;
    private Transform groundCheck;
    AnimatorStateInfo currentStateInfo;
    //SpriteRenderer currentSprite;


    void Start ()
    {

        //Enable movement
        canMove = true;

        //Get Rigidbody & Animator components
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        //Get groundCheck object
        groundCheck = gameObject.transform.Find("GroundCheck");

        //Set speed to maxSpeed
        currentSpeed = maxSpeed;


        //currentSprite = GetComponent<SpriteRenderer>();
        //UNUSED: For tracking sprite animations
    }


    void Update ()
    {

        //Get info on current AnimationState
        currentStateInfo = anim.GetCurrentAnimatorStateInfo (0);
        currentState = currentStateInfo.fullPathHash;

        //Check if player is on the ground
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        anim.SetBool("OnGround", onGround);

        //Check if player is alive
        anim.SetBool("Dead", isDead);

        //Set states to correct movement speeds
        if (currentState == attack1State || currentState == attack2State || currentState == attack3State || currentState == attack1_0State)
        {
            //Slow movement
            SlowSpeed();
        }
        else if (currentState == blockState || currentState == damageState || currentState == fallState || currentState == riseState ||
            currentState == deathState || canMove == false)
        {
            //No movement
            ZeroSpeed();
        }
        else
        {
            //Reset speed
            ResetSpeed();
        }

        //Check for jumping
        if (jump && onGround && ((currentState == idleState) || (currentState == walkState)))
        {
            //Make character jump using jumpForce
            Vector3 jumpVector = new Vector3(0, jumpForce, 0);
            rb.AddForce(jumpVector, ForceMode.Impulse);
            jump = false;
        }

        //State Machine - debug display - current player state
        /*if (currentState == idleState)
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
            */


        //UNUSED: for hitboxes & using animation
        //if (attack1HitFrame == currentSprite.sprite)
        //attack1Box.gameObject.SetActive(true);
        //else if (!attack1HitFrame == currentSprite.sprite)
        //attack1Box.gameObject.SetActive(false);

    }

    private void FixedUpdate()
    {

        //Make sure player is alive.
        if(!isDead)
        {
            
            //BLOCKING CONTROLS//----------------------------------------------------------------------//
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


            //MOVEMENT CONTROLS//----------------------------------------------------------------------//
            float horizAxis = Input.GetAxis("Horizontal");
            float vertAxis = Input.GetAxis("Vertical");

            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;

            rb.velocity = new Vector3(horizAxis * currentSpeed, rb.velocity.y, vertAxis * currentSpeed);
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth + 1, maxWidth - 1),
                rb.position.y,
                Mathf.Clamp(rb.position.z, minHeight, maxHeight));

            //Direction check
            if (horizAxis > 0 && !facingRight && canMove == true)
            {
                Flip();
            }
            else if (horizAxis < 0 && facingRight && canMove == true)
            {
                Flip();
            }


            //Check if the player is on the ground
            if (onGround)
            {
                //Set AirAttack to false
                anim.SetBool("AirAttack", false);

                //Set correct speed
                anim.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));

            //JUMP CONTROLS//--------------------------------------------------------------------------//
                if (Input.GetButtonDown("Jump") && 
                    ((currentState == idleState) || (currentState == walkState)))
                {
                    //Set Jump trigger
                    anim.SetTrigger("Jump");

                    //Set Jump to true
                    jump = true;
                }

            //ATTACK CONTROLS//------------------------------------------------------------------------//
                if (Input.GetButtonDown("Fire1"))
                {
                    anim.SetTrigger("Attack");
                }
            }

            
            //Check if the player is in the air
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


        }

        //Hit check
        if (tookDamage == true && knockedDown == false)
        {
            StartCoroutine(TookDamage());
        }

        //Check for falling
        if (knockedDown == true)
        {
            canMove = false;
            StartCoroutine(KnockedDown());
        }
    }

    IEnumerator TookDamage()
    {
        anim.Play("playerDamage");
        anim.SetBool("IsHit", true);
        canMove = false;
        //anim.SetTrigger("HitDamage");

        if (facingRight == false)
            rb.AddRelativeForce(new Vector3(knockBackForce, 2, transform.position.z));
            if (facingRight == false)
            rb.AddRelativeForce(new Vector3(-knockBackForce, 2, transform.position.z));

        yield return new WaitForSeconds(stunTime);

        anim.SetBool("IsHit", false);
        canMove = true;
        tookDamage = false;
    }


    //Fall check
    IEnumerator KnockedDown()
    {

        /*       float knockDownTimer = 0;

               //Play fall animation
               anim.Play("playerFall");

               //Knock the player back in the right direction

               //Knockdown timer
               while (.02 > knockDownTimer)
               {
                   if (facingRight == false)
                   {
                       knockDownTimer += Time.deltaTime;

                       //rb.AddForce(new Vector3(knockBackForce * 75, knockBackForce * 3, transform.position.z));
                       rb.AddRelativeForce(new Vector3(knockBackForce, 2, transform.position.z), ForceMode.Impulse);
                   }
                   else if (facingRight == true)
                   {
                       knockDownTimer += Time.deltaTime;

                       //rb.AddForce(new Vector3(-knockBackForce * 75, knockBackForce * 3, transform.position.z));
                       rb.AddRelativeForce(new Vector3(-knockBackForce, 2, transform.position.z), ForceMode.Impulse);
                   }
               }

               //rb.AddRelativeForce(new Vector3(3, 5, 0), ForceMode.Impulse);

               //Wait for fall & animation then play rising animation
               yield return new WaitForSeconds(sleepyTime / 2);
               anim.Play("playerRise");

               //Wait for rising animation and re-enable player movement
               yield return new WaitForSeconds(sleepyTime / 2);
               canMove = true;
       */

        anim.Play("playerFall");
        anim.SetBool("FallDown", true);
        canMove = false;

        yield return new WaitForSeconds(knockDownTime);

        anim.SetBool("FallDown", false);
        canMove = true;
        knockedDown = false;
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
