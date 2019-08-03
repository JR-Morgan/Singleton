using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName="Game Item/Item")]
public class Item : ScriptableObject
{
	public string iName;
	public string iDescription;

    public Sprite iSprite;
}