using UnityEngine;
using Zenject;

public class LargeMeteor : Meteor
{
    [Inject]
    public void Construct(LargeMeteorData large)
    {
        Setup(large);

    }
    public class Factory : PlaceholderFactory<LargeMeteor>
    {
        
    }
}