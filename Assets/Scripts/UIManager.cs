using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
public Text scoreText;
public Text  livesText;

public Text  targetText;

public int score{get;set;}

private void Awake() {
    Brick.OnBrickDestruction += OnBrickDestruction;
    BricksManager.OnLevelLoaded += OnLevelLoaded;
     GameManager.OnLiveLost += OnLiveLost;
}
private void Start(){
    
     OnLiveLost(GameManager.instance.availableLives);
    }

    private void OnLiveLost(int remainingLives)
    {
        livesText.text = $"Lives:{Environment.NewLine}{remainingLives}";
    }

    private void OnLevelLoaded()
    {
      UpdatingRemainingBricksText();
      UpdateScoreText(0);
    }

    private void UpdateScoreText(int increment)
    {
        this.score += increment;
        string scoreString = this.score.ToString().PadLeft(5,'0');
        scoreText.text = $"Score:{Environment.NewLine}{scoreString}";
    }

    private void OnBrickDestruction(Brick obj)
    {
        UpdatingRemainingBricksText();
        UpdateScoreText(10);
    }

    private void UpdatingRemainingBricksText()
    {
     targetText.text = $"Target:{Environment.NewLine}{BricksManager.instance.RemainingBricks.Count} / {BricksManager.instance.InitialBrickCount}";
    }

    private void OnDisable(){
        Brick.OnBrickDestruction -= OnBrickDestruction;
        BricksManager.OnLevelLoaded -= OnLevelLoaded;
    }
}
