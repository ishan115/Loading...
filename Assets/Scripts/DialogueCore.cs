using UnityEngine;
using UnityEngine.UI;

public class DialogueCore : MonoBehaviour
{
    public static DialogueCore References { get; private set; }

    [SerializeField] private RawImage moveForwardArrow;
    public GameObject dialoguePanel;
    public Text dialogueOutput;
    public Text choiceAOutput;
    public Text choiceBOutput;
    public Animator SelectionArrow;
    public Slider empathySlider;

    private void Awake()
    {
        References = this;
        SetForwardErrorVisible(false);
        dialoguePanel.SetActive(false);
    }

    public void SetForwardErrorVisible(bool isVisible)
    {
        moveForwardArrow.enabled = isVisible;
    }
}