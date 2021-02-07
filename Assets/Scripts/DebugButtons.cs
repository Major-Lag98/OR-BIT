using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugButtons : MonoBehaviour 
{
    
    public void loadLevelOne()
    {
        SceneManager.LoadScene(1);
    }
}
