using UnityEngine;
using UnityEngine.Events;

public abstract class Trigger : MonoBehaviour
{
    public UnityEvent onTrigger;
    public bool once = true;

    public void InitTrigger()
    {
        onTrigger?.Invoke();
        if (once) Destroy(gameObject);
    }
}
