using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsManager : MonoBehaviour
{
    

    
#region Singleton

  private static BallsManager _instance;
  public static BallsManager instance => _instance;

[SerializeField]  private Ball ballPrefab;
  private Ball initialBall;

  public float initialBallSpeed;
  private Rigidbody2D initialBallRb;

  void Awake(){
      if(_instance !=null){
          Destroy(gameObject);
      } else {
          _instance = this;
      }
  }  
  #endregion
 public List<Ball> Balls { get; set; }

 private void Start(){
     InitBalls();
 }

  private void Update(){
      if(!GameManager.instance.IsGameStarted)
      {

          Vector3 paddlePosition = PaddleScript.instance.gameObject.transform.position;
          Vector3 ballPosition = new Vector3(paddlePosition.x,paddlePosition.y + .27f,0);
          initialBall.transform.position = ballPosition;
      
      if(Input.GetMouseButtonDown(0) )
      {
          initialBallRb.isKinematic = false;
          initialBallRb.AddForce(new Vector2(0,initialBallSpeed));
          GameManager.instance.IsGameStarted = true;
       }
      }
  }

public void ResetBall()
    {
        foreach (var ball in this.Balls)
        {
            Destroy(ball.gameObject);
        }
        InitBalls();
    }

    private void InitBalls(){
     Vector3 paddlePosition = PaddleScript.instance.gameObject.transform.position;
     Vector3 startingPosition = new Vector3(paddlePosition.x,paddlePosition.y + .27f,0);
     initialBall = Instantiate(ballPrefab,startingPosition,Quaternion.identity);
     initialBallRb = initialBall.GetComponent<Rigidbody2D>();
     
     this.Balls = new List<Ball>{

         initialBall
     };
 }
}
