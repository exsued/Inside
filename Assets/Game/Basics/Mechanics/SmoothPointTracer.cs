using UnityEngine;
using System.Collections;

public class SmoothPointTracer : MonoBehaviour
{
    //Camera poses
    public Transform Feet;

    public float CrouchHeightCoeff = 0.6f;  //Насколько камера опускается в процентах относительно высоты игрока

    public float lerpSpeed = 10f;

    float standHeight = 0f;
    float curHeight;

    private void Update()
    {
        if (Player.instance == null)
            return;
        standHeight = Player.instance.controllerHeight - 0.1f;  //Чтобы камера не была на самом кончике коллайдера уменьшаю на 0.1
        CheckPlayerState();
        transform.position =
            new Vector3(Feet.position.x,
            Mathf.Lerp(transform.position.y, Feet.position.y + curHeight, Time.deltaTime * lerpSpeed),
            Feet.position.z);
    }
    void CheckPlayerState()
    {
        switch(Player.instance.states)
        {
            case PlayerPosState.Crouch:
                curHeight = standHeight * CrouchHeightCoeff;
                break;
            case PlayerPosState.Run:
            case PlayerPosState.Walk:
                curHeight = standHeight;
                break;
        }
    }
}
