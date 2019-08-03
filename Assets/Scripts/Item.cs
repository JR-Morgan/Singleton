using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName="Game Item")]
public class Item : ScriptableObject
{
	public enum ItemType
	{
		Sword,
		Coin,
		Key,
		Bomb
	}

    public string name;

    public Sprite sprite;

	public ItemType type;
}
