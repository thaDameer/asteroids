using Zenject;

public class MediumMeteor : Meteor
{
    [Inject]
    public void Construct(MediumMeteorData meteorData)
    {
        Setup(meteorData);
    }
    public class Factory : PlaceholderFactory<MediumMeteor>
    {
        
    }
    
}