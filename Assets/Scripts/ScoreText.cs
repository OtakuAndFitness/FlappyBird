using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreText : MonoBehaviour
{
    Text score;
    // Start is called before the first frame update
    void Onenable()
    {
        score = GetComponent<Text>();
        score.text = "Score: " + GameManager.Instance.Score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
