using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelFailed : MonoBehaviour
{
    public float delay = 5;

    void Start()
    {
        //calls to the player tag in the PlayerScript in order to access the health variable 
        GameObject thePlayer = GameObject.Find("Player");
        PlayerScript playerScript = thePlayer.GetComponent<PlayerScript>();

        //if the health variable is less than or equal to 0 the scene level failed will load 
        if (playerScript.health <= 0)
        {
            StartCoroutine(LoadLevelAfterDelay(delay));
        }
    }

    IEnumerator LoadLevelAfterDelay(float delay)
    {
        //loads the scene 
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("levelF");

    }
}
