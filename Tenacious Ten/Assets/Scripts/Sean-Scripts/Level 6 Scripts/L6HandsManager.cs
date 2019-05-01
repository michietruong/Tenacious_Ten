﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L6HandsManager : MonoBehaviour
{

    public int maxHealth;
    public int enemyHealth;

    public GameObject deathEffect;

    SpriteRenderer sr;
	ChariotMiniReset2 track;
	[SerializeField]
	GameObject track3;
	[SerializeField]
	GameObject track4;
	bool skyStarter = false;

    [SerializeField] public bool handsIsDead;

    [SerializeField] AudioSource hurtSound;

    public bool moveToStart;
    [SerializeField] float moveSpeedLeft;

    public Level6Manager L6Manager;

    void Start()
    {
        maxHealth = enemyHealth;
        sr = GetComponent<SpriteRenderer>();
        handsIsDead = false;
        moveToStart = false;
        L6Manager = FindObjectOfType<Level6Manager>();
		track = GetComponent<ChariotMiniReset2>();
	}

    void FixedUpdate()
    {
		if (enemyHealth <= 5)
        {
            Instantiate(deathEffect, transform.position, transform.rotation);
            Destroy(gameObject);
            handsIsDead = true;
        }
        if (moveToStart == false && L6Manager.bootsDead)
        {
			if(skyStarter == false)
			{
				track.damageBenchmark = track.pushBack.TotalDamage;
				track3.GetComponent<ChariotMiniReset3>().damageBenchmark = track3.GetComponent<ChariotMiniReset3>().pushBack.TotalDamage;
				track4.GetComponent<ChariotMiniReset4>().damageBenchmark = track4.GetComponent<ChariotMiniReset4>().pushBack.TotalDamage;
				skyStarter = true;
			}
            transform.position = new Vector3(transform.position.x - moveSpeedLeft, transform.position.y, transform.position.z);
            if (transform.position.x <= -53)
            {
                moveToStart = true;
            }
        }

    }

    public void giveDamage(int damageToGive)
    {
        hurtSound.Play();
        enemyHealth -= damageToGive;
        StartCoroutine(flash());
    }

    IEnumerator flash()
    {
        sr.color = new Color(1, 1, 0.3f, 1);
        yield return new WaitForSeconds(0.03f);
        sr.color = new Color(1, 1, 1, 1);
    }
}