using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
	private PuzzleTrigger pt;
	private GameObject activator;

	// Start is called before the first frame update
	void Start()
	{
		pt = this.GetComponent<PuzzleTrigger>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		Debug.Log("Unlocked by: " + collision.gameObject.name);

		if (this.activator == null)
		{
			this.activator = collision.gameObject;
			this.pt.isUnlocked = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Debug.Log("locked by: " + collision.gameObject.name);

		if (collision.gameObject == this.activator)
		{
			this.activator = null;
			this.pt.isUnlocked = false;
		}
	}
}
