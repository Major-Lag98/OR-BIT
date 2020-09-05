using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour //this class will help us reference things we neen
{

    public static GameAssets Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    //

    public EdgeCollider2D limitInScene;

}
