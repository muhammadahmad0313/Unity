using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Transform player;
    private Vector3 offset;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!player) return;
        if (player.position.x > -65f && player.position.x < 230f)
        {
            offset = transform.position;
            offset.x = player.position.x;
        
            transform.position = offset;
        }
    }
}
