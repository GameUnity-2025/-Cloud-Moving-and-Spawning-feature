using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chief_NPC_interaction : MonoBehaviour
{
    public string interactButtonName = "talk_text"; // Set this to the name of your interact button GameObject
    public float interactionRange = 1f; // Adjust this value based on your desired range

    private GameObject interactButton; // Reference to the UI interact button GameObject

    // Dialogue GameObject references
    public string textBubbleName = "text_bubble sprite_0";
    private GameObject text_bubble;
    private GameObject chief_dialogue_one;
    private GameObject chief_dialogue_two;
    private GameObject chief_dialogue_three;
    private GameObject chief_dialogue_four;
    private GameObject chief_dialogue_five;
    private GameObject chief_dialogue_six;

    // Biến trạng thái mới: Theo dõi đoạn hội thoại hiện tại (0: Bắt đầu, 1-6: Các đoạn, 7: Kết thúc)
    private int currentDialogueStep = 0;

    public bool completedinteraction;

    // Tham chiếu đến Player components (sẽ được tìm trong OnTalkButtonPressed hoặc Start)
    private character_movement characterMovementVariable;
    private character_jump_movement characterJumpMovementVariable;
    private GameObject player;


    void Start()
    {
        // ... (Giữ nguyên phần tìm GameObject và đặt trạng thái SetActive(false) ban đầu) ...
        text_bubble = GameObject.Find(textBubbleName);
        chief_dialogue_one = GameObject.Find("chiefdialogueOne");
        chief_dialogue_two = GameObject.Find("chiefdialogueTwo");
        chief_dialogue_three = GameObject.Find("chiefdialogueThree");
        chief_dialogue_four = GameObject.Find("chiefdialogueFour");
        chief_dialogue_five = GameObject.Find("chiefdialogueFive");
        chief_dialogue_six = GameObject.Find("chiefdialogueSix");

        // Tắt tất cả UI đối thoại khi bắt đầu
        text_bubble.SetActive(false);
        chief_dialogue_one.SetActive(false);
        chief_dialogue_two.SetActive(false);
        chief_dialogue_three.SetActive(false);
        chief_dialogue_four.SetActive(false);
        chief_dialogue_five.SetActive(false);
        chief_dialogue_six.SetActive(false);

        completedinteraction = false;

        interactButton = GameObject.Find(interactButtonName);

        if (interactButton == null)
        {
            Debug.LogError("Interact button not found. Make sure to set the correct name.");
        }

        // Lấy tham chiếu Player và Scripts
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            characterMovementVariable = player.GetComponent<character_movement>();
            characterJumpMovementVariable = player.GetComponent<character_jump_movement>();
        }
    }


    void Update()
    {
        CheckPlayerDistance();

        // Xử lý Input PC (Phím I) bằng cách gọi hàm OnTalkButtonPressed
        if (Input.GetKeyDown(KeyCode.I) && PlayerInRange())
        {
            OnTalkButtonPressed();
        }
    }


    void CheckPlayerDistance()
    {
        if (player == null || characterMovementVariable == null || characterJumpMovementVariable == null) return;

        if (PlayerInRange())
        {
            ShowInteractButton();
        }
        else
        {
            HideInteractButton();
        }
    }

    // Hàm kiểm tra khoảng cách
    bool PlayerInRange()
    {
        if (player == null) return false;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        return distance <= interactionRange;
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

            // Nếu người chơi đi ra ngoài phạm vi, tắt đối thoại đang hiển thị (nếu có)
            if (text_bubble.activeSelf)
            {
                EndDialogueCleanUp();

                // Mở khóa di chuyển khi kết thúc đối thoại đột ngột
                if (characterMovementVariable != null) characterMovementVariable.ToggleMovement(true);
                if (characterJumpMovementVariable != null) characterJumpMovementVariable.ToggleJumpMovement(true);
            }
        }
    }

    // ====================================================================
    // 📢 HÀM CÔNG KHAI DÙNG CHO NÚT UI ONCLICK() VÀ PHÍM I
    // ====================================================================
    public void OnTalkButtonPressed()
    {
        if (!PlayerInRange())
        {
            return; // Không làm gì nếu người chơi không ở trong phạm vi
        }

        // Đảm bảo Player Scripts đã được tìm thấy
        if (characterMovementVariable == null || characterJumpMovementVariable == null)
        {
            Debug.LogError("Player Movement Scripts not found!");
            return;
        }

        // Khóa di chuyển khi bắt đầu đối thoại (Chỉ khóa 1 lần ở bước 0)
        if (currentDialogueStep == 0)
        {
            characterMovementVariable.ToggleMovement(false);
            characterJumpMovementVariable.ToggleJumpMovement(false);
        }

        currentDialogueStep++; // Tăng bước đối thoại

        // Gọi hàm tương ứng với bước hiện tại
        switch (currentDialogueStep)
        {
            case 1:
                StartDialogue();
                break;
            case 2:
                SecondDialogue();
                break;
            case 3:
                ThirdDialogue();
                break;
            case 4:
                FourthDialogue();
                break;
            case 5:
                FifthDialogue();
                break;
            case 6:
                SixthDialogue();
                break;
            case 7:
                EndDialogueCleanUp();

                // Mở khóa di chuyển khi kết thúc đối thoại
                characterMovementVariable.ToggleMovement(true);
                characterJumpMovementVariable.ToggleJumpMovement(true);

                completedinteraction = true; // Đánh dấu tương tác hoàn tất
                currentDialogueStep = 0; // Đặt lại trạng thái để có thể tương tác lại (nếu muốn)
                break;
        }
    }

    // Hàm ẩn tất cả đối thoại
    void ResetAllDialogues()
    {
        chief_dialogue_one.SetActive(false);
        chief_dialogue_two.SetActive(false);
        chief_dialogue_three.SetActive(false);
        chief_dialogue_four.SetActive(false);
        chief_dialogue_five.SetActive(false);
        chief_dialogue_six.SetActive(false);
    }

    void EndDialogueCleanUp()
    {
        ResetAllDialogues();
        text_bubble.SetActive(false);
    }


    // Call this method for the first dialogue
    void StartDialogue()
    {
        ResetAllDialogues(); // Tắt các đoạn khác (chỉ để đề phòng lỗi)
        text_bubble.SetActive(true);
        chief_dialogue_one.SetActive(true);
    }

    // Call this method when the first dialogue is complete
    void SecondDialogue()
    {
        chief_dialogue_one.SetActive(false);
        chief_dialogue_two.SetActive(true);
    }

    // ... (Giữ nguyên các hàm ThirdDialogue, FourthDialogue, FifthDialogue, SixthDialogue) ...
    void ThirdDialogue()
    {
        chief_dialogue_two.SetActive(false);
        chief_dialogue_three.SetActive(true);
    }

    void FourthDialogue()
    {
        chief_dialogue_three.SetActive(false);
        chief_dialogue_four.SetActive(true);
    }

    void FifthDialogue()
    {
        chief_dialogue_four.SetActive(false);
        chief_dialogue_five.SetActive(true);
    }

    void SixthDialogue()
    {
        chief_dialogue_five.SetActive(false);
        chief_dialogue_six.SetActive(true);
    }
}