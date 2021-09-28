using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float accel;
    public float returnAccel;
    public float maxSpeed;
    public float minSpeed;
    public float turnAccel;
    public float jumpHeight;
    public Vector2 boostSpeed;
    public float boostAccel;
    public float boostTime;
    float speed;
    float currAccel;
    float dir = 1;
    float boostDir = 1;
    bool turning = false;
    bool onGround = false;
    bool boosting = false;
    bool braked = false;
    float boostCurrTime = 0f;

    Rigidbody2D myBody;
    SpriteRenderer mySprite;
    Animator animator;
    AudioSource sound;
    public AudioClip brakeSound;
    public AudioClip accelStart;
    public AudioClip accelLoop;
    public GameObject canvas;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        speed = minSpeed;
    }
    
    void Update()
    {
        if (turning) DoTurn();
        else DoHorizontalSpeed();
        HandleSound();
        myBody.velocity = new Vector2(speed, myBody.velocity.y);

        if (boosting)
        {
            boostCurrTime += Time.deltaTime;
            if (boostCurrTime > boostTime)
            {
                boosting = false;
                animator.SetBool("isBoosting", false);
            }
            else myBody.velocity = new Vector2(myBody.velocity.x, boostSpeed.y);
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, jumpHeight);
                onGround = false;
                animator.SetBool("isJumping", true);
            }
            else if (boostCurrTime == 0f)
            {
                boosting = true;
                animator.SetBool("isBoosting", true);
                if (Input.GetAxisRaw("Horizontal") != 0) boostDir = Input.GetAxisRaw("Horizontal");
                else if (!mySprite.flipX) boostDir = 1;
                else boostDir = -1;

                if (boostDir == 1) mySprite.flipX = false;
                else mySprite.flipX = true;
            }
        }
    }

    void DoTurn()
    {
        int turnDir = 1;
        if (mySprite.flipX) turnDir *= -1;
        speed += turnAccel * turnDir * Time.deltaTime;
        dir = Mathf.Sign(speed);
        if (Mathf.Abs(speed) > minSpeed)
        {
            turning = false;
            animator.SetBool("isTurning", false);
        }
    }

    void DoHorizontalSpeed()
    {
        if (Mathf.Abs(speed) > minSpeed)
        {
            speed -= returnAccel * dir * Time.deltaTime;
            if (Mathf.Abs(speed) < minSpeed) speed = minSpeed * dir;
        }
        currAccel = Input.GetAxis("Horizontal") * accel * Time.deltaTime;
        if (boosting) currAccel += boostAccel * boostDir;
        speed += currAccel;
        if (boosting) speed = Mathf.Clamp(speed, -(maxSpeed + boostSpeed.x), maxSpeed + boostSpeed.x);
        else speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
        dir = Mathf.Sign(speed);
        if (Mathf.Abs(speed) < minSpeed && !boosting)
        {
            turning = true;
            animator.SetBool("isTurning", true);
            if (!mySprite.flipX) mySprite.flipX = true;
            else mySprite.flipX = false;
        }
    }

    void HandleSound()
    {
        if (turning)
        {
            if (!braked)
            {
                sound.clip = brakeSound;
                sound.Play();
                sound.loop = false;
                braked = true;
            }
        }
        else if (!sound.isPlaying)
        {
            if (sound.clip == brakeSound)
            {
                sound.Stop();
                sound.clip = accelStart;
                sound.loop = false;
                braked = false;
                sound.Play();
            }
            else
            {
                sound.clip = accelLoop;
                sound.loop = true;
                sound.Play();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            onGround = true;
            animator.SetBool("isJumping", false);
            boostCurrTime = 0f;
            boosting = false;
            animator.SetBool("isBoosting", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            canvas.SendMessage("DoWin");
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            canvas.SendMessage("DoDeath");
        }
    }
}
