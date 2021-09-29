using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class UICollectableUpdater : MonoBehaviour
{
    GameObject playerObject;
    PlayerScript playerScript;

    public Text textbox;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerObject.GetComponent<PlayerScript>();

        textbox = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        textbox.text = "Collectables: " + playerScript.GetCollectableCounter();
    }
}
