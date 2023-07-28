using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using static PlayerInputController;
using static UnityEditor.Timeline.TimelinePlaybackControls;


public enum eAnimState
{
    IDLE,
    WALK,
    RUN,
    SHOOT,
    RELOAD,
    THROW,
    MELEE_ATTACK,
}


public enum eControlScheme
{
    CONTROL_1,
    CONTROL_2,
}


[Serializable]
public struct AnimationInfos
{
    public eAnimState State;
    public string AnimationName;
}


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerInput playerInput;
    InputAction MoveUp;
    InputAction MoveDown;
    InputAction MoveLeft;
    InputAction MoveRight;

    public float MoveSpeed = 5f;
    public float RunSpeed = 30f;
    public float RotationSpeed = 360f;

    PlayerInputController Controller;
    CharacterController CharacterController;

    public eControlScheme CurrentControlScheme;

    public bool UseRelativeMovement = true;

    public float Gravity = -9.81f;


    public eAnimState CurrentAnimState = eAnimState.IDLE;

    public List<AnimationInfos> AnimInfos;
    private Dictionary<eAnimState, string> AnimToStringMap;

    private Animator Animator;

    public CinemachineVirtualCamera VirtualCamera;

    public void ChangeAnimationState(eAnimState newState)
    {
        if(CurrentAnimState == newState)
        {
            return;
        }

        string AnimationState = GetAnimationName(newState);

        if(AnimationState != null)
        {
            CurrentAnimState = newState;
            Animator.Play(AnimationState);
        }
        else
        {
            Debug.LogError("Trying play an invalid animation");
        }
        
    }


    public void CreateAnimationDictionary()
    {
        AnimToStringMap = new();

        foreach(AnimationInfos anim in  AnimInfos) 
        {
            AnimToStringMap.Add(anim.State,anim.AnimationName);
        }
    }

    public string GetAnimationName(eAnimState state) 
    {
        string toReturn = null;

        if(AnimToStringMap.TryGetValue(state, out toReturn)) 
        {
            return toReturn;
        }

        return toReturn;
    }


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        PlayerInputController inputActions = new PlayerInputController();
        inputActions.PlayerControls.Enable();
        Controller = inputActions;
        Animator = GetComponent<Animator>();

        CharacterController = GetComponent<CharacterController>();
        CreateAnimationDictionary();

        MoveUp = inputActions.PlayerControls.MoveUp;
        MoveDown = inputActions.PlayerControls.MoveDown;
        MoveLeft = inputActions.PlayerControls.MoveLeft;
        MoveRight = inputActions.PlayerControls.MoveRight;

    }

    Vector3 GetMouseRotation()
    {
        Vector2 currentMousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(currentMousePosition);
        Vector3 direction = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            direction = hit.point - transform.position;
            direction.Normalize();
            direction.y = 0;
        }

        return direction;
    }

    void HandleMouseInput()
    {
        //Vector3 direction = GetMouseRotation();
        //if (direction != Vector3.zero)
        //    transform.rotation = Quaternion.LookRotation(direction);

        float mouseX = Input.GetAxis("Mouse X");
        float rotationAmount = mouseX * RotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotationAmount, 0f);
    }

    void HandleJoystickInput()
    {
        Vector2 joystickPosition = Controller.PlayerControls.Rotation.ReadValue<Vector2>();
        Vector3 direction = new Vector3(joystickPosition.x, 0f, joystickPosition.y);

        if (joystickPosition.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

    }

    void CharacterControllerMovement(Vector3 direction)
    {
        float moveSpeed = 0.0f;

        if(Controller.PlayerControls.Run.ReadValue<float>() > 0.0f)
        {
            moveSpeed = RunSpeed;
            ChangeAnimationState(eAnimState.RUN);
        }
        else
        {
            ChangeAnimationState(eAnimState.WALK);
            moveSpeed = MoveSpeed;
        }

        CharacterController.Move(direction*moveSpeed*Time.deltaTime);
    }

    void HandleTurningBasedOnControlScheme()
    {

    }

    void HandleMovementBasedOnControllerScheme()
    {
        switch (CurrentControlScheme)
        {
            case eControlScheme.CONTROL_1:
                {
                    if (MoveUp.ReadValue<float>() > 0.0f)
                    {
                        Debug.Log("Called Move Up");
                        //transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

                        if (UseRelativeMovement)
                        {
                            CharacterControllerMovement(transform.forward);
                        }
                        else
                        {
                            CharacterControllerMovement(Vector3.forward);
                        }
                    }

                    break;
                }

            case eControlScheme.CONTROL_2:
                {

                    if (MoveUp.ReadValue<float>() > 0.0f)
                    {
                        Debug.Log("Called Move Up");
                        //transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

                        if (UseRelativeMovement)
                        {
                            CharacterControllerMovement(transform.forward);
                        }
                        else
                        {
                            CharacterControllerMovement(Vector3.forward);
                        }
                    }

                    if (MoveDown.ReadValue<float>() > 0.0f)
                    {
                        Debug.Log("Called Move Down");
                        //transform.position += (-Vector3.forward) * moveSpeed * Time.deltaTime;
                        if (UseRelativeMovement)
                        {
                            CharacterControllerMovement(-transform.forward);
                        }
                        else
                        {
                            CharacterControllerMovement(Vector3.back);
                        }
                    }

                    if (MoveLeft.ReadValue<float>() > 0.0f)
                    {
                        //transform.position += (-Vector3.right) * moveSpeed * Time.deltaTime;
                        if (UseRelativeMovement)
                        {
                            CharacterControllerMovement(-transform.right);
                        }
                        else
                        {
                            CharacterControllerMovement(Vector3.left);
                        }

                    }

                    if (MoveRight.ReadValue<float>() > 0.0f)
                    {
                        //transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                        if (UseRelativeMovement)
                        {
                            CharacterControllerMovement(transform.right);
                        }
                        else
                        {
                            CharacterControllerMovement(Vector3.right);
                        }
                    }

                    break;
                }
        }
    }

    void HandleMovement()
    {
        bool isMoving = MoveUp.ReadValue<float>() > 0.0f || MoveDown.ReadValue<float>() > 0.0f
                        ||MoveLeft.ReadValue<float>()>0.0f || MoveRight.ReadValue<float>() > 0.0f;

        if(!isMoving)
        {
            ChangeAnimationState(eAnimState.IDLE);
        }

        HandleMovementBasedOnControllerScheme();
       
    }

    void HandleGravity()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            Debug.Log("Came here");
            float groundHeight = hit.point.y;
            transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
        }

        Vector3 moveDirection = new Vector3(0, Gravity, 0);
        CharacterController.Move(moveDirection * Time.deltaTime);
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log("Character controller grounded"+CharacterController.isGrounded.ToString());

        if(Input.GetMouseButtonDown(0)) 
        {
            ChangeAnimationState(eAnimState.MELEE_ATTACK);
        }

        HandleGravity();
        HandleMovement();
        HandleMouseInput();

        //CharacterController.Move(Physics.gravity * Time.deltaTime);
        //HandleJoystickInput();
        

        

    }
}
