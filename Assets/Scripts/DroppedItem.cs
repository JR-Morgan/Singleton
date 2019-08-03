using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Item item;

	[SerializeField] private float DEFAULT_PICKUP_TIME = 0.25f;

	[SerializeField] private float DAMPENING = 0.01f;

	private float pickupTime;

    // Start is called before the first frame update
    void Start()
    {
		this.GetComponent<SpriteRenderer>().sprite = this.item.iSprite;
		this.pickupTime = DEFAULT_PICKUP_TIME;
    }

    // Update is called once per frame
    void Update()
    {
		this.pickupTime -= Time.deltaTime;

		if (pickupTime <= 0.0f)
		{
			//Once the item cam be picked up again, set the layer to 
			//item so it can be detected by layer player and set 
			//isTrigger to true to it won't interfere with collision
			this.gameObject.layer = LayerMask.NameToLayer("Item");
			this.GetComponent<BoxCollider2D>().isTrigger = true;
		}
    }

	private void FixedUpdate()
	{
		this.GetComponent<Rigidbody2D>().velocity *= Mathf.Pow(DAMPENING, Time.deltaTime);
	}
}
