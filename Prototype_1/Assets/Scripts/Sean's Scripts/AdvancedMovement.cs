﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedMovement : MonoBehaviour
{

    public LayerMask groundLayer;   // for checking if player hits the ground

    Rigidbody rigbody;          // this body is used to apply directional force 

    // basic movement controls
    int maxVelocity = 10;        // max speed of the object
    int moveForce = 150;         // strength of push used for movement

    // jumping movement controls
    float jumpForce = 1000f;       // strength of push used for jumping
    bool jumping = false;       // has the object used jump
    bool doubleJumping = false; // has the object used double jump

    // dash movement controls
    float lastA = -3;   // used for timing double tap in dash
    float lastD = -3;   // used for timing double tap in dash
    bool dashing = false;  // is object dashing
    float dashTimer = -3; // when dash was used last
    float dashMulti = 2.5f; // multiplier used to multiply jumpForce in the dash movements

    // Checks if the player touches a platform and resets jumping capability if they do
    public void OnCollisionEnter(Collision collision)
    {
        if (dashing)
        {
            if (CheckTime(dashTimer) > 1) {

            }
        }
        if (collision.gameObject.layer == groundLayer)
        {
            // resets ability to jump/ double jump
            jumping = false;
            doubleJumping = false;
        }
    }

    // Use this for initialization
    void Start()
    {
        rigbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {   

        // jumping movement
        if (Input.GetKeyDown(KeyCode.Space) && (!jumping || !doubleJumping))
        {            
            if (!jumping)
            {
                rigbody.AddForce(0, jumpForce, 0);
                jumping = true;
            }
            else
            {
                rigbody.AddForce(0, jumpForce * 1.25f, 0);
                doubleJumping = true;
            }
        }
        // ground pound movement
        if (Input.GetKey("s") && (jumping || doubleJumping))
        {
            rigbody.velocity.Set(0, 0, 0);      // stop directional movement
            rigbody.AddForce(0, -jumpForce/2, 0);   // apply downwards force
        }
        // right movement
        if (Input.GetKey("d") && rigbody.velocity[0] < maxVelocity)
        {
            rigbody.AddForce(moveForce, 0, 0);
        }
        // right dash
        if (Input.GetKeyDown("d"))
        {   
            if (CheckTime(lastD) < 0.25 && CheckTime(dashTimer) > 3)
            {
                rigbody.AddForce(jumpForce*dashMulti, 0, 0);
                dashTimer = Time.time;
                dashing = true;
            }
            else
            {
                lastD = Time.time;
            }
        }
        // left movement
        if (Input.GetKey("a") && rigbody.velocity[0] > -maxVelocity)
        {
            rigbody.AddForce(-moveForce, 0, 0);
        }
        // left dash
        if (Input.GetKeyDown("a"))
        {
            if (CheckTime(lastA) < 0.25 && CheckTime(dashTimer) > 3)
            {
                rigbody.AddForce(-jumpForce*dashMulti, 0, 0);
                dashTimer = Time.time;
                dashing = true;
            }
            else
            {
                lastA = Time.time;
            }
        }
    }
    // time difference between 2 times. Confirms user double tapped.
    float CheckTime(float i) {
        return Time.time - i;
    }
}