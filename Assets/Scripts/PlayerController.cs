﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Item heldItem;

    [SerializeField] private float SPEED = 64.0f;
    [SerializeField] private float ATTACK_COOLDOWN = 0.5f;
    [SerializeField] private float THROW_SPEED = 16.0f;

    [SerializeField] private float DAMPENING = 0.01f;

    [SerializeField] private float MAX_HEALTH = 3;

    [SerializeField] private float KNOCKBACK = 16.0f;

    [SerializeField] private GameObject DROPPED_ITEM_PREFAB;
    [SerializeField] private Item EMPTY_ITEM;
    [SerializeField] private LayerMask ENEMY_LAYER_MASK;

    //Audio
    [SerializeField] private AudioClip ITEM_THROW;
    [SerializeField] private AudioClip ITEM_PICKUP;
    [SerializeField] private AudioClip DOOR_UNLOCK;
    [SerializeField] private AudioClip ENEMY_HIT;
    [SerializeField] private AudioClip SWORD_SWING;
    [SerializeField] private AudioClip PORTAL_ACTIVATE;


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
        if(health <= 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
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
        this.CheckShop(collision);
        this.CheckPortal(collision);
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

                if (this.heldItem is BlockCasterItem)
                {
                    BlockCasterItem item = (BlockCasterItem)heldItem;
                    GameObject g = GameObject.Instantiate(item.bBlockPrefab);

                    Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    g.transform.position = v;
                }
                else if (this.heldItem is RangeItem)
                {
                    RangeItem item = (RangeItem)heldItem;
                    //TODO

                }
                else if (this.heldItem is AttackItem)
                {
					audioSource.PlayOneShot(SWORD_SWING);
                    AttackItem item = (AttackItem)heldItem;
                    Attack(transform, item.aRange, item.aDamage);
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

    /// <summary>
    /// Causes enemies within the attack radius to take damage equal to the damage amount
    /// </summary>
    /// <param name="attackPosition">The transform of the attack</param>
    /// <param name="attackRadius">The radius of the attack</param>
    /// <param name="damage">The damage the target(s) will take from the attack</param>
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
            Debug.Log(health);

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

    //Dunno if this is the way you want to do them so feel free to change.
    private void CheckPortal(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Portal"))
        {
            Debug.Log("Portal Activating..");
            collision.gameObject.GetComponent<PortalController>().Teleport();
			AudioSource.PlayClipAtPoint(PORTAL_ACTIVATE, this.transform.position, 2.0f);
        }
    }

    private void CheckShop(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Shop") &&
            heldItem.iName == collision.gameObject.GetComponent<ShopController>().cost.iName)
        {
            if (collision.gameObject.GetComponent<ShopController>().ammount != 0)
            { //The shop will vend the item directly to player's hand
                this.heldItem = collision.gameObject.GetComponent<ShopController>().vend;
                if (this.heldItem == null) this.heldItem = EMPTY_ITEM;

                this.hand.GetComponent<SpriteRenderer>().sprite = this.heldItem.iSprite;

                collision.gameObject.GetComponent<ShopController>().ammount--;

                this.audioSource.PlayOneShot(ITEM_PICKUP);

            }
            if (collision.gameObject.GetComponent<ShopController>().ammount == 0)
            { //Then checks if shop should be destroyed
                Destroy(collision.gameObject);
            }

        }
    }


}
