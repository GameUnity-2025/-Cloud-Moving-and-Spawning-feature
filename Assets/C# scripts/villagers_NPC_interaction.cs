using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class villagers_NPC_interaction : MonoBehaviour
{
    [Header("Interact Button (UI)")]
    public string interactButtonName = "talk_text";
    public float interactionRange = 1f;

    private GameObject interactButton;

    [Header("Dialogue Objects")]
    public string textBubbleName = "text_bubble sprite_0";
    public string villager_dialogue_one_name = "dialogueOne_text";
    public string villager_dialogue_two_name = "dialogueTwo_text";

    private GameObject text_bubble;
    private GameObject villager_dialogue_one;
    private GameObject villager_dialogue_two;

    private GameObject player;
    private character_movement movementScript;
    private character_jump_movement jumpScript;


    void Start()
    {
        // Find player reference
        player = GameObject.FindGameObjectWithTag("Player");
        movementScript = player.GetComponent<character_movement>();
        jumpScript = player.GetComponent<character_jump_movement>();

        // Find dialogue UI objects
        text_bubble = GameObject.Find(textBubbleName);
        villager_dialogue_one = GameObject.Find(villager_dialogue_one_name);
        villager_dialogue_two = GameObject.Find(villager_dialogue_two_name);

        text_bubble.SetActive(false);
        villager_dialogue_one.SetActive(false);
        villager_dialogue_two.SetActive(false);

        // Find interact UI button
        interactButton = GameObject.Find(interactButtonName);

        if (interactButton == null)
        {
            Debug.LogError("Interact button not found! Check name in inspector.");
        }
    }


    void Update()
    {
        CheckPlayerDistance();

        // PC input (phím I)
        if (Input.GetKeyDown(KeyCode.I) && PlayerInRange())
        {
            OnTalkButtonPressed();
        }
    }


    // Check distance
    void CheckPlayerDistance()
    {
        if (PlayerInRange())
        {
            ShowInteractButton();
        }
        else
        {
            HideInteractButton();
        }
    }

    bool PlayerInRange()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        return distance <= interactionRange;
    }


    // UI button active when close
    void ShowInteractButton()
    {
        interactButton.SetActive(true);
    }

    void HideInteractButton()
    {
        interactButton.SetActive(false);
    }


    // MOBILE + PC both call this
    public void OnTalkButtonPressed()
    {
        if (!PlayerInRange()) return;

        // Lock movement while talking
        movementScript.ToggleMovement(false);
        jumpScript.ToggleJumpMovement(false);

        // Start → Continue → End
        if (!text_bubble.activeSelf && !villager_dialogue_one.activeSelf && !villager_dialogue_two.activeSelf)
        {
            StartDialogue();
        }
        else if (text_bubble.activeSelf && villager_dialogue_one.activeSelf)
        {
            ContinueDialogue();
        }
        else if (text_bubble.activeSelf && villager_dialogue_two.activeSelf)
        {
            EndDialogue();

            // Unlock movement
            movementScript.ToggleMovement(true);
            jumpScript.ToggleJumpMovement(true);
        }
    }


    void StartDialogue()
    {
        text_bubble.SetActive(true);
        villager_dialogue_one.SetActive(true);
    }

    void ContinueDialogue()
    {
        villager_dialogue_one.SetActive(false);
        villager_dialogue_two.SetActive(true);
    }

    void EndDialogue()
    {
        villager_dialogue_two.SetActive(false);
        text_bubble.SetActive(false);
    }
}
