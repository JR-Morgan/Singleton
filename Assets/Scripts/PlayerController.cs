using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Item item;

    [SerializeField] private static readonly float SPEED = 64.0f;
    [SerializeField] private static readonly float ATTACK_SPEED = 0.1f; //Attack speed in secconds

    [SerializeField] private readonly float DAMPENING = 0.01f;

	private Rigidbody2D rb;
    private float attackDeltaT = 0f;

    void Start()
    {
		this.rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        AttackUpdate();
    }

	private void FixedUpdate()
	{
		MovementUpdate();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		CheckItemPickup(collision);
	}

	private void AttackUpdate()
    {

        transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, 33.333f);

        if (Input.GetButton("Attack"))
        {
			transform.GetChild(1).transform.rotation = Quaternion.identity;
            attackDeltaT = ATTACK_SPEED;
        }
        if (attackDeltaT <= 0)
        {
            attackDeltaT -= Time.deltaTime;
        }


    }

    private void MovementUpdate()
    {
        Vector2 acceleration = new Vector2();

        if (Input.GetKey(KeyCode.W)) acceleration.y += SPEED;
        if (Input.GetKey(KeyCode.S)) acceleration.y -= SPEED;
        if (Input.GetKey(KeyCode.A)) acceleration.x -= SPEED;
        if (Input.GetKey(KeyCode.D)) acceleration.x += SPEED;

        this.rb.velocity += acceleration * Time.deltaTime;
        this.rb.velocity *= Mathf.Pow(DAMPENING, Time.deltaTime);
    }

    private void CheckItemPickup(Collider2D collision)
    {
		Debug.Log("Collided with trigger!");

        if (collision.gameObject.tag == "Item")
		{
			item = collision.gameObject.GetComponent<DropedItem>().item;
			Destroy(collision.gameObject);
			transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = item.sprite;
		}
    }

 
}
