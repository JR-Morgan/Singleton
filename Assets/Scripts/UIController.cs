using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
	[SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
		{
			player = GameObject.FindGameObjectWithTag("Player");
		}
    }

    // Update is called once per frame
    void Update()
    {
		Item i = player.GetComponent<PlayerController>().heldItem;

		if (i != null || i.iName.Equals("")  )
		{
			this.GetComponent<UnityEngine.UI.Image>().color = Color.clear;
		}
		else
		{
			this.GetComponent<UnityEngine.UI.Image>().color = Color.white;
		}

		this.GetComponent<UnityEngine.UI.Image>().sprite = player.GetComponent<PlayerController>().heldItem.iSprite;
    }
}
