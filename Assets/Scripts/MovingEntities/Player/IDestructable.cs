using UnityEngine;

public interface IDestructable
{
    public SpriteRenderer SpriteRenderer { get; set; }
    public abstract int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }

   
    public void TakeDamage(int dmg);

    public void Died() { }
}