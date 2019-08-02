using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private static readonly int MAP_WIDTH;
	[SerializeField] private static readonly int MAP_HEIGHT;

	private GameObject[,] map;

    // Start is called before the first frame update
    void Start()
    {
		this.map = new GameObject[9, 9];

		for (int x = 0; x < MAP_WIDTH; x++)
		{
			for (int y = 0; y < MAP_WIDTH; y++)
			{

			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
