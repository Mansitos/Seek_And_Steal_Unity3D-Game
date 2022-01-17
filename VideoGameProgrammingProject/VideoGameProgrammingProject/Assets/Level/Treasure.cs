using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Treasure : MonoBehaviour
{
    [Header(" - - - Pick Up Logic - - - ")]
    [SerializeField] float timeToPickUp = 2.0f;
    [SerializeField] float pickupAreaRadius = 2.0f;
    [SerializeField] GameObject playerRef;
    [SerializeField] GameObject winHandlererRef;
    [SerializeField] GameObject stealingUI;

    private PlayerMovementSystem playerMovementSystem;
    private bool playerIsPickingUp = false;
    private TreasureCounter tresUI;

    [Header(" - - - DEBUG - - - ")]
    [SerializeField] bool logEvents;


    void Start()
    {
        // Setting up pickable area radius
        SphereCollider triggerCollider = GetComponent<SphereCollider>();
        triggerCollider.radius = pickupAreaRadius;
        tresUI = winHandlererRef.GetComponent<TreasureCounter>();

        playerMovementSystem = playerRef.GetComponent<PlayerMovementSystem>();
        stealingUI = GameObject.FindGameObjectWithTag("StealingProgressBar");
    }

    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Joystick1Button2))
            {
                if (!playerIsPickingUp)
                {
                    if (logEvents) { Debug.Log("Object pick up procedure started by player!"); }
                    playerMovementSystem.setFreezeMotion(true);

                    stealingUI.GetComponent<StealProgressBar>().setActive(true);

                    Invoke("ExecutePickUp", timeToPickUp);
                }

                if (logEvents) { Debug.Log("Player is already picking up object!"); }

            }
        }
    }

    private void ExecutePickUp()
    {
        if (logEvents) { Debug.LogError("Object picked up by player!"); }
        playerMovementSystem.setFreezeMotion(false);
        tresUI.addOneTreasureOnCounter();
        stealingUI.GetComponent<StealProgressBar>().setActive(false);
        Destroy(this);
        Destroy(this.gameObject);
    }
}
