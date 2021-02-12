using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{

#region Singleton



  private static CollectableManager _instance;
  public static CollectableManager instance => _instance;


  void Awake(){
      if(_instance !=null){
          Destroy(gameObject);
      } else {
          _instance = this;
      }
  }  
  #endregion


public List<Collectable> availableBuffs;
public List<Collectable> availableDeBuffs;

[Range(0,100)]
 public float buffChance;
 [Range(0,100)]
 public float deBuffChance;
}
