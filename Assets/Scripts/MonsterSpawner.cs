using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    
    [SerializeField] private GameObject[] monsters;
    [SerializeField] private Transform left, right;
    private GameObject monster;
    
    private int randomSide;
    private int randomMonster; //0,1,2
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnMonster());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnMonster()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1,5));
        
            randomSide = Random.Range(0, 2);
            randomMonster = Random.Range(0, monsters.Length);
            monster = Instantiate(monsters[randomMonster]);
        
            //left
            if (randomSide == 0)
            {
                monster.transform.position = left.position;
                monster.GetComponent<Monster>().speed = Random.Range(1f, 10f);
            }
        
            //right
            else
            {
                monster.transform.position = right.position;
                monster.GetComponent<Monster>().speed = -Random.Range(1f, 10f);
                monster.GetComponent<SpriteRenderer>().flipX = true;
                // monster.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }

    }
}
