using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Item item;

    [SerializeField]
    private static readonly float SPEED = 512.0f;

	[SerializeField] private readonly float DAMPENING = 0.01f;

	private Vector2 speed;
	
    void Start()
    {
		this.speed = new Vector2();
    }

    private void Update()
	{
		Vector2 acceleration = new Vector2();

		if (Input.GetKey(KeyCode.W)) acceleration.y += SPEED;
		if (Input.GetKey(KeyCode.S)) acceleration.y -= SPEED;
		if (Input.GetKey(KeyCode.A)) acceleration.x -= SPEED;
		if (Input.GetKey(KeyCode.D)) acceleration.x += SPEED;

		this.speed += acceleration * Time.deltaTime;
		this.transform.position += new Vector3(this.speed.x * Time.deltaTime, this.speed.y * Time.deltaTime, 0.0f);

		this.speed *= Mathf.Pow(DAMPENING, Time.deltaTime);

		PickupCheck();
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Collided with: " + collision.gameObject.name);
	}

	private void PickupCheck()
    {
        
    }
}
