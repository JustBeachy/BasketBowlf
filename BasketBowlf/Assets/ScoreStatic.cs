using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreStatic : MonoBehaviour
{
    public Text[] frameText;
    public Text[] totalText;

    public static int[] frameScore = new int[10];
    public static int[] totalScore = new int[10];
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i<frameScore.Length;i++)
        {
            frameText[i].text = frameScore[i].ToString();
            totalText[i].text = totalScore[i].ToString();

            if (frameText[i].text == "0")
                frameText[i].text = "";

            if (totalText[i].text == "0")
                totalText[i].text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
