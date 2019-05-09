using UnityEngine;
using UnityEngine.UI;

public class DialogueAction : MonoBehaviour
{
    [SerializeField] private float charsPerSecond;
    [SerializeField] private string[] lines;

    [SerializeField] private bool hasChoice;
    [SerializeField] private string choice1;
    [SerializeField] private string choice2;
    [SerializeField] private int choice1Empathy;
    [SerializeField] private int choice2Empathy;

    [SerializeField] private BoxCollider2D linkedBlockade;

    private PlayerMovementController interactingPlayer;
    private string currentStringToPrint;
    private bool isCurrentlyPrinting = false;
    private bool inDialogue = false;
    private float printStartTime;
    private int printIndex;
    private bool inChoice = false;
    private bool isChoiceA = true;

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
                    DialogueCore.References.dialogueOutput.text = lines[printIndex];
                    isCurrentlyPrinting = false;
                }
                else
                {
                    // continue printing.
                    DialogueCore.References.dialogueOutput.text = lines[printIndex].Remove(currentPrintedCharacter);
                }
            }
            else
            {
                if (inChoice)
                {
                    if (Input.GetAxis("Vertical") < 0 && isChoiceA)
                    {
                        isChoiceA = false;
                        DialogueCore.References.SelectionArrow.SetBool("OnOptionA", false);
                    }
                    else if(Input.GetAxis("Vertical") > 0 && !isChoiceA)
                    {
                        isChoiceA = true;
                        DialogueCore.References.SelectionArrow.SetBool("OnOptionA", true);
                    }
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        if (isChoiceA)
                        {
                            DialogueCore.References.empathySlider.value += choice1Empathy;
                        }
                        else
                        {
                            DialogueCore.References.empathySlider.value += choice2Empathy;
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
                                DialogueCore.References.SelectionArrow.gameObject.GetComponent<RawImage>().enabled = true;
                                DialogueCore.References.choiceAOutput.text = choice1;
                                DialogueCore.References.choiceBOutput.text = choice2;
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
        DialogueCore.References.dialoguePanel.SetActive(false);
        inDialogue = false;
        interactingPlayer.InDialogue = false;

        if(linkedBlockade != null)
        {
            Destroy(linkedBlockade);
            DialogueCore.References.SetForwardErrorVisible(true);
        }

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
                    DialogueCore.References.choiceAOutput.text = "";
                    DialogueCore.References.choiceBOutput.text = "";
                    DialogueCore.References.dialogueOutput.text = "";
                    DialogueCore.References.SelectionArrow.gameObject.GetComponent<RawImage>().enabled = false;

                    DialogueCore.References.dialoguePanel.SetActive(true);
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