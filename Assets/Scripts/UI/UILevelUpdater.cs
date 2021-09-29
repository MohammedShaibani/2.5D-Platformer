using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class UILevelUpdater : MonoBehaviour
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

    private void Update()
    {
        textbox.text = "Current Level: " + playerScript.curLevel;
    }

}
