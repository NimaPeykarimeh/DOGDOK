using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    static public ScoreManager Instance;

    [SerializeField] float currentScore;
    [SerializeField] float scoreMultiplier = 10;
    public int killCounter = 0;
    bool isGameEnded;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI KillsText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void AddScore(float _score)
    {
        currentScore += _score;
    }

    private void FixedUpdate()
    {
        if (!isGameEnded)
        {
            currentScore += Time.fixedDeltaTime * scoreMultiplier;
        }
    }

    public void ResetScore()
    {
        isGameEnded = false;
        currentScore = 0;
    }

    public void SetScoreValue()
    {
        isGameEnded = true;
        scoreText.text = "SCORE:" + currentScore.ToString("0");
        KillsText.text = "KILLS:" + killCounter.ToString();
    }
}
