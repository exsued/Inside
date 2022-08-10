using UnityEngine;
using UnityEngine.Events;

public abstract class Trigger : MonoBehaviour
{
    protected Collider alignCollider;
    public UnityEvent onTrigger;
    public bool once = false;

    protected void Start()
    {
        alignCollider = GetComponent<Collider>();
    }
    protected void InitTrigger()
    {
        onTrigger?.Invoke();
        if (once)
        {
            Destroy(gameObject);
        }
    }
}
