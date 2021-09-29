using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitGame : MonoBehaviour
{
  public void LoadMainMenu()
    { //when exit button is clicked it returns to the main menu scene
        SceneManager.LoadScene("mainMenu");
    }
}
