using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = System.Random;

public class Player1 : MonoBehaviour
{
    public float moveX;
    public float maxForce = 10f;
    public float maxSpeed = 1f;
    private int lives;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Canvas Scorecanvas;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject[] heartPrefab;

    //added for mobile
    [SerializeField] private Canvas moveJoyStickCanvas;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button upButton;

    private string WALK_ANIMATION = "Walk";
    private string GROUNDED = "Ground";
    private string ENEMY = "Enemy";

    private bool isGrounded = true;
    private int score;
    [SerializeField] private GameObject coin;

    //for mobile buttons
    private bool buttonLeft = false;
    private bool buttonRight = false;
    private bool buttonUp = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        moveJoyStickCanvas = GameObject.Find("MoveCanvas").GetComponent<Canvas>();
        Scorecanvas = GameObject.Find("ScoreCanvas").GetComponent<Canvas>();
        score = 0;
        scoreText = Scorecanvas.transform.Find("Score").GetComponent<Text>();
        coin = GameObject.FindWithTag("Coin");
        lives = 3;

        // Initialize hearts
        heartPrefab[0] = GameObject.Find("ScoreCanvas/H1");
        heartPrefab[1] = GameObject.Find("ScoreCanvas/H2");
        heartPrefab[2] = GameObject.Find("ScoreCanvas/H3");

        // Setup UI buttons
        SetupUIButtons();

        UpdateScoreDisplay();
        StartCoroutine(CoinMaker());
    }

    private void SetupUIButtons()
    {
        // Find and setup the UI buttons
        leftButton = moveJoyStickCanvas.transform.Find("Left").GetComponent<Button>();
        rightButton = moveJoyStickCanvas.transform.Find("Right").GetComponent<Button>();
        upButton = moveJoyStickCanvas.transform.Find("Up").GetComponent<Button>();

        if (leftButton != null)
        {
            EventTrigger leftTrigger = leftButton.gameObject.GetComponent<EventTrigger>();
            if (leftTrigger == null)
                leftTrigger = leftButton.gameObject.AddComponent<EventTrigger>();
            
            leftTrigger.triggers.Clear();

            // Down event
            EventTrigger.Entry pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((data) => { OnLeftButtonDown(); });
            leftTrigger.triggers.Add(pointerDown);

            // Up event
            EventTrigger.Entry pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((data) => { OnLeftButtonUp(); });
            leftTrigger.triggers.Add(pointerUp);
        }

        if (rightButton != null)
        {
            EventTrigger rightTrigger = rightButton.gameObject.GetComponent<EventTrigger>();
            if (rightTrigger == null)
                rightTrigger = rightButton.gameObject.AddComponent<EventTrigger>();
            
            rightTrigger.triggers.Clear();

            // Down event
            EventTrigger.Entry pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((data) => { OnRightButtonDown(); });
            rightTrigger.triggers.Add(pointerDown);

            // Up event
            EventTrigger.Entry pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((data) => { OnRightButtonUp(); });
            rightTrigger.triggers.Add(pointerUp);
        }

        if (upButton != null)
        {
            EventTrigger upTrigger = upButton.gameObject.GetComponent<EventTrigger>();
            if (upTrigger == null)
                upTrigger = upButton.gameObject.AddComponent<EventTrigger>();
            
            upTrigger.triggers.Clear();

            // Down event
            EventTrigger.Entry pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((data) => { OnUpButtonDown(); });
            upTrigger.triggers.Add(pointerDown);

            // Up event
            EventTrigger.Entry pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((data) => { OnUpButtonUp(); });
            upTrigger.triggers.Add(pointerUp);
        }
    }

    // Button control methods - can be used directly from Unity UI button events
    public void OnLeftButtonDown()
    {
        buttonLeft = true;
    }

    public void OnLeftButtonUp()
    {
        buttonLeft = false;
    }

    public void OnRightButtonDown()
    {
        buttonRight = true;
    }

    public void OnRightButtonUp()
    {
        buttonRight = false;
    }

    public void OnUpButtonDown()
    {
        buttonUp = true;
        if (isGrounded) {
            isGrounded = false;
            rb.AddForce(new Vector2(0f, maxForce), ForceMode2D.Impulse);
        }
    }

    public void OnUpButtonUp()
    {
        buttonUp = false;
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
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);

        if (jumpPressed && isGrounded)
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
        // Set moveX based on keyboard or UI buttons
        if (Input.GetAxisRaw("Horizontal") != 0)
            moveX = Input.GetAxisRaw("Horizontal");
        else if (buttonRight)
            moveX = 1;
        else if (buttonLeft)
            moveX = -1;
        else
            moveX = 0;

        if (transform.position.x > -65f && transform.position.x < 230f)
        {
            rb.transform.position += new Vector3(moveX, 0f, 0f) * maxSpeed * Time.deltaTime;
        }
        else
        {
            if (moveX < 0)
            {
                rb.transform.position += new Vector3(moveX+3, 0f, 0f) * maxSpeed * Time.deltaTime;
            }
            else if (moveX > 0)
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