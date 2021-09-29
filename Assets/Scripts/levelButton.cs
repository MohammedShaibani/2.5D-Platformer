using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelButton : MonoBehaviour
{
    public void LoadLevelOptions()
    {
        //loads the level scene 
        //i still have to implement the different levels here 
        SceneManager.LoadScene("goToLevel");
    }
}
