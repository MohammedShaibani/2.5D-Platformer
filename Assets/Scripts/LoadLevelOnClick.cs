using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LoadLevelOnClick : MonoBehaviour
{
    string filePath = "Assets/Resources/lastLevel.csv";

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");

        StreamWriter writer = new StreamWriter(filePath);
        writer.WriteLine("1");
        writer.Close();

    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");

        StreamWriter writer = new StreamWriter(filePath);
        writer.WriteLine("2");
        writer.Close();
    }
    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level3");

        StreamWriter writer = new StreamWriter(filePath);
        writer.WriteLine("3");
        writer.Close();
    }

}
