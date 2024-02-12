using UnityEngine;

public abstract class ManagerBase : MonoBehaviour
{
    protected bool initialized;
    public virtual void Initialize()
    {
        initialized = true;
    }
}