using Zenject;

public class SmallMeteor : Meteor
{
    [Inject]
    public void Construct(SmallMeteorData meteorData)
    {
        Setup(meteorData);
    }
    public class Factory : PlaceholderFactory<SmallMeteor>
    {
        
    }
    
}