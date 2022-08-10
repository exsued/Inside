using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public bool alarm_actived
    {
        get
        {
            return _alarm_actived;
        }
        set
        {
            _alarm_actived = value;
        }
    }
    public bool pill_taken
    {
        get
        {
            return _pill_taken;
        }
        set
        {
            _pill_taken = value;
        }
    }
    bool _alarm_actived = false;
    bool _pill_taken = false;

    public Collider bedTrigger = null;
    public UnityEvent onSleepEvent;

    public void TryToSleep()
    {
        if(pill_taken && alarm_actived)
        {
            onSleepEvent?.Invoke();
            StartCoroutine(LoadSceneAtTime());
        }
        else
        {
            ShortMessage.instance.Sec3Toast("Прежде чем уснуть, нужно принять пилюлю и завести будильник");
        }
    }
    IEnumerator LoadSceneAtTime(float seconds = 4.5f)
    {
        yield return new WaitForSecondsRealtime(seconds);
        SceneManager.LoadScene("RoomOfCruelty");
    }
}
