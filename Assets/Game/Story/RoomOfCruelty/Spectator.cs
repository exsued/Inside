using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectator : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Player.instance.transform);
    }
}
