using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShortMessage : MonoBehaviour
{
    Text textmesh;
    public static ShortMessage instance;
    bool isEnabled;
    private void Start()
    {
        textmesh = GetComponent<Text>();
        instance = this;
        //textmesh.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }
    public void Sec3Toast(string text)
    {
        if (isEnabled)
        {
            StopAllCoroutines();
        }
        StartCoroutine(_shortToast(text, 3f));
    }
    public void OnDisable()
    {
        textmesh.text = "";
        isEnabled = false;
    }
    public void ShortToast(string text, float time)
    {
        if(isEnabled)
        {
            StopAllCoroutines();
        }
        StartCoroutine(_shortToast(text, time));
    }
    IEnumerator _shortToast(string text, float time)
    {
        isEnabled = true;
        textmesh.text = text;
        yield return new WaitForSeconds(time);
        textmesh.text = "";
        isEnabled = false;
    }
}
