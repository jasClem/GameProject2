﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public float startingHealth;
    public float health;

    public bool displayUI;

    public Slider healthSlider;

    public GameObject healthUI;

    void Awake()
    {
        health = startingHealth;
    }

    void Update()
    {
        if (gameObject.tag == ("Player"))
        {
            healthUI = GameObject.FindGameObjectWithTag("PlayerHealthUI");
            healthSlider = healthUI.gameObject.transform.GetChild(0).GetComponent<Slider>();
            if (healthSlider.maxValue == 0)
            {
                healthSlider.maxValue = startingHealth;
            }
            healthSlider.value = health;
        }

        if (gameObject.tag == ("Enemy"))
        {
            if (displayUI)
            {
                healthUI = GameObject.FindGameObjectWithTag("EnemyHealthUI");
                healthSlider = healthUI.gameObject.transform.GetChild(0).GetComponent<Slider>();
                if (healthSlider.maxValue == 0)
                {
                    healthSlider.maxValue = startingHealth;
                }
                healthSlider.value = health;
            }

            if(!displayUI)
            {
                healthUI = null;
                healthSlider = null;
            }

        }

        if (health <= 0)
            Destroy(gameObject);
    }

}
