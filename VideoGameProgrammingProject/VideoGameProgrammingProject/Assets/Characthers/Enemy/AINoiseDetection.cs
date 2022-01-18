using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINoiseDetection : MonoBehaviour
{

    [Header(" - - - Sound Volume Detection Parameters - - - ")]
    public float radius;

    [Header(" - - - References - - - ")]
    public GameObject targetRef;
    public GameObject soundDetectionVolumeRef;
    private EnemyAI agent;

    // Internal status
    private bool canHearPlayer;
    private GameManager gm;

    [Header(" - - - DEBUG - - - ")]
    [SerializeField] bool logEvents = false;


    void Start()
    {

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (targetRef == null)
        {
            targetRef = gm.getPlayerRef();
        }

        soundDetectionVolumeRef.GetComponent<SphereCollider>().radius = radius;
        agent = GetComponent<EnemyAI>();
        
    }

    public void receiveSoundClue()
    {
        var isPlayerRunning = targetRef.GetComponent<PlayerMovementSystem>().isPlayerRunning();
        if (isPlayerRunning == false)
        {
            if (logEvents)
            {
                Debug.Log("Sound clue received, but player is not running! ignore!");
            }
        }
        else if(isPlayerRunning == true)
        {
            if (logEvents)
            {
                Debug.Log("Sound clue received, running! start chasing!");
            }

            // Tell the agent to start chase the target!
            agent.startChasingAfterSoundClue();
        }
    }

    void Update()
    {
        
    }
}
