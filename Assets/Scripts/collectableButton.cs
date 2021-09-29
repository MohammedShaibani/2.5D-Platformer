using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class collectableButton : MonoBehaviour
{
    public void LoadCollect()
    {
        //loads the collect scene when button is clicked. This scene shows the number of collectables that the player has collected in their lifetime
        SceneManager.LoadScene("collect");
    }

}