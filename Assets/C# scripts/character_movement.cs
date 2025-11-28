using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_movement : MonoBehaviour
{
    private SpriteRenderer spriteRendererComponent;

    private float moveSpeed = 7f;
    private bool canMove = true;

    // Hướng di chuyển khi dùng nút UI
    private float moveDirection = 0f;

    public bool isfacingright = true;

    void Start()
    {
        spriteRendererComponent = GetComponent<SpriteRenderer>();

        if (spriteRendererComponent == null)
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject.");
        }
    }

    void Update()
    {
        if (!canMove) return;

        float input = 0f;

        // Ưu tiên bàn phím nếu có nhấn, còn không thì dùng UI
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            input = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            input = 1f;
        }
        else
        {
            input = moveDirection;
        }

        // Di chuyển
        if (input != 0)
        {
            transform.Translate(Vector3.right * input * moveSpeed * Time.deltaTime);

            // Flip sprite theo hướng
            if (input < 0)
            {
                FlipSprite(true);
                isfacingright = false;
            }
            else
            {
                FlipSprite(false);
                isfacingright = true;
            }
        }
    }

    public void ToggleMovement(bool allowMovement)
    {
        canMove = allowMovement;
    }

    private void FlipSprite(bool flip)
    {
        spriteRendererComponent.flipX = flip;
    }

    // ========== HÀM DÙNG CHO NÚT UI ==========

    // Nhấn giữ nút trái
    public void OnMoveLeftButtonDown()
    {
        moveDirection = -1f;
    }

    // Nhấn giữ nút phải
    public void OnMoveRightButtonDown()
    {
        moveDirection = 1f;
    }

    // Nhả nút → dừng di chuyển
    public void OnMoveButtonUp()
    {
        moveDirection = 0f;
    }
}
