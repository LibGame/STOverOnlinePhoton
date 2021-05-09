using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    public static UIGame Instance;
    public Text PlayerRedScore;

    public void Awake()
    {
        Instance = GetComponent<UIGame>();
    }

    public void DisplayPlayerScore(int score)
    {
        PlayerRedScore.text = score.ToString();
    }
}
