using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class additive : MonoBehaviour
{
    public void addScene()
    {
        SceneManager.LoadSceneAsync("InGameDisplay", LoadSceneMode.Additive);

    }
}
