﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public float speedX;
    public float jumpSpeedY;

    bool facingRight, Jumping, isGrounded, isWalking;
    public float speed;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool grounded;

    public GameObject leftProjectile, rightProjectile;

    Transform projectilePos;    //transform is position

    public float shotDelay;
    private float shotDelayCounter;

    public Animator anim;
    Rigidbody2D rb;

    [SerializeField] float flipValue;

    bool justJumped;

    int prevAnimState; //used to hold a variable for the previous animation state and call back to it if needed

    public PlayerHealthManager healthManager;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        healthManager = FindObjectOfType<PlayerHealthManager>();

        facingRight = true;

        isWalking = false;

        projectilePos = transform.Find("projectilePos");   //find child named projectilePos (i.e. its position)

        flipValue = 1;

        justJumped = false;
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        //checks for if player is touching the ground
    }

    // Update is called once per frame
    void Update () {
        if(!Boss01PauseMenu.GameIsPaused)
        {
            if(healthManager.isDead)
            {
                speed = 0;
                anim.SetInteger("State", 0);
            }
            if (!healthManager.isDead)
            {
                MovePlayer(speed);
                Flip();


                if (Input.GetKey(KeyCode.RightArrow))
                {
                    anim.SetInteger("State", 2);
                    speed = speedX;     //move right
                    isWalking = true;
                }
                if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    anim.SetInteger("State", 0);
                    speed = 0;         //not walking/idle
                    isWalking = false;
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    anim.SetInteger("State", 2);
                    speed = -speedX;    //move left
                    isWalking = true;
                }
                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    anim.SetInteger("State", 0);
                    speed = 0;          //not walking/idle
                    isWalking = false;
                }


                if (justJumped && grounded)
                {
                    anim.SetInteger("State", 5);
                    justLanded(0.2f);
                    justJumped = false;
                }

                if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)    //jump
                {
                    Jump();
                    justJumped = true;
                }



                if (Input.GetKey(KeyCode.UpArrow))
                {
                    anim.SetInteger("State", 3);
                }

                if (Input.GetKeyDown(KeyCode.Space))    //shoot
                {
                    //anim.SetInteger("State", 7);
                    Fire();
                    shotDelayCounter = shotDelay;
                }

                if (Input.GetKey(KeyCode.Space))    //shoot, but able to hold down space to shoot automatically
                {
                    anim.SetInteger("State", 9);
                    shotDelayCounter -= Time.deltaTime;

                    if (shotDelayCounter <= 0)
                    {
                        shotDelayCounter = shotDelay;
                        Fire();
                    }
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    if (!grounded) //if player is in the air, e.g. jumping
                    {
                        anim.SetInteger("State", 3);
                    }
                    if (grounded && isWalking) //(Input.GetKeyDown(KeyCode.LeftArrow)) || Input.GetKey(KeyCode.RightArrow)) //if player is on ground and walking
                    {
                        anim.SetInteger("State", 2);
                    }
                    else if (grounded)
                    {
                        anim.SetInteger("State", 0);
                    }
                }
            }
            
        }
        else if(Boss01PauseMenu.GameIsPaused)
        {
            speed = 0;
        }

        


    }

    void Flip()     //function to flip the image of the player. also flips the projectile as well 
    {
        if (speed > 0 && facingRight == false || speed < 0 && facingRight == true)
        {
            facingRight = !facingRight;

            Vector3 temp = transform.localScale;
            Vector3 temp2 = transform.localPosition;
            temp.x *= -1;
            if(facingRight)
            {

                temp2.x = temp2.x + flipValue;
            }
            if(!facingRight)
            {

                temp2.x = temp2.x - flipValue;
            }
            transform.localScale = temp;
            transform.localPosition = temp2;
        }

    }

    void MovePlayer(float playerSpeed) //player movement
    {
        rb.velocity = new Vector3(playerSpeed, rb.velocity.y, 0);
    }

    void Jump()
    {
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.AddForce(new Vector2(rb.velocity.x, jumpSpeedY));  
        Jumping = true;

    }

    void Fire() //shoot projectile/fire projectile
    {
        if (facingRight == true)
        {
            Instantiate(rightProjectile, projectilePos.position, Quaternion.identity);    
            //Instantiate means create. So create a right projectile, at a specific position in world/space (projectilePos)
            //Quanternion is rotation, in this case do not rotate
        }
        if (facingRight == false)
        {
            Instantiate(leftProjectile, projectilePos.position, Quaternion.identity);
        }
    }

    IEnumerator justLanded(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetInteger("State", 0);
    }
    
}
