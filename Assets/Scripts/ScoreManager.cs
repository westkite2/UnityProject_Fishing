using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;

    private int score;

    // Update is called once per frame
    void Update()
    {
        //���� ����
        scoreText.text = score.ToString();
    }
    
    public void addScore(int amount)
    {
        //���� ����
        score += amount;
    }
}
