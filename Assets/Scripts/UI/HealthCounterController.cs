using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class HealthCounterController : ControllerBase<HealthCounterController.Parameters>
{
    [SerializeField] private HealthIcon iconBlueprint;
    private List<HealthIcon> icons = new List<HealthIcon>();
    
    private IShipLifeCounter _iShipLifeCounter;
    [Inject]
    public void Construct(IShipLifeCounter shipLifeCounter)
    {
        _iShipLifeCounter = shipLifeCounter;
        _iShipLifeCounter.OnLifeCountChanged += LifeCountChanged;
    }
    
    [Serializable]
    public class Parameters
    {
        [FormerlySerializedAs("TotalLives")] public int MaxShips;
        [FormerlySerializedAs("LivesLeft")] public int ShipsLeft;
        public bool UpdateCounter;
    }
    
    private void OnDestroy()
    {
        if(_iShipLifeCounter!=null)
            _iShipLifeCounter.OnLifeCountChanged -= LifeCountChanged;
    }

    private void LifeCountChanged(int count)
    {
        
        Setup(new Parameters
        {
            ShipsLeft = _iShipLifeCounter.ShipsLeft,
            MaxShips = _iShipLifeCounter.MaxShips,
            UpdateCounter =  count <= _iShipLifeCounter.MaxShips
        });
    }
    public override void Setup(Parameters parameters)
    {
        base.Setup(parameters);
        if(icons.Count == 0)
            SetupIcons();
        for (int i = 0; i <icons.Count; i++)
        {
            int count = i + 1;
            bool active = parameters.ShipsLeft > i ;
            var icon = icons[i];
            icon.SetIcon(active, false);
        }
            
    }

    private void SetupIcons()
    {
        for (int i = 0; i < parameters.MaxShips; i++)
        {
            var icon = Instantiate(iconBlueprint, transform);
            icon.SetIcon(true, false);
            icons.Add(icon);
        }
        iconBlueprint.gameObject.SetActive(false);
    }

    public int LifeCount { get; set; }
}