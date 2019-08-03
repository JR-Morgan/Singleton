﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor : MonoBehaviour
{
	[SerializeField] private PuzzleTrigger[] triggers;

    // Start is called before the first frame update
    void Start()
    {
        
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
			GameObject.Destroy(this.gameObject);
		}
    }
}