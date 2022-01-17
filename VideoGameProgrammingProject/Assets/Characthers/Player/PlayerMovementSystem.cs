using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class PlayerMovementSystem : MonoBehaviour
{
    // Andrea Mansi - Università Degli Studi di Udine

    // In this script, directions are updated with camera rotation. Forward direction (so others) is always changed/corrected depending on where camera is watching.

    // Reference to the sub-script
    private InputHandler _input;

    [Header(" - - - Movement Main Parameters - - - ")]
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float RunningSpeedMultiplier;
    [SerializeField] private float RunningRotationMultiplier;
    [SerializeField] private float MaxStamina = 100.0f;
    [SerializeField] private float timeStep = 0.1f;
    [SerializeField] private float raiseStep = 1.0f;
    [SerializeField] private float dropStep = 3.0f;

    private float stamina;
    private bool dropStaminaInvoked = false;
    private bool raiseStaminaInvoked = false;
    private StaminaBar staminaUI;

    [Header(" - - - References - - - ")]
    [SerializeField] private Camera Camera;

    // Player movement status values
    private bool isWalking;
    private bool isRunning;
    private bool isFreezed = false; // if freezed, movement input is disabled, used while performing certain actions which requires no movement.

    private void Awake()
    {
        stamina = MaxStamina; // starting stamina = full
        _input = GetComponent<InputHandler>(); 
    }

    private void Start()
    {
        staminaUI = GameObject.FindGameObjectWithTag("StaminaSlider").GetComponent<StaminaBar>();
    }

    void Update()
    {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);

        if (!isFreezed) // if moovement is not freezed for some reason
        {
            var movementVector = MoveTowardTarget(targetVector);

            RotateTowardMovementVector(movementVector.normalized); // normalized vector must be used in order to get same speed in each direction
            UpdateStatus(movementVector);

        }
        else if (isFreezed)
        {
            var movementVector = new Vector3(0, 0, 0);
            UpdateStatus(movementVector);
        }
    }

    private void UpdateStatus(Vector3 movementVector)
    {
        if(movementVector != new Vector3(0, 0, 0))
        {
            isWalking = true;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Joystick1Button0))
            {
                isWalking = false;
                isRunning = true;
                useStamina();
            }
            else
            {
                increaseStamina();
                isRunning = false;
            }
            if(stamina <= 0)
            {
                isWalking = true;
                isRunning = false;
            }
        }
        else
        {
            increaseStamina();
            isWalking = false;
            isRunning = false;
        }
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = MovementSpeed * Time.deltaTime;
        if (isRunning)
        {
            speed = speed * RunningSpeedMultiplier;
        }

        targetVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector.normalized * speed;
        transform.position = targetPosition;
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if(movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        var rotSpeed = RotationSpeed;
        if (isRunning)
        {
            rotSpeed = RotationSpeed * RunningRotationMultiplier;
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotSpeed);
    }

    public bool isPlayerWalking()
    {
        return isWalking;
    }

    public bool isPlayerRunning()
    {
        return isRunning;
    }

    public void setFreezeMotion(bool flag)
    {
        isFreezed = flag;
    }

    private void increaseStamina()
    {
        if (stamina < MaxStamina)
        {
            if(raiseStaminaInvoked == false)
            {
                raiseStaminaInvoked = true;
                StartCoroutine(raiseStamina());
            }
        }
        staminaUI.setValue(stamina);
    }

    private void useStamina()
    {
        if (stamina > 0)
        {
            if (dropStaminaInvoked == false)
            {
                dropStaminaInvoked = true;
                StartCoroutine(dropStamina());
            }
        }
        staminaUI.setValue(stamina);
    }

    private IEnumerator dropStamina()
    {
        stamina = stamina - dropStep;
        if(stamina < 0)
        {
            stamina = 0;
        }
        yield return new WaitForSeconds(timeStep);
        dropStaminaInvoked = false;
    }

    private IEnumerator raiseStamina()
    {
        stamina = stamina + raiseStep;
        if(stamina > MaxStamina)
        {
            stamina = MaxStamina;
        }
        yield return new WaitForSeconds(timeStep);
        raiseStaminaInvoked = false;
    }
}
