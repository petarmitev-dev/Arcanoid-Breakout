public class MultiBall : Collectable
{
    protected override void ApplyEffect()
    {

foreach (Ball ball in BallsManager.instance.Balls.ToArray())
{
    BallsManager.instance.SpawnBall(ball.gameObject.transform.position,2,ball.isLightningBall);
}  
      
    }
}