using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName="Game Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string itemDescription;

    public Sprite sprite;

    public int attack;

    public bool isKey;
}
