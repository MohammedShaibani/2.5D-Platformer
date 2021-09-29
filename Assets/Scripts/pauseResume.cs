using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseResume : MonoBehaviour
{
    public bool paused = false;
    void Start()
    {
        paused = false;
        Time.timeScale = 1;
    }
    public void TogglePause()
    {
        if (paused == false)
        {
            Time.timeScale = 0;
            paused = true;
        }
        else if (paused == true)
        {
            Time.timeScale = 1;
            paused = false;
        }
    }
}
