using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CollectableScript : MonoBehaviour
{
   
    
    void OnTriggerEnter(Collider other)
    {//checks to see if the gameobject with player tag collides with out gameobject coin 
        if (other.transform.tag == "Player") //if the player collides with coin then destroy coin 
        {
            //Play the pickup sound
            PlayerScript playerScript = other.GetComponent<PlayerScript>();
            playerScript.PlaySoundOneShot(playerScript.collectablePickupSound);

            //Increment the # of collectables collected
            playerScript.IncrementCollectablesCollected();

            //Destroy this collectable
            Destroy(gameObject);
        }
    }
}

