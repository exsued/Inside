using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : Trigger
{
    public string interactName;
    public Sprite Icon;
    
    public virtual void Interact()
    {
        InitTrigger();
    }
}
