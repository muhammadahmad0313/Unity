using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollector : MonoBehaviour
{
    private string ENEMY = "Enemy";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(ENEMY)) Destroy(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag(ENEMY)) Destroy(other.gameObject);
    }
}
