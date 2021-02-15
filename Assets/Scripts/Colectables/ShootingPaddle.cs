
public class ShootingPaddle : Collectable
{
    protected override void ApplyEffect()
    {
        PaddleScript.instance.StartShooting();
    }
}
