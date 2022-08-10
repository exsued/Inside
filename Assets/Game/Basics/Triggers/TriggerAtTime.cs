using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerAtTime : Trigger
{
    public float time;
    protected new IEnumerator Start()
    {
        yield return new WaitForSeconds(time);
        InitTrigger();
    }
}
