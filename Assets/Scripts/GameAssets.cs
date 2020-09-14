using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    public TextMeshProUGUI UIAmountLeft;

    public Image NextPieceImage;

    public GameObject UI;
}
