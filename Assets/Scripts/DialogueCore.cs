using UnityEngine;
using UnityEngine.UI;

public class DialogueCore : MonoBehaviour
{
    public static DialogueCore References { get; private set; }

    public GameObject dialoguePanel;
    public Text dialogueOutput;
    public Text choiceAOutput;
    public Text choiceBOutput;
    public Animator SelectionArrow;
    public Slider empathySlider;

    private void Awake(){ References = this; }
}