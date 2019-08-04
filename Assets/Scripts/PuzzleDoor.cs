using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor : MonoBehaviour
{
	[SerializeField] private PuzzleTrigger[] triggers;
    [SerializeField] private GameObject[] activateOnTrigger;
    [SerializeField] private AudioClip DOOR_UNLOCK;
    [SerializeField] private bool STAY_UNLOCKED;
    private AudioSource audioSource;
    private Vector2 closedPos;
    private bool lastState = false;


    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
        closedPos = transform.position;
    }

    private void toggleUnlocked(bool isUnlocked)
    {
        this.audioSource.PlayOneShot(DOOR_UNLOCK);

        if (isUnlocked)
        {
            foreach (GameObject g in activateOnTrigger)
            {
                g.SetActive(true);
            }
            transform.SetPositionAndRotation(new Vector2(255, 255), Quaternion.identity);

        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isUnlocked = true;
		

		foreach (PuzzleTrigger p in triggers)
		{
			if (!p.isUnlocked)
			{
                transform.SetPositionAndRotation(closedPos, Quaternion.identity);
                isUnlocked = false;
				break;
			}
		}
        if(STAY_UNLOCKED && isUnlocked)
        {
            Destroy(gameObject);
        }

        if(lastState != isUnlocked)
        {
            lastState = isUnlocked;
            toggleUnlocked(isUnlocked);
        }

    }
}
