using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllerService 
{
    public bool Accelerate { get; }
    public bool Shoot { get; }
    public Vector2 InputAxis { get; }
}

public class KeyboardControls : IControllerService
{
    public bool Accelerate => InputAxis.y > 0;
    public bool Shoot => Input.GetKeyDown(KeyCode.Space);
    public Vector2 InputAxis => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); 
}


 
