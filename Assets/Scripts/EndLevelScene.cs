using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            //Increment the counter for levels cleared
            PlayerScript playerScript = other.GetComponent<PlayerScript>();

            //Write all the counters to the CSV file
            playerScript.WriteCountersToFile();

            //Load the level success screen
            SceneManager.LoadScene("levelS");
        }
    }
}
