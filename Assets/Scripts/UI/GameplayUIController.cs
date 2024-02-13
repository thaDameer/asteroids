using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField] private HealthCounterController healthCounterController;


    private void Start()
    {
        healthCounterController.Setup(new HealthCounterController.Parameters
        {
            LivesLeft = 3,
            TotalLives = 3,
            UpdateCounter = false
        });
    }
}
