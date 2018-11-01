using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWalk : MonoBehaviour
{

    public float enemySpeed;
    public float enemyCurrentSpeed;
    public float minHeight, maxHeight;
    public bool facingRight;

    Animator animator;
    private Rigidbody rb;
    EnemySight enemySight;

    private Transform groundCheck;
    private bool onGround;

    private Transform target;
    private float walkTimer;
    public float zForce;

    public GameObject player;
    public GameObject targetPlayer;
    public float targetDistance;

    void Awake ()
    {
        target = FindObjectOfType<Player>().transform;

        enemySight = GetComponent<EnemySight>();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        groundCheck = transform.Find("GroundCheck");

        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update ()
    {
        targetPlayer = player;
        targetDistance = Vector3.Distance(targetPlayer.transform.position, gameObject.transform.position);

        //Check if enemy is on the ground
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        animator.SetBool("OnGround", onGround);

        if (enemySight.playerInSight == true)
        {
            Vector3 targetDistance = target.position - transform.position;
            float hForce = targetDistance.x / Mathf.Abs(targetDistance.x);



            if (walkTimer >= Random.Range(1f, 2f))
            {
                zForce = Random.Range(-1, 2);
                walkTimer = 0;
            }

            if (Mathf.Abs(targetDistance.x) < 2f)
            {
                hForce = 0;
            }



            rb.velocity = new Vector3(hForce * enemyCurrentSpeed, 0, zForce * enemyCurrentSpeed);
                animator.SetBool("Walk", true);

            if (Mathf.Abs(targetDistance.x) < 1.6f && Mathf.Abs(targetDistance.z) < 1.6f)
            {
                animator.SetBool("Walk", false);
            }

            if (enemySight.playerOnRight && !facingRight)
            {
                Flip();
            }
            else if (!enemySight.playerOnRight && facingRight)
            {
                Flip();
            }

            animator.SetFloat("Speed", Mathf.Abs(enemyCurrentSpeed));

            rb.position = new Vector3
            (
                rb.position.x,
                rb.position.y,
                Mathf.Clamp(rb.position.z, minHeight, maxHeight));
        }

        if (enemySight.playerOnRight == true && !facingRight)
            Flip();
        else if (enemySight.playerOnRight == false && facingRight)
            Flip();
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
}
