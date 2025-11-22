using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    public float moveSpeed = 5f;
    public float jumpForce = 6f;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void MoveLeft()
    {
        rigidbody2D.velocity = new Vector2(-moveSpeed, rigidbody2D.velocity.y);
    }

    public void MoveRight()
    {
        rigidbody2D.velocity = new Vector2(moveSpeed, rigidbody2D.velocity.y);
    }

    public void Jump()
    {
        rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void Update()
    {
    }
}
