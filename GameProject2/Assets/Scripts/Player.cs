using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float maxSpeed = 4;
    public float jumpForce = 400;
    public float minHeight, maxHeight;

    private float currentSpeed;

    private Rigidbody rb;
    private Animator anim;
    private Transform groundCheck;
    private bool onGround;

    AnimatorStateInfo currentStateInfo;

    static int currentState;
    static int idleState = Animator.StringToHash("Base Layer.playerIdle");
    static int walkState = Animator.StringToHash("Base Layer.playerWalk");
    static int jumpState = Animator.StringToHash("Base Layer.playerJump");
    static int attack1State = Animator.StringToHash("Base Layer.playerAttack1");
    static int attack2State = Animator.StringToHash("Base Layer.playerAttack2");
    static int attack3State = Animator.StringToHash("Base Layer.playerAttack3");
    static int damageState = Animator.StringToHash("Base Layer.playerDamage");
    static int fallState = Animator.StringToHash("Base Layer.playerFall");
    static int riseState = Animator.StringToHash("Base Layer.playerRise");
    static int deathState = Animator.StringToHash("Base Layer.playerDeath");


    private bool isDead = false;
    private bool facingRight = true;
    private bool jump = false;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundCheck = gameObject.transform.Find("GroundCheck");
        currentSpeed = maxSpeed;

		
	}
	
	// Update is called once per frame
	void Update () {

        currentStateInfo = anim.GetCurrentAnimatorStateInfo (0);
        currentState = currentStateInfo.fullPathHash;

        if (currentState == idleState)
            Debug.Log("Idle State");

        if (currentState == walkState)
            Debug.Log("Walk State");

        if (currentState == jumpState)
            Debug.Log("Jump State");

        if (currentState == attack1State)
            Debug.Log("Attack 1 State");

        if (currentState == attack2State)
            Debug.Log("Attack 2 State");

        if (currentState == attack3State)
            Debug.Log("Attack 3 State");


        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        anim.SetBool("OnGround", onGround);
        anim.SetBool("Dead", isDead);


        if(Input.GetButtonDown("Jump") && onGround)
        {
            anim.SetTrigger("Jump");
            jump = true;
        }

        if(Input.GetButton("Fire1"))
        {
            anim.SetTrigger("Attack");
        }

        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("Block");
        }


    }

    private void FixedUpdate()
    {
        if(!isDead)
        {
            float h = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (!onGround)
                z = 0;

            rb.velocity = new Vector3(h * currentSpeed, rb.velocity.y, z * currentSpeed);

            if (onGround)
                anim.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));

            if (h > 0 && !facingRight)
            {
                Flip();
            }
            else if(h < 0 && facingRight)
            {
                Flip();
            }

            if(jump)
            {
                jump = false;
                rb.AddForce(Vector3.up * jumpForce);
            }

            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;


            //rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth + 1, maxWidth - 1),
                //rb.position.y,
                //Mathf.Clamp(rb.position.z, minHeight + 1, maxHeight - 1));   <--Makes character jump around screen (don't use cause it sucks)
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    void ResetSpeed()
    {
        currentSpeed = maxSpeed;
    }

}
