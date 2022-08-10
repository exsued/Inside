using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class sewageTreatmentPlantTerminal : MonoBehaviour
{
    public Canvas monitor;
    public Transform workPoint; //В какой позиции игрок работает
    public Transform playerLookPoint;
    public float camAngle;
    public Transform camPos;
    public GameObject loadingWindow;
    //private FMOD.Studio.EventInstance audioSource;
    bool activated = false;
    Vector3 startPos;
    public UnityEvent onExitTerminal;
    void Start()
    {
        monitor.gameObject.SetActive(false);
    }
    void PlayAudio(string soundPath)
    {
        StartCoroutine(LoadingWindow(3f));
    }
    public void Interact()
    {
        //if (Computer.instance && Computer.instance.actived)
        //    return;
        StartCoroutine(PlayerWorkWithTerminal());
    }
    IEnumerator LoadingWindow(float time)
    {
        loadingWindow.SetActive(true);
        yield return new WaitForSeconds(time);
        loadingWindow.SetActive(false);
    }
    public void OnDestroy()
    {
        ExitTerminalAndDestroy();
    }

    public void ExitTerminal(bool lastUse = false)
    {
        if (!activated)
            return;
        StartCoroutine(_exitTerminal(lastUse));
    }
    public void ExitTerminalAndDestroy()
    {
        if (!activated)
            return;
        var player = Player.instance;
        var playerCam = player.alignCamera;
        print(playerCam);
        playerCam.transform.position = startPos;
        playerCam.CursorActived = false;
        //playerCam.actived = true;
        monitor.gameObject.SetActive(false);
        Player.instance.enabled = true;
        playerCam.StopLookAt();
        playerCam.XLockRotate = playerCam.YLockRotate = false;
        //Player.instance.CanUseComputer = true;
        activated = false;
        onExitTerminal?.Invoke();
    }
    IEnumerator _exitTerminal(bool lastUse)
    {
        var player = Player.instance;
        var playerCam = player.alignCamera;
        yield return StartCoroutine(playerCam.TranslateAtPosition(startPos));
        playerCam.CursorActived = false;
        playerCam.actived = true;
        monitor.gameObject.SetActive(false);
        Player.instance.enabled = true;
        playerCam.StopLookAt();
        playerCam.XLockRotate = playerCam.YLockRotate = false;
        //Player.instance.CanUseComputer = true;
        activated = false;
        onExitTerminal?.Invoke();
        if (lastUse)
            enabled = false;
    }
    IEnumerator _enterTerminal()
    {
        activated = true;
        //Player.instance.CanUseComputer = false;
        var player = Player.instance;
        var playerCam = player.alignCamera;

        Player.instance.enabled = false;

        playerCam.XLockRotate = playerCam.YLockRotate = true;
        playerCam.StartLookAt(workPoint.rotation, camAngle);
        startPos = playerCam.transform.position;
        yield return StartCoroutine(playerCam.TranslateAtPosition(workPoint));
        monitor.gameObject.SetActive(true);
        playerCam.CursorActived = true;
    }
    IEnumerator PlayerWorkWithTerminal()
    {
        yield return StartCoroutine(_enterTerminal());
        yield return StartCoroutine(LoadingWindow(0.5f));
        while (Input.GetKey(KeyCode.F))
        {
            yield return null;
        }
        while (!Input.GetKey(KeyCode.F))
        {
            yield return null;
        }
        ExitTerminal();
    }
}
