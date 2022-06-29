using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnterTrigger : Trigger
{
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            InitTrigger();
        }
    }
}
