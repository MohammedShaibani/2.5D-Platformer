using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class readCount : MonoBehaviour
{
    public Text textbox;

    private void Start()
     {
        textbox = GetComponent<Text>();

        string filePath = "Assets/Resources/counters.csv";
        StreamReader reader = new StreamReader(filePath);
        //Ignore the first line
        reader.ReadLine();
        int numCollectables = int.Parse(reader.ReadLine());

        textbox.text = "" + numCollectables;
    }
}

