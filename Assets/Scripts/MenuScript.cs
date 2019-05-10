using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    GameObject creditsPanel;

    private void Start()
    {
        Cursor.visible = false;
        creditsPanel = GameObject.Find("Credits Panel");
        creditsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        if (Input.GetButtonDown("Fire2"))
            creditsPanel.SetActive(!creditsPanel.activeSelf);
    }
}
