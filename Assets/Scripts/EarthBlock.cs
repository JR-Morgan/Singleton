using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBlock : MonoBehaviour
{
	[SerializeField] private AudioClip clip;
	[SerializeField] private AudioSource source;

    // Start is called before the first frame update
    void Start()
	{
		GameObject[] a = GameObject.FindGameObjectsWithTag("EarthBlock");

		foreach (GameObject g in a)
		{
			if (g != this.gameObject)
			{
				GameObject.Destroy(g);
			}
		}

		source.clip = clip;
		source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
