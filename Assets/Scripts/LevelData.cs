﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{

    public static LevelData Instance;

    void Awake()
    {
        if (Instance == null) //singleton for leveldata
        {
            Instance = this;
        }

        for (int i = 0; i < _pieces.Count; i++)
        {
            PiecesQueue.Enqueue(_pieces[i]);
        }
    }

    [SerializeField] List<GameObject> _pieces;

    public Queue<GameObject> PiecesQueue = new Queue<GameObject>();


    private void Start() //transfer objects from list into queue
    {
        
    }
}
