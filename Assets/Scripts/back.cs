using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class back : MonoBehaviour
{
    public float delay = 5;

    void Start()
    {
        StartCoroutine(LoadLevelAfterDelay(delay));
    }

    IEnumerator LoadLevelAfterDelay(float delay)
    {
        //loads the main menu scene after a delay of 5 seconds 
        //this scripts is applied to the gameDes scene 
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("mainMenu");

    }
   
}