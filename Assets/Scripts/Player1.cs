using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    public float moveX;
    public float maxForce = 14f;
    public float maxSpeed = 11f;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Canvas canvas;
    
    private string WALK_ANIMATION = "Walk";
    private string GROUNDED = "Ground";
    private string ENEMY = "Enemy";
    
    private bool isGrounded = true;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        
        
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(GROUNDED))
        {
            isGrounded = true;
        }
        if(other.gameObject.CompareTag(ENEMY))
        {
            canvas.sortingOrder = 2;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(ENEMY))
        {
            canvas.sortingOrder = 2;
            Destroy(gameObject);
        }
    }
    
}
