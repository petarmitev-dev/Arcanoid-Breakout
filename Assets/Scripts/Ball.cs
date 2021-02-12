using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
public bool isLightningBall;
public float lightningBallDuration = 10;
private SpriteRenderer spriteRenderer;
public static event Action<Ball> OnBallDeath;

public static event Action<Ball> OnLightningBallEnable;

public static event Action<Ball> OnLightningBallDisable;
public ParticleSystem LightningBallEffect;

private void Awake(){
    this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
}
   

    public void Die()
    {
        OnBallDeath?.Invoke(this);
        Destroy(gameObject,1);
       
    }

     internal void StartLightningBall()
    {
        if(!this.isLightningBall){
            this.isLightningBall = true;
            this.spriteRenderer.enabled = false;
            LightningBallEffect.gameObject.SetActive(true);
            StartCoroutine(StopLightningEffectAfterTime(this.lightningBallDuration));
            OnLightningBallEnable.Invoke(this);
        }
    }

    private IEnumerator StopLightningEffectAfterTime(float seconds)
    {
       yield return new WaitForSeconds(seconds);
       StopLightningBall();
    }

    private void StopLightningBall()
    {
       if (this.isLightningBall){
           this.isLightningBall = false;
            this.spriteRenderer.enabled = true;
             LightningBallEffect.gameObject.SetActive(false);
             OnLightningBallDisable.Invoke(this);
       }
       {
           
       }
    }
}
