using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  static UnityEngine.ParticleSystem;
public class Brick : MonoBehaviour
{

private SpriteRenderer spriteRenderer;
private BoxCollider2D boxCollider;
    public int hitPoints ;
    public ParticleSystem destroyEffect;
    public static event Action<Brick> OnBrickDestruction;

    private void Awake(){
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.boxCollider = this.GetComponent<BoxCollider2D>();
        Ball.OnLightningBallEnable += OnLightningBallEnable;
        Ball.OnLightningBallDisable += OnLightningBallDisable;
       
    }

   

    private void OnLightningBallEnable(Ball obj)
    {
       if(this != null){
           this.boxCollider.isTrigger = true;
       }
    }

     private void OnLightningBallDisable(Ball obj)
    {
        
       if(this != null){
           this.boxCollider.isTrigger = false;
       }
    }

    private void OnCollisionEnter2D(Collision2D coll){

     bool instantKill = false;
     if(coll.collider.tag == "Ball"){
          Ball ball = coll.gameObject.GetComponent<Ball>();
          instantKill = ball.isLightningBall;
     }

     if(coll.collider.tag == "Ball"|| coll.collider.tag == "Projectile"){

 ApplyCollisionLogic(instantKill);
     }
}

 private void OnTriggerEnter2D(Collider2D coll){
     bool instantKill = false;
     if(coll.tag == "Ball"){
          Ball ball = coll.gameObject.GetComponent<Ball>();
          instantKill = ball.isLightningBall;
     }

     if(coll.tag == "Ball"|| coll.tag == "Projectile"){

 ApplyCollisionLogic(instantKill);
     }
       
 }

    private void ApplyCollisionLogic(bool instantKill)
    {
       this.hitPoints --;
       if(this.hitPoints <= 0 || instantKill){
           BricksManager.instance.RemainingBricks.Remove(this);
           OnBrickDestruction?.Invoke(this);
           OnBricksDestroy();
           SpawnDestroyEffect();
           Destroy(this.gameObject);
       } else {

             this.spriteRenderer.sprite = BricksManager.instance.sprites[this.hitPoints -1];
       }

    }

    private void OnBricksDestroy()
    {
        float buffSpawnChance = UnityEngine.Random.Range(0,100f);
        
        float deBuffSpawnChance = UnityEngine.Random.Range(0,100f);

        bool alreadySpawned = false;
        if(buffSpawnChance <= CollectableManager.instance.buffChance){
            alreadySpawned = true;
            Collectable newBuff = this.SpawnCollectable(true);
        }
        if(deBuffSpawnChance <= CollectableManager.instance.deBuffChance && !alreadySpawned){
            Collectable newBuff = this.SpawnCollectable(false);
        }
    }

    private Collectable SpawnCollectable(bool isBuff)
    {
        List<Collectable> collection;
        if(isBuff)
        {
            collection = CollectableManager.instance.availableBuffs;
        }
        else
        {
            collection =CollectableManager.instance.availableDeBuffs;
        }

        int buffIndex = UnityEngine.Random.Range(0,collection.Count);
        Collectable prefab = collection[buffIndex];
        Collectable newCollectable = Instantiate(prefab,this.transform.position,Quaternion.identity) as Collectable;

        return newCollectable;
    }

    private void SpawnDestroyEffect()
    {
      Vector3 brickPos = gameObject.transform.position;
      Vector3 spawnPosition = new Vector3(brickPos.x,brickPos.y,brickPos.z - 0.2f);
      GameObject effect = Instantiate(destroyEffect.gameObject,spawnPosition,Quaternion.identity);
      MainModule mainModule = effect.GetComponent<ParticleSystem>().main;
      mainModule.startColor = this.spriteRenderer.color;
      Destroy(effect,destroyEffect.main.startLifetime.constant);
    }

    internal void Init(Transform containerTransform, Sprite sprite, Color color, int hitpoints)
    {
       transform.SetParent(containerTransform);
       this.spriteRenderer.sprite = sprite;
       this.spriteRenderer.color = color;
       this.hitPoints = hitpoints;

    }

   private void OnDisable(){
       Ball.OnLightningBallEnable -= OnLightningBallEnable;
       Ball.OnLightningBallDisable -= OnLightningBallDisable;
   }
}
