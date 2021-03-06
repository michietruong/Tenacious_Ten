﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipLever : MonoBehaviour
{
	public bool flipped;
	AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
		flipped = false;
		sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(flipped)
		{
			GetComponent<SpriteRenderer>().flipX = true;
		}
		else
		{
			GetComponent<SpriteRenderer>().flipX = false;
		}
    }

	void OnTriggerEnter2D(Collider2D other)//may change to input later for triggering the lever
	{
		if(other.gameObject.tag == "Player")
		{
			if(!flipped)
				sound.Play();
			flipped = true;
		}
	}
}
