using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCounterController : ControllerBase<HealthCounterController.Parameters>
{
    [SerializeField] private HealthIcon iconBlueprint;
    private List<HealthIcon> icons = new List<HealthIcon>();
    
    [Serializable]
    public class Parameters
    {
        public int TotalLives;
        public int LivesLeft;
        public bool UpdateCounter;
    }

    public override void Setup(Parameters parameters)
    {
        base.Setup(parameters);
        if(icons.Count == 0)
            SetupIcons();
        for (int i = 0; i <icons.Count; i++)
        {
            int count = i + 1;
            bool active = parameters.LivesLeft > i ;
            var icon = icons[i];
            icon.SetIcon(active,false);
        }
            
    }

    private void SetupIcons()
    {
        for (int i = 0; i < parameters.TotalLives; i++)
        {
            var icon = Instantiate(iconBlueprint, transform);
            icon.SetIcon(true, false);
            icons.Add(icon);
        }
        iconBlueprint.gameObject.SetActive(false);
    }

}