using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS SCRIPT IS A CUSTOMIZED VERSION OF A PUBLIC GITHUB REPOSITORY FROM Comp3interactive
// https://github.com/Comp3interactive/FieldOfView

public class AIVisualDetection : MonoBehaviour
{
    [Header(" - - - Vision Cone Parameters - - - ")]
    public float radius;
    [Range(0,360)]
    public float angle;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    [Header(" - - - References - - - ")]
    public GameObject targetRef;
    private EnemyAI agent;

    // Internal status
    private bool canSeePlayer;
    private GameManager gm;

    [Header(" - - - DEBUG - - - ")]
    [SerializeField] bool logEvents = false;

    private void Start()
    {

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (targetRef == null)
        {
            targetRef = gm.getPlayerRef();
        }

        agent = GetComponent<EnemyAI>();
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    agent.setTargetUnderVision(true);
                    if (logEvents) { Debug.Log("Target detected!"); }
                }
                else
                {
                    canSeePlayer = false;
                    agent.setTargetUnderVision(false);
                    if (logEvents) { Debug.Log("Target lost!"); }
                }
            }
            else
            {
                canSeePlayer = false;
                agent.setTargetUnderVision(false);
                if (logEvents) { Debug.Log("Target lost!"); }
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
            agent.setTargetUnderVision(false);
            if (logEvents) { Debug.Log("Target lost!"); }
        }
    }

    public bool isTargetDetected()
    {
        return canSeePlayer;
    }

}
