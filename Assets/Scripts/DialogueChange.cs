using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChange : MonoBehaviour
{
    private Button button;
    private GameObject player;
    private Transform playerLocation;

    [SerializeField] private Button currentButton;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerLocation = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        ButtonChange();   
    }

    private void ButtonChange()
    {
        if(playerLocation)
        {
            playerLocation.position = new Vector3(32, 9, 0);

        }
    }
}
