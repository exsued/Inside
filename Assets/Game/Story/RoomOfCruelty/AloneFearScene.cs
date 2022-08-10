using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AloneFearScene : MonoBehaviour
{
    public Transform lightPl;

    public UnityEvent OnEnabled;
    public UnityEvent OnDisabled;

    [SerializeField] float lightSpeed = 10f;
    void OnEnable()
    {
        OnEnabled?.Invoke();
        StartCoroutine(AloneFearQuest());
    }
    void OnDisable()
    {
        OnDisabled?.Invoke();
        StartCoroutine(AloneFearQuest());
    }
    IEnumerator AloneFearQuest()
    {
        while(enabled)
        {
            lightPl.position = Vector3.Lerp(lightPl.position, Player.instance.transform.position, Time.deltaTime * lightSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}
