using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_jump_movement : MonoBehaviour
{
    private float jumpForce = 5f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;

    private Rigidbody2D rb;

    private bool canJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        canJump = true;
    }

    void Update()
    {
        // Check if the character is grounded
        bool isGrounded = IsGrounded();

        print(isGrounded);

        // Jump when the space key is pressed and the character is grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            Jump();
        }
    }

    // Checks if the character is in contact with the ground
    bool IsGrounded()
    {
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;

        Ray2D ray = new Ray2D(new Vector2(bounds.center.x, bounds.min.y), Vector2.down);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, bounds.size.y / 2f, groundLayer);

        return hit.collider != null;
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    // Method to toggle movement on/off (for in the 'interaction' C# scripts)
    public void ToggleJumpMovement(bool allowMovement)
    {
        canJump = allowMovement;
    }

    // ✔ HÀM SỬ DỤNG CHO BUTTON UI
    public void OnJumpButtonClick()
    {
        if (IsGrounded() && canJump)
        {
            Jump();
        }
    }
}
