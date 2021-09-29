using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class loadScenes : MonoBehaviour
{
    public float delay = 5;
  
    void Start()
    {
        StartCoroutine(LoadLevelAfterDelay(delay));
    }

    IEnumerator LoadLevelAfterDelay(float delay)
    {
        //loads first the splash screen with the name of the game then the game description eah with a delay of 5 seconds 
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("splashScreen");
        SceneManager.LoadScene("gameDes");
       
        
    }

}