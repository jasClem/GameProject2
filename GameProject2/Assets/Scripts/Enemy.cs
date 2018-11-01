using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public float maxSpeed;
    public float minHeight, maxHeight;
    public float damageTime = 0.5f;
    public int maxHealth = 10;
    public float attackRate = 1f;

    public bool tookDamage;
    public bool knockedDown;
    public float stunTime;
    public float knockDownTime;

    private int currentHealth;
    private float currentSpeed;
    private Rigidbody rb;
    private Animator anim;
    private Transform groundCheck;
    private bool onGround;
    private bool facingRight;
    private Transform target;
    private bool isDead = false;
    public float zForce;
    private float walkTimer;
    private bool damaged = false;
    private float damageTimer;
    public float nextAttack;
    public bool canMove;

    public float attackDistance = 2f;
    public float randomAttack;

    AnimatorStateInfo currentStateInfo;
    
    //Integers
    static int currentState;
    static int idleState = Animator.StringToHash("Base Layer.enemyIdle");
    static int walkState = Animator.StringToHash("Base Layer.enemyWalk");
    static int attack1State = Animator.StringToHash("Base Layer.enemyAttack1");
    static int attack2State = Animator.StringToHash("Base Layer.enemyAttack2");
    static int damageState = Animator.StringToHash("Base Layer.enemyDamage");
    static int fallState = Animator.StringToHash("Base Layer.enemyFall");
    static int riseState = Animator.StringToHash("Base Layer.enemyRise");
    static int deathState = Animator.StringToHash("Base Layer.enemyDeath");


    

    public bool playerInSight;

    public GameObject player;

    public GameObject targetPlayer;

    //NavMeshAgent navMeshAgent;

    //public GameObject spriteObject;

    public float targetDistance;

    public Vector3 playerRelativePosition;

    public bool playerOnRight;

    public bool playerAbove;

    public GameObject frontTarget;
    public GameObject backTarget;

    public float frontTargetDistance;
    public float backTargetDistance;

    Stats stats;


    void Awake()
    {
        //navMeshAgent = GetComponent<NavMeshAgent>();
        //navMeshAgent.speed = maxSpeed;
        player = GameObject.FindGameObjectWithTag("Player");

        frontTarget = GameObject.Find("Enemy Front Target");
        backTarget = GameObject.Find("Enemy Back Target");

        stats = GetComponent<Stats>();

        //Enable movement
        canMove = true;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");
        target = FindObjectOfType<Player>().transform;
        currentHealth = maxHealth;

    }


    void Start ()
    {
        
		
	}


    void Update ()
    {
        //Check if enemy is on the ground
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        anim.SetBool("OnGround", onGround);
        anim.SetBool("Dead", isDead);

        //Hit check
        if (knockedDown == true && tookDamage == false)
        {
            stats.displayUI = true;
            anim.SetBool("FallDown", true);
            if (facingRight == false)
            {
                //rb.AddForce(new Vector3(knockBackForce * 75, knockBackForce * 3, transform.position.z));
                rb.AddRelativeForce(new Vector3(3, 5, transform.position.z));
            }
            else if (facingRight == true)
            {
                //rb.AddForce(new Vector3(-knockBackForce * 75, knockBackForce * 3, transform.position.z));
                rb.AddRelativeForce(new Vector3(-3, 5, transform.position.z));
            }
            StartCoroutine(KnockedDown());
            
        }
        else if (tookDamage == true)
        {
            stats.displayUI = true;
            anim.SetBool("IsHit", true);
            StartCoroutine(TookDamage());
        }
        else if (knockedDown == false && tookDamage == false)
        {
            stats.displayUI = false;
        }

        //targetPlayer = player;


        //Find player's distance from enemy
        playerRelativePosition = player.transform.position - gameObject.transform.position;

        //Check if player is...
        if (playerRelativePosition.x < 0)
        {
            //to the left
            playerOnRight = false;
        }
        else if (playerRelativePosition.x > 0)
        {
            //to the right
            playerOnRight = true;
        }

        if (playerRelativePosition.z < 0)
        {
            //below
            playerAbove = false;
        }
        else if (playerRelativePosition.z > 0)
        {
            //above
            playerAbove = true;
        }

        frontTargetDistance = Vector3.Distance(frontTarget.transform.position, gameObject.transform.position);
        backTargetDistance = Vector3.Distance(backTarget.transform.position, gameObject.transform.position);

        if (frontTargetDistance < backTargetDistance)
            targetPlayer = frontTarget;
        else if (frontTargetDistance > backTargetDistance)
            targetPlayer = backTarget;

        targetDistance = Vector3.Distance(targetPlayer.transform.position, gameObject.transform.position);

        //Get info on current AnimationState
        currentStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        currentState = currentStateInfo.fullPathHash;


        //Set states to correct movement speeds
        if (currentState == attack1State || currentState == attack2State)
        {
            //Slow movement
            SlowSpeed();
        }
        else if (currentState == damageState || canMove == false)
        {
            //No movement
            ZeroSpeed();
        }
        else
        {
            //Reset speed
            ResetSpeed();
        }

        /*facingRight = (target.position.x < transform.position.x) ? false : true;
        if (facingRight)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);

        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }*/

        /*if(damaged && !isDead)
        {
            damageTimer += Time.deltaTime;
            if(damageTimer >= damageTime)
            {
                damaged = false;
                damageTimer = 0;
            }
        }*/

    }

    private void FixedUpdate()
    {
        

        if (!isDead && knockedDown == false && tookDamage == false)
        {
            Vector3 targetDistance = target.position - transform.position;
            float hForce = targetDistance.x / Mathf.Abs(targetDistance.x);

            walkTimer += Time.deltaTime;


            if (Mathf.Abs(targetDistance.x) < attackDistance)
            {
                hForce = 0;

                randomAttack = Random.Range(0, 2);
                Debug.Log("Random Attack == " + randomAttack);
            }


            if (walkTimer >= Random.Range(1f, 2f))
            {

                walkTimer = 0;
            }


            if (!damaged && playerInSight)
            {
                if (Mathf.Abs(targetDistance.z) < (attackDistance + 1))
                {
                    zForce = Random.Range(-1, 2);
                }
                else if (Mathf.Abs(targetDistance.z) > (attackDistance + 1))
                {
                    zForce = 0;
                }

                rb.velocity = new Vector3(hForce * currentSpeed, 0, (targetDistance.z + zForce) * currentSpeed);
                anim.SetBool("Walk", true);

                if (Mathf.Abs(targetDistance.x) < attackDistance && Mathf.Abs(targetDistance.z) < attackDistance)
                {
                    anim.SetBool("Walk", false);
                }
            }

            if (!playerInSight)
            {
                anim.SetBool("Walk", false);
            }

            if (playerOnRight && !facingRight)
            {
                Flip();
            }
            else if (!playerOnRight && facingRight)
            {
                Flip();
            }

            anim.SetFloat("Speed", Mathf.Abs(currentSpeed));


            //Attack
            if (Mathf.Abs(targetDistance.x) < attackDistance && Mathf.Abs(targetDistance.z + zForce) < attackDistance && Time.time > nextAttack)
            {

                if (randomAttack > 0)
                {
                    anim.SetTrigger("Attack_2"); //not turning off fix plz
                }

                else if (randomAttack <= 0)
                {
                    anim.SetTrigger("Attack_1");
                }
                currentSpeed = 0;
                nextAttack = Time.time + attackRate;
            }
            else if (Mathf.Abs(targetDistance.x) > attackDistance && Mathf.Abs(targetDistance.z + zForce) > attackDistance && Time.time < nextAttack)
            {
                anim.SetBool("Attack", false);
                anim.SetBool("Attack2", false);
            }

            rb.position = new Vector3
            (
                rb.position.x,
                rb.position.y,
                Mathf.Clamp(rb.position.z, minHeight, maxHeight));
        }



        


    }


    //Hit check
    IEnumerator TookDamage()
    {
        anim.Play("enemyHit");
        canMove = false;

        yield return new WaitForSeconds(stunTime);

        anim.SetBool("IsHit", false);
        canMove = true;
        tookDamage = false;
    }


    //Fall check
    IEnumerator KnockedDown()
    {

        //float knockDownTimer = 0;

        //Play fall animation


        /*        //Knockdown timer
                while (.02 > knockDownTimer)
                {
                    if (facingRight == false)
                    {
                        knockDownTimer += Time.deltaTime;

                        //rb.AddForce(new Vector3(knockBackForce * 75, knockBackForce * 3, transform.position.z));
                        rb.AddRelativeForce(new Vector3(3, 5, transform.position.z));
                    }
                    else if (facingRight == true)
                    {
                        knockDownTimer += Time.deltaTime;

                        //rb.AddForce(new Vector3(-knockBackForce * 75, knockBackForce * 3, transform.position.z));
                        rb.AddRelativeForce(new Vector3(-3, 5, transform.position.z));
                    }
                }
        */

        

        //Wait for fall & animation then play rising animation
        yield return new WaitForSeconds(knockDownTime);
        anim.SetBool("FallDown", false);
        canMove = true;
        knockedDown = false;
    }

    /*public void TakeDamage(int damage)
    {
        if(!isDead)
        {
            damaged = true;
            currentHealth -= damage;
            anim.SetTrigger("HitDamage");

            if(currentHealth <= 0)
            {
                anim.SetTrigger("Dead");
                isDead = true;
                rb.AddRelativeForce(new Vector3(3, 5, 0), ForceMode.Impulse);
            }

        }
    }*/


    //Vision Checks
    void OnTriggerStay(Collider other)
    {

        if (other.gameObject == player)
        {
            playerInSight = true;
        }
    }
    void OnTriggerExit(Collider other)
    {

        if (other.gameObject == player)
        {
            playerInSight = false;
        }
    }


    //Disable Enemy
    public void DisableEnemy()
    {
        gameObject.SetActive(false);
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
