using UnityEngine;
using Random = UnityEngine.Random;

public class Meteor : MovementEntity,IDestructable
{
    private int MeteorHealth = 3;
    public override Vector3 MovementDirection { get; set; }

    private Vector2 rotateDir;
    [field: SerializeField]public SpriteRenderer SpriteRenderer { get; set; }
    [field: SerializeField]public int MaxHealth { get; set; }


    public int CurrentHealth { get; set; }

    private void Start()
    {
        SetupMeteor();
        CurrentHealth = MaxHealth;
    }

    

    private void SetupMeteor()
    {
        SetRandomDirection();
        SetRandomRotationDirection();
    }
    private void SetRandomDirection()
    {

        MovementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
    private void SetRandomRotationDirection()
    {
        rotateDir.x= Mathf.Clamp01(Random.Range(-1f, 1f));
    }

    private void Update()
    {
        UpdateRotation(rotateDir);
        UpdateMovement(true);
    }

    public void Died()
    {
        Destroy(gameObject);
        GameEvents.OnMeteoerDestroyed?.Invoke(this);
    }


    public void TakeDamage(int dmg)
    {
        CurrentHealth -= dmg;
        SpriteHelperClass.Flash(SpriteRenderer,0.05f,2);
        if (CurrentHealth <= 0)
        {
            Died();
            return;
        }
    }
}
