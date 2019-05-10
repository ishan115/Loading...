using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("This is the dialogue panel that will appear when the collider is triggered.")]
    private GameObject dialogue;

    PlayerMovementController playerController;

    // Start is called before the first frame update
    void Start()
    {
        dialogue.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && dialogue.activeSelf == true)
        {
            dialogue.SetActive(false);
            playerController.PlayerCanMove = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerController = collision.GetComponent<PlayerMovementController>();
            playerController.PlayerCanMove = false;
            dialogue.SetActive(true);
        }
    }
}
