using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Item item;



    [SerializeField]
    private readonly float SPEED = 1.5f;
    
    private float horizontal;
    private float vertical;
    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 movementVector = new Vector2(   Input.GetAxisRaw("Horizontal") * SPEED,
                                                Input.GetAxisRaw("Vertical") * SPEED);
        rb.MovePosition(rb.position + movementVector * Time.deltaTime);
    }

    private void Update()
    {
        PickupCheck();
    }

    private void PickupCheck()
    {
        
    }
}
