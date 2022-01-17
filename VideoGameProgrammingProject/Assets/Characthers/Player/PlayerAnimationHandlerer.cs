using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandlerer : MonoBehaviour
{

    private Animator playerAnimator;
    private PlayerMovementSystem playerMovement;


    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovementSystem>();
    }


    void Update()
    {
        playerAnimator.SetBool("isWalking", playerMovement.isPlayerWalking());
        playerAnimator.SetBool("isRunning", playerMovement.isPlayerRunning());

    }
}
