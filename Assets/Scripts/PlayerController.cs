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

    private Vector2 speed;
    private float attackDeltaT = 0f;

    void Start()
    {
        this.speed = new Vector2();
    }

    private void Update()
    {
        movementUpdate();
        attackUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        itemPickupcheck(collision);
    }

    private void attackUpdate()
    {

        transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, 33.333f);

        if (Input.GetButton("Attack"))
        {
            transform.GetChild(1).transform.rotation = new Quaternion(0, 0, 0, 0);
            attackDeltaT = ATTACK_SPEED;
        }
        if (attackDeltaT <= 0)
        {
            attackDeltaT -= Time.deltaTime;
        }


    }

    private void movementUpdate()
    {
        Vector2 acceleration = new Vector2();

        if (Input.GetKey(KeyCode.W)) acceleration.y += SPEED;
        if (Input.GetKey(KeyCode.S)) acceleration.y -= SPEED;
        if (Input.GetKey(KeyCode.A)) acceleration.x -= SPEED;
        if (Input.GetKey(KeyCode.D)) acceleration.x += SPEED;

        this.speed += acceleration * Time.deltaTime;
        this.transform.position += new Vector3(this.speed.x * Time.deltaTime, this.speed.y * Time.deltaTime, 0.0f);

        this.speed *= Mathf.Pow(DAMPENING, Time.deltaTime);
    }

    

    private void itemPickupcheck(Collision2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);

        if (collision.gameObject.tag == "Item")
        {
            if(item != null)
            {
                
            }
            item = collision.gameObject.GetComponent<DropedItem>().item;
            Destroy(collision.gameObject);
            transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = item.sprite;
        }
    }

 
}
