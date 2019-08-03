﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Item heldItem;

    [SerializeField] private float SPEED = 64.0f;
	//Time between attacks
	[SerializeField] private float ATTACK_COOLDOWN = 0.5f;
	//Speed at which objects are thrown
	[SerializeField] private float THROW_SPEED = 16.0f;

	[SerializeField] private float DAMPENING = 0.01f;

	[SerializeField] private GameObject DROPPED_ITEM_PREFAB;

	private Rigidbody2D rb;
	private float attackCooldown;

    void Start()
    {
		this.attackCooldown = 0.0f;
		this.rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
	{
		this.ThrowUpdate();
		this.AttackUpdate();
    }

	private void FixedUpdate()
	{
		this.MovementUpdate();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		this.CheckItemPickup(collision);
	}

	private void ThrowUpdate()
	{
		if (Input.GetMouseButtonDown(1))
		{
			Vector2 throwDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
			throwDirection.Normalize();

			//instantiate new droppedItem prefab
			GameObject g = GameObject.Instantiate(DROPPED_ITEM_PREFAB);

			g.transform.position = this.transform.position;
			g.GetComponent<Rigidbody2D>().velocity = throwDirection * THROW_SPEED;

			//Swap items in prefab and inventory, effectively clears it
			{
				Item temp = this.heldItem;
				this.heldItem = g.GetComponent<DroppedItem>().item;
				g.GetComponent<DroppedItem>().item = temp;
			}

			//Set hand sprite to null
			this.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = null;
		}
	}

	private void AttackUpdate()
    {
		if (attackCooldown <= 0.0f)
		{
			if (Input.GetMouseButton(0))
			{
				attackCooldown = ATTACK_COOLDOWN;
			}
			else
			{
				this.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, 33.333f);
			}
		}
		else
		{
			this.transform.GetChild(1).transform.rotation = Quaternion.identity;
			this.attackCooldown -= Time.deltaTime;
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
        if (collision.gameObject.tag == "Item")
		{
			DroppedItem i = collision.gameObject.GetComponent<DroppedItem>();

			if (this.heldItem.iName.Equals(""))
			{
				this.heldItem = i.item;
				this.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = this.heldItem.iSprite;
				GameObject.Destroy(collision.gameObject);
			}
		}
    }

 
}
