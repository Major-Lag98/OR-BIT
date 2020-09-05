using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{

    public static LevelData Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) //singleton for leveldata
        {
            Instance = this;
        }
    }

    public Queue<GameObject> Pieces;

}
