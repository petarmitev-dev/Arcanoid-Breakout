using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BricksManager : MonoBehaviour
{
#region Singleton



  private static BricksManager _instance;
  public static BricksManager instance => _instance;
 public  static event Action OnLevelLoaded ;

  void Awake(){
      if(_instance !=null){
          Destroy(gameObject);
      } else {
          _instance = this;
      }
  }  
  #endregion



private GameObject brickContainer;
private int maxRows = 17;
private int maxCols = 12;
private float initialBricksSpawnPositionX = - 1.96f;
  private float initialBricksSpawnPositionY = 3.325f;
  private float shiftAmount = 0.365f;
  public Sprite[] sprites;
  public Color[] brickColors;
  public int CurrentLevel;
  public Brick brickPrefab;
  public List<int[,]> LevelsData{get;set;}
  public List<Brick> RemainingBricks{get;set;}
    public int InitialBrickCount { get; set; }
    public int CurrentLevelentLevel { get; internal set; }
   

    public void LoadLevel(int level )
    {
 this.CurrentLevel = level;
this.ClearRemainingBricks();
this.GeneratedBricks();
    }

    public void LoadNextLevel()
    {
        this.CurrentLevel++;
        if(this.CurrentLevel >= this.LevelsData.Count){
            GameManager.instance.ShowVictoryScreen();
        } 
        else {
            this.LoadLevel(this.CurrentLevel);
        }
    }

    private void Start(){
      this.LevelsData = this.LoadLevelsData();
      this.brickContainer = new GameObject("BrickContainer");
      this.GeneratedBricks();
      
  }

    private void GeneratedBricks()
    {
    this.RemainingBricks = new List<Brick>();
    int[,] currentLevelData = this.LevelsData[this.CurrentLevel];
     float  currentSpawnX = initialBricksSpawnPositionX;
      float  currentSpawnY = initialBricksSpawnPositionY;
      float zShift = 0;
      for( int row = 0; row < this.maxRows; row++){
          for(int col = 0; col < this.maxCols; col++){
              int brickType = currentLevelData[row,col];

              if(brickType > 0){    
            Brick newBrick = Instantiate(brickPrefab,new Vector3(currentSpawnX,currentSpawnY,0.00f - zShift),Quaternion.identity);
              newBrick.Init(brickContainer.transform,this.sprites[brickType -1],this.brickColors[brickType],brickType);
              this.RemainingBricks.Add(newBrick);
              zShift = 0.0001f;
              }

              currentSpawnX += shiftAmount;
              if(col + 1== this.maxCols){
                  currentSpawnX = initialBricksSpawnPositionX;
              }
          }
          currentSpawnY -= shiftAmount;
      }
      this.InitialBrickCount = this.RemainingBricks.Count;
      OnLevelLoaded?.Invoke();
    }


    private void ClearRemainingBricks()
    {
        foreach ( Brick brick  in this.RemainingBricks.ToArray())
        {
           Destroy(brick.gameObject); 
        }
    }

    private List<int[,]> LoadLevelsData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;
        string[] rows = text.text.Split(new string[]{Environment.NewLine},StringSplitOptions.RemoveEmptyEntries);
        List<int[,]> lData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows,maxCols];
        int currentRow = 0;
        for(int row = 0;row < rows.Length; row++){
            string line = rows[row];
            if(line.IndexOf("--") == -1)
            {

string[] bricks = line.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries);
for(int col = 0; col < bricks.Length; col++){
        currentLevel[currentRow,col] = int.Parse(bricks[col]);
    }
         currentRow++;
            } 
            else 
            {

           currentRow = 0;
           lData.Add(currentLevel);
           currentLevel = new int[maxRows,maxCols];
            }
        }
        return lData;

    }
}
