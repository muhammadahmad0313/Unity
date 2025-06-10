using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Player1 : MonoBehaviour
{
    public float moveX;
    public float maxForce = 14f;
    public float maxSpeed = 11f;
    private int lives;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Canvas Scorecanvas;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject[] heartPrefab;
    
    private string WALK_ANIMATION = "Walk";
    private string GROUNDED = "Ground";
    private string ENEMY = "Enemy";
    
    private bool isGrounded = true;

    private int score;

    [SerializeField] private GameObject coin;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        Scorecanvas = GameObject.Find("ScoreCanvas").GetComponent<Canvas>();
        score = 0;
        scoreText = Scorecanvas.transform.Find("Score").GetComponent<Text>();
        coin = GameObject.FindWithTag("Coin");
        lives = 3;
        // Initialize hearts
        
            heartPrefab[0]= GameObject.Find("ScoreCanvas/H1");
            heartPrefab[1]= GameObject.Find("ScoreCanvas/H2");
            heartPrefab[2]= GameObject.Find("ScoreCanvas/H3");
        
        
        UpdateScoreDisplay();
        StartCoroutine(CoinMaker());
        
    }
    
    // Update is called once per frame
    void Update()
    {
        Movement();
        Animation();
    }

    private void FixedUpdate()
    {
        PlayerJump();
        
    }
    
    public void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            rb.AddForce(new Vector2(0f, maxForce), ForceMode2D.Impulse);
        }
    }

    IEnumerator CoinMaker()
    {
        GameObject originalCoin = coin;
        while (true)
        {
            
            yield return new WaitForSeconds(UnityEngine.Random.Range(1,3));
            
            GameObject newcoin = Instantiate(originalCoin);
            newcoin.transform.position = new Vector3(UnityEngine.Random.Range(-65f, 250f), UnityEngine.Random.Range(-2f, 5f), 0f);
        }
        
    }
    
    public void Movement()
    {
        if (transform.position.x > -65f && transform.position.x < 230f)
        {
            moveX = Input.GetAxisRaw("Horizontal");
            rb.transform.position += new Vector3(moveX, 0f, 0f) * maxSpeed * Time.deltaTime;
        }
        else
        {
            if (moveX < 0)
            {
                rb.transform.position += new Vector3(moveX+3, 0f, 0f) * maxSpeed * Time.deltaTime;
            }
            else
            {
                rb.transform.position += new Vector3(moveX-3, 0f, 0f) * maxSpeed * Time.deltaTime;
            }
        }
    }
    
    public void Animation()
    {
        
        //right
        if (moveX > 0)
        {
            sprite.flipX = false;
            anim.SetBool(WALK_ANIMATION, true);
        }
        
        //left
        else if (moveX < 0)
        {
            sprite.flipX = true;
            anim.SetBool(WALK_ANIMATION, true);
        }

        //idle
        else
        {
            anim.SetBool(WALK_ANIMATION, false);
        }
    }
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    private void updateHearts()
    {
        int i = 3 - lives;
        while (i>0)
        {
            i--;
            heartPrefab[heartPrefab.Length - 1 - i].SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(GROUNDED))
        {
            isGrounded = true;
        }
        if(other.gameObject.CompareTag(ENEMY))
        {
           if(lives-1 > 0) 
           {
               Destroy(other.gameObject);
               lives--;
               updateHearts();
           }
           else
           {
               canvas.sortingOrder = 2;
               MonsterSpawner.isGameOver = true;
               lives--;
               updateHearts();
               Destroy(gameObject);
               }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            score++;
            UpdateScoreDisplay();
            Destroy(other.gameObject);
        }
    }
    
}
