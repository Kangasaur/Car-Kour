using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float accel;
    public float returnAccel;
    public float maxSpeed;
    public float minSpeed;
    public float jumpHeight;
    float speed;
    float currAccel;
    float dir = 1;
    bool turning = false;

    Rigidbody2D myBody;
    SpriteRenderer mySprite;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (Mathf.Abs(speed) > minSpeed) speed -= returnAccel * dir * Time.deltaTime;
        else speed += returnAccel * dir * Time.deltaTime;
        currAccel = Input.GetAxis("Horizontal") * accel * Time.deltaTime;
        speed += currAccel;
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
        dir = Mathf.Sign(speed);
        if (dir == -1) mySprite.flipX = false;
        else mySprite.flipX = true;
        myBody.velocity = new Vector2(speed, myBody.velocity.y);
    }
}
