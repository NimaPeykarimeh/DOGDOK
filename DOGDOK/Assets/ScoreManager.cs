using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    static public ScoreManager Instance;

    [SerializeField] float currentScore;
    [SerializeField] float scoreMultiplier = 10;
    public int killCounter = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
        currentScore += Time.fixedDeltaTime * scoreMultiplier;
    }
}
