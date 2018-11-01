using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    //public int damage;

    public bool knockDownAttack;
    public float attackStrength;

    GameObject otherObject;
    Stats otherStats;
    Player playerState;
    Enemy enemyState;


    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "PlayerAttackBox" && other.tag == "EnemyHitBox")
            EnemyTakeDamage(other.gameObject);
        else if (gameObject.tag == "EnemyAttackBox" && other.tag == "PlayerHitBox")
            PlayerTakeDamage(other.gameObject);
        else return;
    }

    void EnemyTakeDamage(GameObject other)
    {
        otherObject = other.transform.parent.gameObject;
        enemyState = otherObject.GetComponent<Enemy>();

        otherStats = otherObject.GetComponent<Stats>();

        otherStats.health = otherStats.health - attackStrength;

        if (knockDownAttack == true)
        {
            enemyState.knockedDown = true;
        }
        else if (knockDownAttack != true)
        {
            enemyState.tookDamage = true;
        }

        Debug.Log("Enemy Takes Damage");
        Debug.Log(otherObject + " was hit");

    }

    void PlayerTakeDamage(GameObject other)
    {

        otherObject = other.transform.parent.gameObject;
        playerState = otherObject.GetComponent<Player>();

        otherStats = otherObject.GetComponent<Stats>();

        otherStats.health = otherStats.health - attackStrength;

        if (knockDownAttack == true)
        {
            playerState.knockedDown = true;
        }
        else if (knockDownAttack != true)
        {
            playerState.tookDamage = true;
        }

        Debug.Log("Player Takes Damage");
        Debug.Log(otherObject + "was hit");
    }
}