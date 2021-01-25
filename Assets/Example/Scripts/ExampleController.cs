using CucuTools;

public class ExampleController : LerpableEntity
{
    public LerpableEntity lerp;

    protected override bool UpdateEntityInternal()
    {
        if (lerp == null) return false;
        
        lerp.Lerp(LerpValue);
        return true;
    }
}