
public class ExtendAndShrink : Collectable
{

    public float NewWidth = 2.5f;
    protected override void ApplyEffect()
    {
        if(PaddleScript.instance != null &&  !PaddleScript.instance.PaddleTransforming)
        {
 
    PaddleScript.instance.StartWidthAmnimation(NewWidth);

        }
    }
}
