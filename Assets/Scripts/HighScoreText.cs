using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HighScoreText : MonoBehaviour
{
    Text highScore;
    // Start is called before the first frame update
    void OnEnable()
    {
        highScore = GetComponent<Text>();
        highScore.text = "HighScore: " + PlayerPrefs.GetInt("HighScore").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
