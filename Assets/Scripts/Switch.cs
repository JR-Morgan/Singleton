using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
	private PuzzleTrigger pt;
	private GameObject activator;
	private AudioSource audioSource;

	[SerializeField] private AudioClip SWITCH_TOGGLE;

	// Start is called before the first frame update
	void Start()
	{
		pt = this.GetComponent<PuzzleTrigger>();
		this.audioSource = this.GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		//Debug.Log("Unlocked by: " + collision.gameObject.name);

		if (this.activator == null)
		{
			this.activator = collision.gameObject;
			this.pt.isUnlocked = true;
			this.audioSource.PlayOneShot(SWITCH_TOGGLE, 0.5f);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		//Debug.Log("locked by: " + collision.gameObject.name);

		if (collision.gameObject == this.activator)
		{
			this.activator = null;
			this.pt.isUnlocked = false;
			//audio.PlayOneShot(SWITCH_TOGGLE);
		}
	}
}
