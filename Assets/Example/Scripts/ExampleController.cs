using CucuTools;

public class ExampleController : LerpableEntity
{
    public LerpableTransform lerp;

    protected override bool UpdateEntityInternal()
    {
        if (lerp == null) return false;
        
        lerp.Lerp(LerpValue);
        transform.Set(lerp.Result);
        return true;
    }
}