using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollector : MonoBehaviour
{
    private string ENEMY = "Enemy";

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag(ENEMY)) Destroy(other.gameObject);
    }
}
