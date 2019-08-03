using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Item item;

    [SerializeField]
    private static readonly float SPEED = 32.0f;

	[SerializeField] private readonly float DAMPENING = 0.01f;

	private Rigidbody2D rb;
	
    void Start()
    {
		this.rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
	{

		PickupCheck();
    }

	private void FixedUpdate()
	{
		Vector2 acceleration = new Vector2();

		if (Input.GetKey(KeyCode.W)) acceleration.y += SPEED;
		if (Input.GetKey(KeyCode.S)) acceleration.y -= SPEED;
		if (Input.GetKey(KeyCode.A)) acceleration.x -= SPEED;
		if (Input.GetKey(KeyCode.D)) acceleration.x += SPEED;

		this.rb.velocity += acceleration * Time.deltaTime;

		this.rb.velocity *= Mathf.Pow(DAMPENING, Time.deltaTime);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		Debug.Log("Collided with: " + collision.gameObject.name);
	}

	private void PickupCheck()
    {
        
    }
}
