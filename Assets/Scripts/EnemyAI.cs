using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	[SerializeField] private float SPEED = 16.0f;

	[SerializeField] private float DAMPENING = 0.01f;

    [SerializeField] private int MAX_HEALTH = 2;

	[SerializeField] private GameObject DROPPED_ITEM_PREFAB;

	[SerializeField] private AudioClip ENEMY_DEATH_CLIP;

	[SerializeField] private Item heldItem;

    
    private Animator animator;
	private Rigidbody2D rb;

	private float aiDelta;
	private Vector2 acceleration;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
		this.animator = this.GetComponent<Animator>();
		this.rb = this.GetComponent<Rigidbody2D>();
		this.aiDelta = 0.0f;
		this.acceleration = new Vector2();

        health = MAX_HEALTH;
    }

    // Update is called once per frame
    void Update()
    {
		if (this.aiDelta <= 0.0f)
		{
			this.aiDelta = Random.Range(0.5f, 3.0f);
			this.acceleration = Random.insideUnitCircle;
			//this.acceleration.Normalize();
		}
		else
		{
			this.aiDelta -= Time.deltaTime;
		}

        if (health <= 0)
        {
            Debug.Log(this.ToString() + " has been killed");

			if (!this.heldItem.iName.Equals(""))
			{
				GameObject g = GameObject.Instantiate(DROPPED_ITEM_PREFAB);
				g.GetComponent<DroppedItem>().item = this.heldItem;
				g.GetComponent<Rigidbody2D>().velocity = this.rb.velocity;
				g.transform.position = this.transform.position;
			}

            GameObject.Destroy(gameObject);
			AudioSource.PlayClipAtPoint(ENEMY_DEATH_CLIP, this.transform.position);
        }

        this.rb.velocity += this.acceleration * Time.deltaTime * SPEED;
		this.rb.velocity *= Mathf.Pow(DAMPENING, Time.deltaTime);

		this.animator.SetFloat("Speed", this.rb.velocity.magnitude);
		this.transform.localScale = new Vector3((this.rb.velocity.x > 0) ? 1.0f : -1.0f, 1.0f, 1.0f);
	}

    /// <summary>
    /// Enemy health will be subtracted by the ammount
    /// </summary>
    /// <param name="ammount"></param>
    public void TakeDamage(int ammount)
    {
        health -= ammount;
        Debug.Log(this.ToString() + " has been attacked, new health " + health);
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();


    }
}
