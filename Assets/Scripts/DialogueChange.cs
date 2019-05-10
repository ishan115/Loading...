using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChange : MonoBehaviour
{
    [SerializeField] private Button currentButton;


    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnCollisionEnter(Collider luke)
    {
        if (luke.gameObject.tag.Equals("luke") == true)
        {
            currentButton.gameObject.SetActive(true);
        }
    }
}
