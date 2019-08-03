﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Game Item/Attack Item")]
public class AttackItem : Item
{
    public int aDamage;
    public float aRange;
}