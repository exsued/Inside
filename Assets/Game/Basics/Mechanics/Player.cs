using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerPosState
{
    Crouch,
    Walk,
    Run
}

[System.Serializable]
public class StepType
{
    public string groundTag;
    public AudioClip[] steps;
}

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public static Player instance = null;

    public float walkSpeed = 5f,
             runSpeed = 8f,
             crouchSpeed = 3f;
    public LayerMask groundMask;
    public LayerMask ceilingMask;
    public CharacterController controller { get; private set; }
    public Vector3 moveDirection { get; private set; }
    public PlayerPosState states { get; private set; } = PlayerPosState.Walk;

    public SmoothPointTracer tracer { get; private set; }

    private float standHeight, crouchHeight, stepLength;
    private float timer;

    private Vector2 inVector;
    private bool crouch;
    private bool run;

    private AudioSource foostepsSource;

    public float runStepVolume = 1.0f;
    public float walkStepVolume = 0.3f;
    public float crouchStepVolume = 0.15f;
    
    public StepType[] runFoosteps;
    public StepType[] walkFoosteps;
    public StepType[] crouchFoosteps;

    RaycastHit stepHit;

    public Transform Feet;
    public PlayerCam alignCamera { get; private set; }

    private bool canControl = true;
    public float controllerHeight
    {
        get => controller.height;
        set
        {
            controller.center = Vector3.up * value / 2f;
            controller.height = value;
        }
    }
    public float CrouchHeight => crouchHeight;
    private void Start()
    {
        instance = this;
        controller = GetComponent<CharacterController>();
        standHeight = controllerHeight;
        crouchHeight = controllerHeight / 2f;
        stepLength = walkSpeed * 0.8f;  //Ширина шага
        alignCamera = GetComponentInChildren<PlayerCam>();
        foostepsSource = Feet.GetComponent<AudioSource>();
        tracer = GetComponentInChildren<SmoothPointTracer>();
    }
    private void Update()
    {
        if(canControl)
            MoveUpdate();
    }
    private void CheckPosState()
    {
        if (crouch && CanCrouch())
            states = PlayerPosState.Crouch;
        else
        {
            if (states == PlayerPosState.Crouch)
                if (!CanStand()) return;
            if (run)
                states = PlayerPosState.Run;
            else
                states = PlayerPosState.Walk;
        }
    }
    public void Teleport(Transform point)
    {
        StartCoroutine(teleportation(point));
    }
    IEnumerator teleportation(Transform point)
    {
        canControl = false;
        transform.position = point.position;
        yield return new WaitForSecondsRealtime(0.1f);
        canControl = true;
    }
    private void PlayFootstep()
    {
        if (stepHit.transform == null)
            return;
        StepType[] types = null;
        var stepVolume = 1f;
        switch (states)
        {
            case PlayerPosState.Walk:
                types = walkFoosteps;
                stepVolume = walkStepVolume;
                break;
            case PlayerPosState.Run:
                types = runFoosteps;
                stepVolume = runStepVolume;
                break;
            case PlayerPosState.Crouch:
                types = crouchFoosteps;
                stepVolume = crouchStepVolume;
                break;
        }
        var type = findByTag(types, stepHit.transform.tag);
        //print(type.groundTag);
        if (type != null)
            foostepsSource.PlayOneShot(type.steps[Random.Range(0, type.steps.Length)], stepVolume);
    }
    StepType findByTag(StepType[] types, string tag)
    {
        for (int i = 0; i < types.Length; i++)
        {
            if (types[i].groundTag == tag)
                return types[i];
        }
        return null;
    }
    private void TryPlayFootstep(float curSpeed)
    {
        if (timer < Time.time)
        {
            PlayFootstep();
            timer = Time.time + stepLength / curSpeed;
        }
    }
    private bool CanCrouch()
    {
        RaycastHit hit2;
        Physics.Raycast(Feet.position, Vector3.down, out hit2, standHeight, groundMask);

        return !(hit2.transform == null || hit2.point.y - transform.position.y > crouchHeight);
    }
    private bool CanStand()
    {
        var r = !Physics.Raycast(transform.position, Vector3.up, standHeight, ceilingMask);
        return r;
    }
    private bool IsGrounded()
    {
        return Physics.Raycast(Feet.position, Vector3.down, out stepHit, controllerHeight, groundMask);
    }

    public void UpdateInputVector(InputAction.CallbackContext context)
    {
        inVector = context.ReadValue<Vector2>();
    }
    public void UpdateCrouchButton(InputAction.CallbackContext context)
    {
        crouch = context.ReadValueAsButton();
    }
    public void UpdateRunButton(InputAction.CallbackContext context)
    {
        run = context.ReadValueAsButton();
    }
    void MoveUpdate()
    {
        CheckPosState();
        //Falling system
        IsGrounded();
        controller.Move(Physics.gravity * Time.deltaTime);

        var motion = new Vector3(inVector.x, 0f, inVector.y);
        moveDirection = transform.TransformDirection(motion).normalized;    //From world to local space

        float curSpeed = 0f;
        switch (states)
        {
            case PlayerPosState.Walk:
                curSpeed = walkSpeed;
                controllerHeight = standHeight;
                break;
            case PlayerPosState.Crouch:
                curSpeed = crouchSpeed;
                controllerHeight = crouchHeight;
                break;
            case PlayerPosState.Run:
                curSpeed = runSpeed;
                controllerHeight = standHeight;
                break;
        }
        controller.Move(moveDirection * curSpeed * Time.deltaTime);
        var curVel = controller.velocity.magnitude;
        if (curVel > curSpeed / 2f)
            TryPlayFootstep(curSpeed * curVel / curSpeed);
    }

    public IEnumerator TranslateAtPosition(Transform point, float lerpSpeed = 10f, float epsilon = 0.1f)
    {
        while (Vector3.Distance(transform.position, point.position) > epsilon)
        {
            transform.position = Vector3.Lerp(transform.position, point.position, Time.deltaTime * lerpSpeed);
            yield return new WaitForEndOfFrame();
        }
        transform.position = point.position;
    }
    public IEnumerator TranslateAtPosition(Vector3 pos, float lerpSpeed = 10f, float epsilon = 0.1f)
    {
        while (Vector3.Distance(transform.position, pos) > epsilon)
        {
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * lerpSpeed);
            yield return new WaitForEndOfFrame();
        }
        transform.position = pos;
    }
}
