using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor : MonoBehaviour
{
	[SerializeField] private PuzzleTrigger[] triggers;
    [SerializeField] private GameObject[] activateOnTrigger;
    [SerializeField] private AudioClip DOOR_UNLOCK;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		bool isUnlocked = true;

		foreach (PuzzleTrigger p in triggers)
		{
			if (!p.isUnlocked)
			{
				isUnlocked = false;
				break;
			}
		}

		if (isUnlocked)
		{
            this.audioSource.PlayOneShot(DOOR_UNLOCK);
            foreach (GameObject g in activateOnTrigger)
            {
                g.SetActive(true);
            }

            GameObject.Destroy(this.gameObject);
		}
    }
}
