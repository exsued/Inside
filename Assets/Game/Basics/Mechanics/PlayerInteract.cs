using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteract : MonoBehaviour
{
    public UnityEvent<InteractTrigger> onInteractEnter;
    public UnityEvent onInteractExit;

    RaycastHit prevInteractHit;
    RaycastHit interactHit;

    public LayerMask interactableMask;

    InteractTrigger availableTrigg;
    public float interactDistance = 0.5f;

    public void Update()
    {
        RaycastUpdate();
    }
    void RaycastUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out interactHit, interactDistance, interactableMask))
        {
            if (interactHit.transform != prevInteractHit.transform)
            {
                availableTrigg = interactHit.transform.GetComponent<InteractTrigger>();
                if (availableTrigg != null)
                {
                    onInteractEnter.Invoke(availableTrigg);
                }
                prevInteractHit = interactHit;
            }
        }
        else
        {
            if (prevInteractHit.transform != null)
            {
                onInteractExit?.Invoke();
                prevInteractHit = new RaycastHit();
                availableTrigg = null;
            }
        }
    }
    public void PressInteractButton()
    {
        availableTrigg?.Interact();
        availableTrigg = null;
        onInteractExit?.Invoke();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward * interactDistance);
    }
}
