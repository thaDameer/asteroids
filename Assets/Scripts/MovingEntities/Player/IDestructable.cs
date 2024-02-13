using UnityEngine;

public interface IDestructable
{
    [field: SerializeField]public SpriteRenderer SpriteRenderer { get; set; }
    [field: SerializeField]public abstract int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }

   
    public void TakeDamage(int dmg);

    public void Died() { }
}