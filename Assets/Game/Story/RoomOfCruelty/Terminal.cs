using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Terminal : InteractTrigger
{
    public UnityEvent onTriggerExit;
    public float time;
    public Transform camPos;

    public bool interacting = false;
    public override void Interact()
    {
        if (alignCollider.enabled)
            ForceInteract();
    }
    public void ForceInteract()
    {
        StartCoroutine(SmoothInteract());
        alignCollider.enabled = false;
    }
    public void PuzzleSolved()
    {
        Destroy(alignCollider);
        Destroy(this);
    }
    public IEnumerator SmoothInteract()
    {
        interacting = !interacting;
        PlayerSetInteract(interacting);
        yield return new WaitForSeconds(time);
        alignCollider.enabled = true;
    }
    public void PlayerSetInteract(bool actived)
    {
        ScreenUI.Instance.gameObject.SetActive(!interacting);
        var playerCam = Player.instance.alignCamera;
        if (actived)
        {
            playerCam.transform.position = camPos.position;
            playerCam.transform.rotation = camPos.rotation;
            onTrigger?.Invoke();
        }
        else
        {
            playerCam.transform.localPosition = Vector3.zero;
            onTriggerExit?.Invoke();
        }
        playerCam.enabled = !actived;
        playerCam.XLockRotate = playerCam.YLockRotate = actived;
        playerCam.CursorActived = actived;
        
        Player.instance.enabled = !actived;
        Player.instance.tracer.enabled = !actived;
    }
}
