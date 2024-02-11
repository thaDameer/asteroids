using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour,IBulletFactory
{
    //[SerializeField] SerializeField  
    public Projectile GetPooledBullet()
    {
        return null;
    }
}

public interface IBulletFactory 
{
    public Projectile GetPooledBullet();
}