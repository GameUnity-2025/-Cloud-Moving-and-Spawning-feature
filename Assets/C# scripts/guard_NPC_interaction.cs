using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guard_NPC_interaction : MonoBehaviour
{
    public string interactButtonName = "talk_text"; // Set this to the name of your interact button GameObject
    public float interactionRange = 1f; // Adjust this value based on your desired range

    private GameObject interactButton; // Reference to the UI interact button GameObject

    // Dialogue GameObject references
    public string textBubbleName = "text_bubble sprite_0";
    private GameObject text_bubble;
    private GameObject guard_dialogue_one;
    private GameObject guard_dialogue_two;

    // Tham chiếu đến Player components (Tìm trong Start)
    private GameObject player;
    private character_movement characterMovementVariable;
    private character_jump_movement characterJumpMovementVariable;


    void Start()
    {
        // 1. Tìm và ẩn UI đối thoại
        text_bubble = GameObject.Find(textBubbleName);
        guard_dialogue_one = GameObject.Find("guarddialogueOne");
        guard_dialogue_two = GameObject.Find("guarddialogueTwo");

        text_bubble.SetActive(false);
        guard_dialogue_one.SetActive(false);
        guard_dialogue_two.SetActive(false);

        // 2. Tìm nút tương tác UI
        interactButton = GameObject.Find(interactButtonName);

        if (interactButton == null)
        {
            Debug.LogError("Interact button not found. Make sure to set the correct name.");
        }

        // 3. Tìm Player và các Scripts của Player
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            characterMovementVariable = player.GetComponent<character_movement>();
            characterJumpMovementVariable = player.GetComponent<character_jump_movement>();
        }
        else
        {
            Debug.LogError("Player GameObject not found! Check if it has 'Player' tag.");
        }
    }


    void Update()
    {
        CheckPlayerDistance();

        // Kích hoạt đối thoại bằng phím I trên PC
        if (Input.GetKeyDown(KeyCode.I) && PlayerInRange())
        {
            OnTalkButtonPressed();
        }
    }

    // Kiểm tra xem Player có ở trong phạm vi tương tác không
    bool PlayerInRange()
    {
        if (player == null) return false;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        return distance <= interactionRange;
    }


    void CheckPlayerDistance()
    {
        if (PlayerInRange())
        {
            ShowInteractButton();
        }
        else
        {
            HideInteractButton();
            // Đảm bảo đối thoại bị ẩn nếu người chơi đi ra xa
            if (text_bubble.activeSelf)
            {
                EndDialogue();
                if (characterMovementVariable != null) characterMovementVariable.ToggleMovement(true);
                if (characterJumpMovementVariable != null) characterJumpMovementVariable.ToggleJumpMovement(true);
            }
        }
    }

    void ShowInteractButton()
    {
        if (interactButton != null)
        {
            interactButton.SetActive(true);
        }
    }

    void HideInteractButton()
    {
        if (interactButton != null)
        {
            interactButton.SetActive(false);
        }
    }

    // ====================================================================
    // 📢 HÀM CÔNG KHAI DÙNG CHO NÚT UI ONCLICK() VÀ PHÍM I
    // ====================================================================
    public void OnTalkButtonPressed()
    {
        // Kiểm tra phạm vi lần nữa (chủ yếu cho nút UI)
        if (!PlayerInRange())
        {
            return;
        }

        // Đảm bảo Player Scripts đã được tìm thấy
        if (characterMovementVariable == null || characterJumpMovementVariable == null)
        {
            Debug.LogError("Player Movement Scripts not found or Player not tagged correctly!");
            return;
        }

        // Logic Đối thoại
        // Tình huống 1: Bắt đầu đối thoại (Chưa có gì hiển thị)
        if (!text_bubble.activeSelf && !guard_dialogue_one.activeSelf && !guard_dialogue_two.activeSelf)
        {
            // Khóa di chuyển
            characterMovementVariable.ToggleMovement(false);
            characterJumpMovementVariable.ToggleJumpMovement(false);

            StartDialogue();
        }
        // Tình huống 2: Tiếp tục đối thoại (Đoạn 1 đang hiển thị)
        else if (text_bubble.activeSelf && guard_dialogue_one.activeSelf && !guard_dialogue_two.activeSelf)
        {
            ContinueDialogue();
        }
        // Tình huống 3: Kết thúc đối thoại (Đoạn 2 đang hiển thị)
        else if (text_bubble.activeSelf && !guard_dialogue_one.activeSelf && guard_dialogue_two.activeSelf)
        {
            EndDialogue();

            // Mở khóa di chuyển
            characterMovementVariable.ToggleMovement(true);
            characterJumpMovementVariable.ToggleJumpMovement(true);
        }
    }

    // Ẩn tất cả dialogue
    void ResetAllDialogues()
    {
        guard_dialogue_one.SetActive(false);
        guard_dialogue_two.SetActive(false);
    }

    void StartDialogue()
    {
        ResetAllDialogues();
        text_bubble.SetActive(true);
        guard_dialogue_one.SetActive(true);
    }

    void ContinueDialogue()
    {
        guard_dialogue_one.SetActive(false);
        guard_dialogue_two.SetActive(true);
    }

    void EndDialogue()
    {
        ResetAllDialogues();
        text_bubble.SetActive(false);
    }
}