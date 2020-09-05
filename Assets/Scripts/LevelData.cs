using System.Collections;
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
    }

    [SerializeField] List<GameObject> _pieces;

    public Queue<GameObject> Q_Pieces;

    private void Start() //transfer objects from list into queue
    {
        for (int i =0; i <= _pieces.Count; i++)
        {
            Q_Pieces.Enqueue(_pieces[i]);
        }
    }
}
