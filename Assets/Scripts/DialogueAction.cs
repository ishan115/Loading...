using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueAction : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogueOutput;
    [SerializeField] private Text choiceAOutput;
    [SerializeField] private Text choiceBOutput;
    [SerializeField] private Animator SelectionArrow;
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
    private bool inChoice = false;
    private bool isChoiceA = true;

    private void Awake()
    {
        dialoguePanel.SetActive(false);
    }

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
                if (inChoice)
                {
                    if (Input.GetAxis("Vertical") < 0 && isChoiceA)
                    {
                        isChoiceA = false;
                        SelectionArrow.SetBool("OnOptionA", false);
                    }
                    else if(Input.GetAxis("Vertical") > 0 && !isChoiceA)
                    {
                        isChoiceA = true;
                        SelectionArrow.SetBool("OnOptionA", true);
                    }
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        if (isChoiceA)
                        {
                            empathySlider.value += choice1Empathy;
                        }
                        else
                        {
                            empathySlider.value += choice2Empathy;
                        }
                        EndDialogue();
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        printIndex++;
                        if (printIndex > lines.Length - 1)
                        {
                            // check for choice:
                            if (hasChoice)
                            {
                                SelectionArrow.gameObject.GetComponent<RawImage>().enabled = true;
                                choiceAOutput.text = choice1;
                                choiceBOutput.text = choice2;
                                inChoice = true;
                            }
                            else
                            {
                                EndDialogue();
                            }
                        }
                        else
                        {
                            isCurrentlyPrinting = true;
                            printStartTime = Time.time;
                        }
                    }
                }
            }
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        inDialogue = false;
        interactingPlayer.InDialogue = false;
        Destroy(gameObject);
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
                    choiceAOutput.text = "";
                    choiceBOutput.text = "";
                    dialogueOutput.text = "";
                    SelectionArrow.gameObject.GetComponent<RawImage>().enabled = false;

                    dialoguePanel.SetActive(true);
                    interactingPlayer.InDialogue = true;
                    inDialogue = true;
                    isCurrentlyPrinting = true;
                    printIndex = 0;
                    printStartTime = Time.time;
                }
            }
        }
    }
}