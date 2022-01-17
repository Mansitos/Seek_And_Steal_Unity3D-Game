using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    // Andrea Mansi - Università Degli Studi di Udine

    [Header(" - - - Follow Target Behaviour - - - ")]

    [Tooltip("The target reference (the player). Who the AI agent is searching for.")]
    [SerializeField] GameObject targetRef;
    [Tooltip("Minimum amount of space between agent and target when target reached.")]
    [SerializeField] float minDistanceFromTarget;
    [Tooltip("If true, agent will be forced to lookAt target when following it and under min. distance")]
    [SerializeField] bool faceTargetWhenFollowing = false;
    [Tooltip("The distance under forced lookAt target is executed (if enabled)")]
    [SerializeField] float minDistanceToForceTargetFacing;
    [Tooltip("When target is lost (vision is lost) it's position will remain known for this amount of time. Use small values (1-2 seconds)." +
            " This is used in order to help AI to don't lost player too easily under certain extreme movements.")]
    [SerializeField] float lostTrackDelay = 1.0f;
    [Tooltip("Speed when chasing target")]
    [SerializeField] float runningSpeed = 2.0f;
    
    // Internal script references
    private PlayerHidingBehaviour targetHidingStatus;
    private Animator agentAnimator;
    private AttackVolumeManager attackVolumeManager;
    private StatusIconHandlerer iconDisplayer;

    // Internal status variables
    private bool isChasingTarget = false;
    private Vector3 lastTargetPosition; // last known position of target
    private bool isRunning;
    private bool targetUnderVision = false;
    private bool lostTrackProcedureAlreadyInvoked = false;
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private int nextPoint = 0; // next patrol point index to reach
    private bool isPatrolling;
    private bool hasToWaitNextAttack = false;
    private bool unlockNextAttackAlreadyInvoked = false;
    private bool isGoingBackToStartPos = false;

    [Header(" - - - Patrol Behaviour - - - ")]
    [Tooltip("If True, agent will perform patrolling of the assigned path")]
    [SerializeField] bool executePatrol = false;
    [Tooltip("Patrol points (in order)")]
    [SerializeField] Transform[] pathPoints;
    [Tooltip("How much time to wait before go back to patrol (when target is lost)")]
    [SerializeField] float waitTimeToGoBackPatrolling = 1.0f;
    [Tooltip("Speed when patrolling")]
    [SerializeField] float patrollingSpeed = 1.0f;

    [Header(" - - - Attack Behaviour - - - ")]
    [Tooltip("Delay between each attack")]
    [SerializeField] float timeBetweenAttacks = 1.0f;
    [Tooltip("When attack is performed, attack animation starts. This value indicates after how many seconds the check if player is being it must be performed. " +
            "Note that if it is set to zero, it is perfomed when attack starts, so player has 0 possibility to escape the attack volume, it will always be hit. " +
            "If too high the AI will always miss if player is moving. Tweak carefully.")]
    [SerializeField] float timeOffsetForHitCheckAfterAnimationTrigger = 0.1f;
    [Tooltip("Attack Volume reference")]
    [SerializeField] GameObject attackVolumeRef;
    [Tooltip("Attack damage (in lifes)")]
    [SerializeField] int attackDamage = 1;

    [Header(" - - - Others - - - ")]
    [SerializeField] GameObject iconRef;

    [Header(" - - - DEBUG - - - ")]
    [SerializeField] GameObject debugProp;
    [SerializeField] bool debugAgentDestination = true;
    [SerializeField] bool debugStatus = false;


    private GameManager gm;
    private NavMeshAgent agent;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (targetRef == null)
        {
            targetRef = gm.getPlayerRef();
        }

        // internal references and variables initialization code
        agent = GetComponent<NavMeshAgent>();
        targetHidingStatus = targetRef.GetComponent<PlayerHidingBehaviour>();
        agentAnimator = GetComponent<Animator>();
        attackVolumeManager = attackVolumeRef.GetComponent<AttackVolumeManager>();
        // wait time is because it is needed that ai agent land on terrain after spawn!
        Invoke("getStartingPosition", 2.5f);
        iconDisplayer = iconRef.GetComponent<StatusIconHandlerer>();

        if (debugAgentDestination)
        {
            debugProp = Instantiate(debugProp, transform.position, Quaternion.identity);
        }

        if (executePatrol)
        {
            InitializePatrolling();
        }
    }

    private void InitializePatrolling()
    {  
        isPatrolling = true;
        updateAgentMovingSpeed();
        GotoNextPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {

        FollowTargetBehaviour();

        if (debugAgentDestination)
        {
            UpdateDebugDestination();
        }
    }

    private void updateAgentMovingSpeed()
    {
        if (isPatrolling)
        {
            agent.speed = patrollingSpeed;
            agent.stoppingDistance = 0.1f; // Do not set 0.0 or bug happens
        }
        else if (isRunning || isChasingTarget)
        {
            if (isChasingTarget)
            {
                isRunning = true;
            }
            agent.speed = runningSpeed;
            agent.stoppingDistance = minDistanceFromTarget;
        } 
        else if (isRunning && isPatrolling)
        {
            Debug.LogError("Can't patrol and chase target at the same time! There are some errors in the agent logic!");
        }
    }

    private void UpdateDebugDestination()
    {
         // show agent destination and update it
         debugProp.SetActive(true);
         debugProp.transform.position = lastTargetPosition;

    }

    private void FollowTargetBehaviour()
    {
        if (isChasingTarget == true) // agent is following the target
        {
            // NavMesh set destination to target position
            // NB: Update only if not too close to target
            float targetDistance = Vector3.Distance(transform.position, targetRef.transform.position);

            if (targetDistance >= minDistanceFromTarget) // not too close to agent
            {
                if (targetUnderVision == true) // agent is following the target and sees the target
                {
                    if (targetHidingStatus.getPlayerIsHiding() == false) // target under vision and not hiding but already being followed (then: keep follow)
                    {
                        lastTargetPosition = targetRef.transform.position;
                        agent.SetDestination(lastTargetPosition);
                        isRunning = true;
                        updateAgentMovingSpeed();
                    }
                    else // target under vision and hiding but already being followed (then: keep follow)
                    {
                        lastTargetPosition = targetRef.transform.position;
                        agent.SetDestination(lastTargetPosition);
                        isRunning = true;
                        updateAgentMovingSpeed();
                    }
                }
                else // target no more under vision, (hiding or not) but already being followed (lost track, reach last setted position)
                {
                    agent.SetDestination(lastTargetPosition);
                    if (lostTrackProcedureAlreadyInvoked == false) {
                        lostTrackProcedureAlreadyInvoked = true;
                    Invoke("lostTrackBehaviour", lostTrackDelay);
                    }

                    // Lost position, but until the invoke of lostTrack is done, keep following it
                    lastTargetPosition = targetRef.transform.position;
                    agent.SetDestination(lastTargetPosition);
                    isRunning = true;
                    updateAgentMovingSpeed();

                    // reset waiting time for attacks!
                    hasToWaitNextAttack = false;
                    unlockNextAttackAlreadyInvoked = false;
                }
            }
            else // too close to target; all status remain the same but target stop run until target distance > threshold
            {
                lastTargetPosition = transform.position;
                isRunning = false;
                updateAgentMovingSpeed();

                if (!hasToWaitNextAttack) // Close to target, if is not already attacking, then attack
                {
                    executeAttack();
                }
            }

            // Force look at target behaviour
            // when very close to target force look at it (if this is disabled, very skilled players can go behind AI view cone very quickly and AI lose track of player.
            if (faceTargetWhenFollowing && (targetDistance <= minDistanceToForceTargetFacing) && isChasingTarget)
            {
                // Rotate agent in order to look at the target (only x and z axis, y fixed)
                Vector3 lookPosition = targetRef.transform.position;
                lookPosition.y = transform.position.y; // y is fixed at agent height
                transform.LookAt(lookPosition);
            }
        }
        else if (isChasingTarget == false) // agent was not following the target
        {
            if (targetUnderVision && targetHidingStatus.getPlayerIsHiding() == true) // agent was not following, target is under vision but hiding (not follow)
            {

            }
            else if (targetUnderVision && targetHidingStatus.getPlayerIsHiding() == false) // agent was not following, target is under vision but not hiding (start to follow)
            {
                isChasingTarget = true;
                isRunning = true;
                isPatrolling = false; // stop patrolling then
                updateAgentMovingSpeed();
                iconDisplayer.displayStatus("spotted");
            }
        }

        // Checks if TARGET POSITION IS REACHED
        // If reached, stop
        if (Vector3.Distance(transform.position, lastTargetPosition) <= (agent.stoppingDistance) * 1.10)
        {
            if (isRunning)
            {
                isRunning = false;
                updateAgentMovingSpeed();
            }

            if (isGoingBackToStartPos == true) // he reached the position after goBackToStartingPosition procedure
            {
                isPatrolling = false;
                isGoingBackToStartPos = false;
                gameObject.transform.rotation = startingRotation; // restoring initial position rotation
                updateAgentMovingSpeed();
            }

        }

        // Choose the next destination point when the agent gets
        // close to the current one.
        if (isPatrolling && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GotoNextPatrolPoint();
        }

        if (debugStatus) { Debug.Log("hasVision:" + targetUnderVision + "|isFollowing:" + isChasingTarget + "|Patrolling:" + isPatrolling + "|PlayerHiding:" + targetHidingStatus.getPlayerIsHiding() + "|canAttack:" + !hasToWaitNextAttack); }
    }

    private void executeAttack()
    {
        agentAnimator.SetTrigger("Attack"); // trigger attack animation
        hasToWaitNextAttack = true;

        if (unlockNextAttackAlreadyInvoked == false)
        {
            Invoke("unlockNextAttack", timeBetweenAttacks);
            unlockNextAttackAlreadyInvoked = true;
        }

        Invoke("checkAttackHit", timeOffsetForHitCheckAfterAnimationTrigger);

    }

    private void checkAttackHit()
    {
        bool hit = attackVolumeManager.isPlayerInsideDamageVolume();
        if (hit)
        {
            targetRef.GetComponent<HealthManager>().removeLifes(attackDamage);
        }
    }

    private void unlockNextAttack()
    {
        hasToWaitNextAttack = false;
        unlockNextAttackAlreadyInvoked = false;
    }

    void GotoNextPatrolPoint()
    {
        // Returns if no points have been set up
        if (pathPoints.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.SetDestination(pathPoints[nextPoint].position);

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        nextPoint = (nextPoint + 1) % pathPoints.Length;
    }


    public bool isAgentRunning()
    {
        return isRunning;
    }

    public bool isAgentPatrolling()
    {
        return isPatrolling;
    }

    public bool hasTargetUnderVision()
    {
        return targetUnderVision;
    }

    public void setTargetUnderVision(bool flag)
    {
        targetUnderVision = flag;
    }

    private void lostTrackBehaviour()
    {
        lostTrackProcedureAlreadyInvoked = false;
        isChasingTarget = false;
        updateAgentMovingSpeed();
        searchAfterLostTrackBehaviour();
        iconDisplayer.displayStatus("lost");
    }

    private void searchAfterLostTrackBehaviour()
    {
        Debug.Log("SEACHING FOR LOST TARGET! TO IMPLEMENT");
        Invoke("resumePatrollingProcedure", waitTimeToGoBackPatrolling);
    }

    private void resumePatrollingProcedure()
    {
        if (executePatrol == false) // if it was not a patrolling guard, just go back to the initial starting position 
        {
            goBackToStartingPositionBehaviour(); 
        }
        else if (executePatrol == true)
        {
            isPatrolling = true;
        }
        updateAgentMovingSpeed();
    }

    private void goBackToStartingPositionBehaviour()
    {
        lastTargetPosition = startingPosition;
        agent.SetDestination(lastTargetPosition);
        isPatrolling = true;
        isRunning = false;
        isGoingBackToStartPos = true;
    }

    public void startChasingAfterSoundClue()
    {
        if(isChasingTarget == false) // the sound clue triggered a new chasing
        {
            iconDisplayer.displayStatus("sound");
        }

        isPatrolling = false;
        isGoingBackToStartPos = false;
        isChasingTarget = true;
        isRunning = true;

    }

    void getStartingPosition()
    {
        startingPosition = GetComponent<Transform>().position;
        startingRotation = GetComponent<Transform>().rotation;
    }
}
