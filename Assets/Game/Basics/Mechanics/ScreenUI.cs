using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ScreenUI : MonoBehaviour
{
    public Image InteractSign = null;
    public Image Crosshair = null;
    public Image BlackOutImg = null;

    public Color interactEnterColor = Color.white;
    public Color interactDefaultColor = Color.gray;
    
    public static ScreenUI Instance { get; private set; }
    
    public void Start()
    {
        InteractSign.enabled = false;
        Crosshair.enabled = true;
        Crosshair.color = interactDefaultColor;
        Instance = this;
    }
    public void InteractEnter(InteractTrigger trigger)
    {
        InteractSign.sprite = trigger.Icon;
        InteractSign.enabled = true;
        Crosshair.color = interactEnterColor;
    }
    public void InteractExit()
    {
        InteractSign.enabled = false;
        Crosshair.color = interactDefaultColor;
    }
    public void BlackOut(float time)
    {
        StartCoroutine(changeTransparency(0f, 1f, time));
    }
    public void ReverseBlackOut(float time)
    {
        StartCoroutine(changeTransparency(1f, 0f, time));
    }
    IEnumerator changeTransparency(float startValue, float endValue, float time)
    {
        var speed = 1f / time;
        for(var i = 0f; i <= 1f; i += speed * Time.deltaTime)
        {
            BlackOutImg.color = new Color(0f, 0f, 0f, Mathf.Lerp(startValue, endValue, i));
            yield return new WaitForEndOfFrame();
        }
    }
}
