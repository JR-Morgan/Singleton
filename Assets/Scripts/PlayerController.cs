using System;
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

	[SerializeField] private float MAX_HEALTH = 3;

	[SerializeField] private float KNOCKBACK = 16.0f;

	[SerializeField] private GameObject DROPPED_ITEM_PREFAB;
	[SerializeField] private GameObject EARTH_BLOCK_PREFAB;
	[SerializeField] private Item EMPTY_ITEM;
    [SerializeField] private LayerMask ENEMY_LAYER_MASK;

    [SerializeField] private AudioClip ITEM_THROW;
	[SerializeField] private AudioClip ITEM_PICKUP;
	[SerializeField] private AudioClip DOOR_UNLOCK;
	[SerializeField] private AudioClip ENEMY_HIT;
	[SerializeField] private AudioClip SWORD_SWING;

	private Rigidbody2D rb;
	private Animator anim;
	private GameObject hand;
	private AudioSource audioSource;
	private float attackCooldown;

	private int health;

	void Start()
    {
		this.attackCooldown = 0.0f;
		this.rb = this.GetComponent<Rigidbody2D>();
		this.anim = this.GetComponent<Animator>();
		this.hand = this.transform.GetChild(1).gameObject;
		this.audioSource = this.GetComponent<AudioSource>();
		this.health = 3;
	}

    private void Update()
	{
		this.ThrowUpdate();
		this.AttackUpdate();
		this.AnimationUpdate();
    }

	private void FixedUpdate()
	{
		this.MovementUpdate();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		this.CheckDoorUnlock(collision);
		this.CheckEnemy(collision);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		this.CheckItemPickup(collision);
	}

	private void AnimationUpdate()
	{
		this.anim.SetFloat("Speed", this.rb.velocity.magnitude);
		this.transform.localScale = new Vector3((this.rb.velocity.x > 0) ? 1.0f : -1.0f, 1.0f, 1.0f);
	}

	private void ThrowUpdate()
	{
		if (!this.heldItem.iName.Equals(""))
		{
			if (Input.GetMouseButtonDown(1))
			{
				Vector2 throwDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
				throwDirection.Normalize();

				//instantiate new droppedItem prefab
				GameObject g = GameObject.Instantiate(DROPPED_ITEM_PREFAB);

				g.transform.position = this.transform.position;
				g.GetComponent<Rigidbody2D>().velocity = throwDirection * THROW_SPEED;

				g.GetComponent<DroppedItem>().item = this.heldItem;
				this.heldItem = EMPTY_ITEM;

				//Set hand sprite to null
				this.hand.GetComponent<SpriteRenderer>().sprite = this.heldItem.iSprite;

				this.audioSource.PlayOneShot(ITEM_THROW);
			}
		}
	}

	private void AttackUpdate()
    {
		if (attackCooldown <= 0.0f)
		{
			if (Input.GetMouseButton(0))
			{
				this.attackCooldown = ATTACK_COOLDOWN;

				switch (this.heldItem.iName)
				{
					case "Earth Staff":
						GameObject g = GameObject.Instantiate(EARTH_BLOCK_PREFAB);

						Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
						g.transform.position = v;
						break;
                    case "Sword":
                        Attack(transform , 1.5f, 1);
						this.audioSource.PlayOneShot(SWORD_SWING);
						break;
					default:
						break;
				}
			}
			else
			{
				this.hand.transform.localRotation = Quaternion.identity;
			}
		}
		else
		{
			this.hand.transform.localRotation = Quaternion.Euler(0, 0, -33.333f);
			this.attackCooldown -= Time.deltaTime;
		}
    }

    private void Attack(Transform attackPosition, float attackRadius, int damage)
    {
        Collider2D[] damageTargets = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius, ENEMY_LAYER_MASK);
        
        foreach (Collider2D target in damageTargets)
        {
            Debug.Log("Target damanged" + target);
            target.GetComponent<EnemyAI>().TakeDamage(damage);
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

	private void CheckDoorUnlock(Collision2D collision)
	{
		if (collision.gameObject.tag.Equals("Door") && this.heldItem.iName.Equals("Key"))
		{
			GameObject.Destroy(collision.gameObject);
			this.heldItem = EMPTY_ITEM;
			this.hand.GetComponent<SpriteRenderer>().sprite = this.heldItem.iSprite;

			this.audioSource.PlayOneShot(DOOR_UNLOCK, 0.5f);
		}
	}

	private void CheckEnemy(Collision2D collision)
	{
		if (collision.gameObject.tag.Equals("Enemy"))
		{
			this.health--;

			Vector2 knockbackAmount = (this.transform.position - collision.transform.position) * KNOCKBACK;

			this.rb.velocity += knockbackAmount;
			collision.rigidbody.velocity -= knockbackAmount;
			this.audioSource.PlayOneShot(ENEMY_HIT);
		}
	}

    private void CheckItemPickup(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Item"))
		{
			DroppedItem i = collision.gameObject.GetComponent<DroppedItem>();

			if (this.heldItem.iName.Equals(""))
			{
				this.heldItem = i.item;
				this.hand.GetComponent<SpriteRenderer>().sprite = this.heldItem.iSprite;
				GameObject.Destroy(collision.gameObject);

				this.audioSource.PlayOneShot(ITEM_PICKUP);
			}
		}
    }

 
}
