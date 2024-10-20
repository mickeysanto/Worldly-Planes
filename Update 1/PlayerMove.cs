using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public InputActionReference move;

    public Vector3 forward;
    public Vector2 moveDirection;

    public float moveSpeed;

    private Rigidbody rb;  

    private Vector3 tempVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();      
        moveDirection = Vector2.zero;
    }

    void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
        forward = transform.forward.normalized;
    }

    void FixedUpdate()
    {
        if (moveDirection != Vector2.zero)
        {
            Move();
        }    
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void Move()
    {
        tempVelocity = new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed;
        tempVelocity.y = rb.velocity.y;
        rb.velocity = tempVelocity;
    }
}
