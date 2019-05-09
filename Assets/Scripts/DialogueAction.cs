using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueAction : MonoBehaviour
{
    [SerializeField] private Text dialogueOutput;
    [SerializeField] private Slider empathySlider;

    [SerializeField] private float charsPerSecond;
    [SerializeField] private string[] lines;

    [SerializeField] private bool hasChoice;
    [SerializeField] private string choice1;
    [SerializeField] private string choice2;
    [SerializeField] private int choice1Empathy;
    [SerializeField] private int choice2Empathy;

    private PlayerMovementController interactingPlayer;
    private string currentStringToPrint;
    private bool isCurrentlyPrinting = false;
    private bool inDialogue = false;
    private float printStartTime;
    private int printIndex;

    // Update is called once per frame
    private void Update()
    {
        if (inDialogue)
        {
            if (isCurrentlyPrinting)
            {
                int currentPrintedCharacter = Mathf.FloorToInt((Time.time - printStartTime) * charsPerSecond);
                if (currentPrintedCharacter >= lines[printIndex].Length - 1)
                {
                    // printing complete.
                    dialogueOutput.text = lines[printIndex];
                    isCurrentlyPrinting = false;
                }
                else
                {
                    // continue printing.
                    dialogueOutput.text = lines[printIndex].Remove(currentPrintedCharacter);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    printIndex++;
                    if (printIndex > lines.Length - 1)
                    {
                        //check for choice:
                        if (hasChoice)
                        {

                        }
                        else
                        {
                            interactingPlayer.PlayerCanMove = true;
                            Destroy(gameObject);
                        }
                    }
                    else
                    {
                        interactingPlayer.PlayerCanMove = false;
                        isCurrentlyPrinting = true;
                        printStartTime = Time.time;
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!inDialogue)
        {
            interactingPlayer = collision.gameObject.GetComponent<PlayerMovementController>();
            if (interactingPlayer != null)
            {

                if (Input.GetKeyDown(KeyCode.R))
                {
                    interactingPlayer.PlayerCanMove = false;
                    inDialogue = true;
                    isCurrentlyPrinting = true;
                    printIndex = 0;
                    printStartTime = Time.time;
                }
            }
        }
    }
}