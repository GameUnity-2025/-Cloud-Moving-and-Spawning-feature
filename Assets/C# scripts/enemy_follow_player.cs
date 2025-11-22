using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_follow_player : MonoBehaviour
{
    public string playerName = "bird character sprite";
    private GameObject player;

    public float moveSpeed = 1f;
    private Rigidbody2D rb;

    public float interactionRange = 5f;
    private float distance;
    private bool startfollowing;

    // Kiểm tra chạm đất cho enemy
    public Transform groundCheck;                // empty object ở dưới chân enemy
    public float groundCheckRadius = 0.12f;
    public LayerMask groundLayer;

    // Thông số nhảy của enemy để "nhảy theo" player
    public float jumpForce = 6f;                 // vận tốc theo trục Y khi nhảy
    public float verticalFollowThreshold = 0.6f; // nếu player cao hơn enemy bao nhiêu thì enemy nhảy

    void Start()
    {
        startfollowing = false;
        player = GameObject.Find(playerName);
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        Vector2 directionToPlayer = player.transform.position - transform.position;
        distance = directionToPlayer.magnitude;

        if (distance <= interactionRange)
        {
            startfollowing = true;
        }

        if (player != null && startfollowing)
        {
            // --- DI CHUYỂN NGANG: chỉ dùng thành phần X (không thay đổi Y)
            float horizontalDirection = Mathf.Sign(directionToPlayer.x); // -1 hoặc 1
            rb.velocity = new Vector2(horizontalDirection * moveSpeed, rb.velocity.y);

            // --- QUAY MẶT THEO HƯỚNG
            if (directionToPlayer.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (directionToPlayer.x < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            // --- NHẢY THEO PLAYER: nếu player cao hơn và enemy đang chạm đất -> nhảy bằng vật lý
            bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            float verticalDifference = player.transform.position.y - transform.position.y;

            if (isGrounded && verticalDifference > verticalFollowThreshold)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
    }

    // Vẽ gizmo vòng tròn groundCheck để dễ debug
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
