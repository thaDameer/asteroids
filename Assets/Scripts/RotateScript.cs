using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RotateScript : MonoBehaviour
{
    [SerializeField] private Vector3 rotationDirection;
    [SerializeField] private float rotationSpeed;

    private void Start()
    {
        var randomRotation = Random.rotation;
        randomRotation.x = rotationDirection.x == 0 ? 0 : randomRotation.x;
        randomRotation.y = rotationDirection.y == 0 ? 0 : randomRotation.y;
        randomRotation.z = rotationDirection.z == 0 ? 0 : randomRotation.z;
        transform.rotation = randomRotation;
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
        }
    }
}
