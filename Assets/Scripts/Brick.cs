using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  static UnityEngine.ParticleSystem;
public class Brick : MonoBehaviour
{

private SpriteRenderer spriteRenderer;
    public int hitPoints ;
    public ParticleSystem destroyEffect;
    public static event Action<Brick> OnBrickDestruction;

    private void Awake(){
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
       
    }
private void OnCollisionEnter2D(Collision2D coll){
    Ball ball = coll.gameObject.GetComponent<Ball>();
    ApplyCollisionLogic(ball);
}

    private void ApplyCollisionLogic(Ball ball)
    {
       this.hitPoints --;
       if(this.hitPoints <= 0){
           OnBrickDestruction?.Invoke(this);
           SpawnDestroyEffect();
           Destroy(this.gameObject);
       } else {

             this.spriteRenderer.sprite = BricksManager.instance.sprites[this.hitPoints -1];
       }

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
}
