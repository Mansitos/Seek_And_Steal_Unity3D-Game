using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHandlerer : MonoBehaviour
{

    public Animator enemyAnimator;
    public EnemyAI enemyAI;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
    }


    void Update()
    {
        enemyAnimator.SetBool("isWalking", enemyAI.isAgentPatrolling());
        enemyAnimator.SetBool("isRunning", enemyAI.isAgentRunning());
    }
}
