using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{

    public bool playerInSight;

    public GameObject player;
    public GameObject target;

    public float targetDistance;

    public bool playerOnRight;
    public bool playerAbove;
    public Vector3 playerRelativePosition;


	void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	

	void Update ()
    {
        target = player;

        targetDistance = Vector3.Distance(target.transform.position, gameObject.transform.position);


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
    }

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
}
