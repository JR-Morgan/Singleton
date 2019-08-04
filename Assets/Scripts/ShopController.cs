using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    //The item used to buy
    public Item cost;
    //The item selling
    public Item vend;
    // The ammount of this item before shop is destroyed
    [Range(-1, 20)]  public int ammount;
}
