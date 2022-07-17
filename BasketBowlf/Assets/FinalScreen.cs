using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "Final Score: " + ScoreStatic.totalScore[9].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
