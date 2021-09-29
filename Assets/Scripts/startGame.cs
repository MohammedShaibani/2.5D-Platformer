using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class startGame : MonoBehaviour
{
    public void LoadGame()
    {
        //First, get the level # that was just completed 
        string filePath = "Assets/Resources/lastLevel.csv";

        StreamReader reader = new StreamReader(filePath);

        int levelNum = int.Parse(reader.ReadLine());

        reader.Close();
        //Increment, looping from 3 back to 1
        levelNum++;
        levelNum = levelNum % 3;

        //Write the levelNum to the file
        StreamWriter writer = new StreamWriter(filePath);
        writer.WriteLine("" + levelNum);

        writer.Close();

        //Now, load levelNum
        SceneManager.LoadScene("Level" + levelNum);
    }
}
