using UnityEngine;

[ExecuteInEditMode]
public class ControllerBase<TParameters> : MonoBehaviour
{
    [SerializeField] protected bool updateInEditor = false;
    [SerializeField] protected TParameters parameters;

    private void OnValidate()
    {
        if (updateInEditor)
        {
            Setup(parameters);
        }
    }

    public virtual void Setup(TParameters parameters)
    {
        this.parameters = parameters;
    }
}