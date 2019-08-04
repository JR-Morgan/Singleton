using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject blockPrefab;
    public Vector2 position;
  

    [SerializeField] private PuzzleTrigger[] triggers;
    [SerializeField] private float COOLDOWN = 1f;

    private float cooldown;


    private void Update()
    {
        cooldown -= Time.deltaTime;
        bool isUnlocked = true;


        foreach (PuzzleTrigger p in triggers)
        {
            if (!p.isUnlocked)
            {
                isUnlocked = false;
                break;
            }

            if (isUnlocked && cooldown <= 0)
            {
                Instantiate(blockPrefab, position, Quaternion.identity);
                cooldown = COOLDOWN;
            }
        }
    }
}
