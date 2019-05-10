using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is for a trigger volume that will end the game and ring the player to the main menu 
/// </summary>
public class EndGameColliderScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Return To Title Scene
        SceneManager.LoadScene(0);
    }
}
