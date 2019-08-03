using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Item item;

    [SerializeField] private static readonly float SPEED = 64.0f;
    [SerializeField] private static readonly float ATTACK_COOLDOWN = 0.5f; //Time between attacks

    [SerializeField] private static readonly float DAMPENING = 0.01f;

	private Rigidbody2D rb;
	private float attackCooldown;

    void Start()
    {
		this.attackCooldown = 0.0f;
		this.rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
	{
		ThrowUpdate();
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
	
	private void ThrowUpdate()
	{
		if (Input.GetMouseButtonDown(1))
		{

		}
	}

	private void AttackUpdate()
    {

        if (Input.GetMouseButtonDown(0) && attackCooldown <= 0.0f)
        {
			transform.GetChild(1).transform.rotation = Quaternion.identity;
			attackCooldown = ATTACK_COOLDOWN;

			switch (item.name)
			{
				case "Sword":
					//Sword attack
					break;
				case "Bomb":
					//Place bomb
					//Clear item
					break;
				case "Key":
				case "Coin":
				case "None":
					//Do nothing
					break;
				default:
					break;
			}
        }
		else
		{
			attackCooldown -= Time.deltaTime;
			transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, 33.333f);
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
			transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = item.iSprite;
		}
    }

 
}
