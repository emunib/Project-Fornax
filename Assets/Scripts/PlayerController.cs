using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private float xForce, yForce;
    public float speed;
    private bool grounded;

    // Use this for initialization
    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        yForce = 100;
    }

    // Update is called once per frame
    private void Update()
    {
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!grounded && other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (grounded && other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    private void FixedUpdate()
    {
        xForce = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                rigidbody2d.AddForce(new Vector2(0, yForce*speed));
                grounded = false;
            }
        }

        var force = new Vector2(xForce, 0);

        rigidbody2d.AddForce(force * speed);
    }
}