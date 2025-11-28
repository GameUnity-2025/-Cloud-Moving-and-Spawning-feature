using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class legendary_sword_interaction : MonoBehaviour
{
    public string interactButtonName = "talk_text"; // Set this to the name of your interact button GameObject
    public float interactionRange = 1f; // Adjust this value based on your desired range

    private GameObject interactButton; // Reference to the UI interact button GameObject

    // Dialogue GameObject references
    private GameObject dialogue_one;
    private GameObject dialogue_two;
    private GameObject dialogue_three;

    // Tham chiếu đến Player components (Tìm trong Start)
    private GameObject player;
    private character_movement characterMovementVariable;
    private character_jump_movement characterJumpMovementVariable;


    void Start()
    {
        // 1. Tìm và ẩn UI đối thoại
        dialogue_one = GameObject.Find("congrats_game_over_text");
        dialogue_two = GameObject.Find("legendary_sword_text");
        dialogue_three = GameObject.Find("thanks_for_playing_text");

        // Đảm bảo tất cả đều ẩn khi bắt đầu
        dialogue_one.SetActive(false);
        dialogue_two.SetActive(false);
        dialogue_three.SetActive(false);

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
            // Nếu người chơi đi ra ngoài phạm vi khi đang đối thoại, kết thúc đối thoại và mở khóa di chuyển
            if (dialogue_one.activeSelf || dialogue_two.activeSelf || dialogue_three.activeSelf)
            {
                CleanupDialogue();
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
        // Kiểm tra phạm vi
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
        if (!dialogue_one.activeSelf && !dialogue_two.activeSelf && !dialogue_three.activeSelf)
        {
            // Khóa di chuyển
            characterMovementVariable.ToggleMovement(false);
            characterJumpMovementVariable.ToggleJumpMovement(false);

            StartDialogue(); // Kích hoạt dialogue_one
        }
        // Tình huống 2: Tiếp tục đối thoại (dialogue_one đang hiển thị)
        else if (dialogue_one.activeSelf)
        {
            ContinueToTwo(); // Chuyển sang dialogue_two
        }
        // Tình huống 3: Tiếp tục đối thoại (dialogue_two đang hiển thị)
        else if (dialogue_two.activeSelf)
        {
            ContinueToThree(); // Chuyển sang dialogue_three
        }
        // Tình huống 4: Kết thúc đối thoại (dialogue_three đang hiển thị)
        else if (dialogue_three.activeSelf)
        {
            CleanupDialogue(); // Ẩn tất cả dialogue

            // Mở khóa di chuyển
            characterMovementVariable.ToggleMovement(true);
            characterJumpMovementVariable.ToggleJumpMovement(true);
        }
    }

    // Ẩn tất cả dialogue
    void ResetAllDialogues()
    {
        dialogue_one.SetActive(false);
        dialogue_two.SetActive(false);
        dialogue_three.SetActive(false);
    }

    // Bắt đầu (Dialogue 1)
    void StartDialogue()
    {
        ResetAllDialogues();
        dialogue_one.SetActive(true);
    }

    // Chuyển sang Dialogue 2
    void ContinueToTwo()
    {
        dialogue_one.SetActive(false);
        dialogue_two.SetActive(true);
    }

    // Chuyển sang Dialogue 3 (Final Message)
    void ContinueToThree()
    {
        dialogue_two.SetActive(false);
        dialogue_three.SetActive(true);
    }

    // Kết thúc và dọn dẹp (Ẩn tất cả)
    void CleanupDialogue()
    {
        ResetAllDialogues();
    }
}