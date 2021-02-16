using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour
{

    
#region Singleton

  private static PaddleScript _instance;
  public static PaddleScript instance => _instance;

    public bool PaddleTransforming { get; set; }
    

  
    void Awake(){
      if(_instance !=null){
          Destroy(gameObject);
      } else {
          _instance = this;
      }
  }  
  #endregion

  //Shooting Variables
public bool PaddleShooting{get;set;}
public GameObject leftMuzzle;
public GameObject rightMuzzle;
public Projectile bulletPrefab;


    private Camera mainCamera;
    private float paddleInitialy;
    private float defaultPaddleWithInPixels = 200;
    private  float defaultLeftClamp =  115;
      private  float defaultRightClamp = 410;
    private SpriteRenderer spriteRenderer;
    public  float extendShrinkDuration = 10;
    public float paddleWidth = 2;
    public float paddleHeight = 0.28f;

    private BoxCollider2D boxCollider;
    // Start is called before the first frame update

   private void Start()
    {
      mainCamera = FindObjectOfType<Camera>();   
      paddleInitialy = this.transform.position.y;  
      spriteRenderer = GetComponent<SpriteRenderer>();
      boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        PaddleMovement();
        UpdateMuzzlePosition();
    }

    private void UpdateMuzzlePosition()
    {
      leftMuzzle.transform.position = new Vector3(this.transform.position.x - (this.spriteRenderer.size.x/2) + 0.1f,this.transform.position.y  + 0.2f, this.transform.position.z);
       rightMuzzle.transform.position = new Vector3(this.transform.position.x + (this.spriteRenderer.size.x/2) - 0.153f,this.transform.position.y  + 0.2f, this.transform.position.z);
    }

    public void StartWidthAmnimation(float newWidth)
    {

   StartCoroutine(AnimatePaddleWidth(newWidth));     

    }

    public IEnumerator AnimatePaddleWidth(float width)
    {
        this.PaddleTransforming = true;
        this.StartCoroutine(ResetPaddleWidthAfterTime(this.extendShrinkDuration));

        if (width > this.spriteRenderer.size.x)
        {
            float currentWidth = this.spriteRenderer.size.x;
            while (currentWidth < width)
            {
                currentWidth += Time.deltaTime * 2;
                this.spriteRenderer.size = new Vector2(currentWidth, paddleHeight);
                boxCollider.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }
        else
        {
            float currentWidth = this.spriteRenderer.size.x;
            while (currentWidth > width)
            {
                currentWidth -= Time.deltaTime * 2;
                this.spriteRenderer.size = new Vector2(currentWidth, paddleHeight);
                boxCollider.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }

        this.PaddleTransforming = false;
    }

    private IEnumerator ResetPaddleWidthAfterTime(float seconds)
    {
       yield return new WaitForSeconds(seconds);
       // Debug.Log($"Shrinking Time{Time.time}");
       this.StartWidthAmnimation(this.paddleWidth);
    }

    private void PaddleMovement(){
    

        float paddleShift = (defaultPaddleWithInPixels - (( defaultPaddleWithInPixels / 2) * this.spriteRenderer.size.x)) / 2;
        float  leftClamp = defaultLeftClamp - paddleShift;
        float rightClamp = defaultRightClamp + paddleShift;
        float mousePositionPixels = Mathf.Clamp(Input.mousePosition.x,leftClamp,rightClamp);
      float  mousePositionWorldX = mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels,0,0)).x;
        this.transform.position= new Vector3(mousePositionWorldX,paddleInitialy,0);
    
    }
    



    private  void OnCollisionEnter2D(Collision2D coll){
        if(coll.gameObject.tag == "Ball"){
        Rigidbody2D ballRB = coll.gameObject.GetComponent<Rigidbody2D>();
        Vector3 hitPoint = coll.contacts[0].point;
        Vector3 paddleCenter =  new Vector3(this.gameObject.transform.position.x,this.gameObject.transform.position.y);
        
       ballRB.velocity = Vector2.zero;
        float difference = paddleCenter.x - hitPoint.x;
           
        if(hitPoint.x < paddleCenter.x){
            ballRB.AddForce(new Vector2(-(Mathf.Abs(difference * 200)),BallsManager.instance.initialBallSpeed));
        } else {
               ballRB.AddForce(new Vector2((Mathf.Abs(difference * 200)),BallsManager.instance.initialBallSpeed));
        }

    }
    }

    public void StartShooting()
    {
      if(!this.PaddleTransforming){
        PaddleShooting = true;
        StartCoroutine(StartShootingRoutine());
      }
    }

    public IEnumerator StartShootingRoutine()
    {
        float fireCoolDown = .5f;
        float fireCoolDownLeft = 0;
        float shootingDuration = 10;
        float shootingDurationLeft = shootingDuration;

        while (shootingDurationLeft >= 0)
        {
            fireCoolDownLeft -= Time.deltaTime;
            shootingDurationLeft -= Time.deltaTime;
            if (fireCoolDownLeft <= 0)
            {
              this.Shoot();
              fireCoolDownLeft = fireCoolDown;
            
                
            }
            yield return null;
            
        }

        this.PaddleShooting = false;
        leftMuzzle.SetActive(false);
       rightMuzzle.SetActive(false);
    }

    private void Shoot()
    {
       leftMuzzle.SetActive(false);
       rightMuzzle.SetActive(false);
       leftMuzzle.SetActive(true);
       rightMuzzle.SetActive(true);
       this.SpawnBullet(leftMuzzle);
       this.SpawnBullet(rightMuzzle);
    }

        private void SpawnBullet(GameObject muzzle)
    {
        Vector3 spawnPosition = new Vector3(muzzle.transform.position.x, muzzle.transform.position.y + 0.2f, muzzle.transform.position.z);
        Projectile bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(new Vector2(0, 450f));
    }
}
