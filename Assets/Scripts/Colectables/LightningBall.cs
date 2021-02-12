public class LightningBall : Collectable
{
    protected override void ApplyEffect()
    {

foreach (var ball in BallsManager.instance.Balls.ToArray())
{
  ball.StartLightningBall();
}  
      
}
}