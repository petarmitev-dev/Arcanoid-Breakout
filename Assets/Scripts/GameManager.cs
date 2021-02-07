using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
public int Lives{get;set;}
#region Singleton

  private static GameManager _instance;
  public static GameManager instance => _instance;
  public static event Action<int> OnLiveLost;

  void Awake(){
      if(_instance !=null){
          Destroy(gameObject);
      } else {
          _instance = this;
      }
  }  
  #endregion

public GameObject gameOverScreen;
public GameObject victoryScreen;
public int availableLives = 3;
public bool IsGameStarted{get;set;}

public void RestartGame(){
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
private void Start(){
    Screen.SetResolution(540,910,false);
    this.Lives = this.availableLives;
    Ball.OnBallDeath += OnBallDeath;
    Brick.OnBrickDestruction += OnBrickDestruction;

}
private void OnBrickDestruction(Brick obj){
    if(BricksManager.instance.RemainingBricks.Count <=0){
        BallsManager.instance.ResetBall();
        GameManager.instance.IsGameStarted = false;

        BricksManager.instance.LoadNextLevel();
    }
}
private void OnBallDeath(Ball obj){
    if(BallsManager.instance.Balls.Count <= 0){
        this.Lives --;
        if(this.Lives < 1){
gameOverScreen.SetActive(true);
        } else {
            OnLiveLost.Invoke(this.Lives);
            BallsManager.instance.ResetBall();
            IsGameStarted = false;
         BricksManager.instance.LoadLevel(BricksManager.instance.CurrentLevel);
        }
    }
}

    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }

    private void OnDisable(){
    Ball.OnBallDeath -= OnBallDeath;
}




}
