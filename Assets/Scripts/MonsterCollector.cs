using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollector : MonoBehaviour
{
    private string ENEMY = "Enemy";
    // private string GHOST_ENEMY = "GhostEnemy";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     Debug.Log($"Trigger entered with: {other.gameObject.name}, tag: {other.tag}");
    //
    //     if (other.CompareTag(GHOST_ENEMY))
    //     {
    //         Debug.Log("Ghost Enemy Destroyed");
    //         Destroy(other.gameObject);
    //     }
    // }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag(ENEMY)) Destroy(other.gameObject);
    }
}
