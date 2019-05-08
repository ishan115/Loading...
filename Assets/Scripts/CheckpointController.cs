using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    private bool isActivated;
    private PlayerMovementController playerController;
    /// <summary>
    /// Property Used to Activate or Deactivate checkpoints
    /// </summary>
    public bool IsActivated
    {
        get
        {
            return isActivated;
        }
        set
        {
            isActivated = value;
        }
    }

    private void Awake()
    {

    }

    private void Start()
    {
        IsActivated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerController = collision.GetComponent<PlayerMovementController>();
            playerController.CurrentCheckpoint = this;
        }
    }
}
