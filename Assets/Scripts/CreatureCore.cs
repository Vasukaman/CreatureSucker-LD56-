using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class CreatureCore : MonoBehaviour, iSuckable
{
    // Variables
    [SerializeField] private bool behaviorLocked;                     // Prevents switching behaviors
    [SerializeField] private float behaviorLockedTime;                  // Timer for locked behavior duration
    [SerializeField] public Transform[] scatterPoints;                // Predefined scatter points for chaotic movement
    [SerializeField] public Transform[] walkingPoints;                // Predefined walking points for regular movement

    [SerializeField] private float _distanceToPlayerToBeAlert = 2;
    [SerializeField] private float _distanceToStop = 0.5f;
    bool behaviorStarted = false;

    [SerializeField] float walkingSpeed = 5;
    [SerializeField] float runningSpeed = 7.5f;


    [SerializeField] private PlayerDataSO _playerDataSO;


    [SerializeField] private Animation _animation;

    // NavMeshAgent for movement
    private NavMeshAgent navMeshAgent;

    // State Management
    private enum State
    {
        Wander,
        RunAway,
        Taunt,
        Freeze,
        PairUp,
        RandomMischief,
        Idle // New Idle state
    }
    [SerializeField] private State currentState;                      // Current behavior state
    [SerializeField] private State newStateOnUnlock;
    void ChangeState(State _newState)
    {
        if (!behaviorLocked)
        {
            currentState = _newState;
            behaviorStarted = false;
        }
        else
        {
            newStateOnUnlock = _newState;
        }
    }

    void Start()
    {
        _animation["walking_ag"].time = Random.Range(0,10);
        //_animation.Play("walking_ag", 0, YOUR_TIME_INDEX_HERE);
        // Initialization code here
        navMeshAgent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        ChangeState(State.Wander);            // Start in the Wander state
    }

    void Update()
    {
        HandleBehaviorLockCountdown();                // Countdown for behavior lock
        EvaluateSurroundings();                      // Check player proximity and nearby creatures
        HandleStateTransitions();                    // Manage state transitions based on conditions
        ExecuteState();
     
    }


    private void ExecuteState()

    {

        // Execute current state behavior
        switch (currentState)
        {
            case State.Wander:
                Wander();
                break;
            case State.RunAway:
                RunAway();
                break;
            case State.Taunt:
                TauntPlayer();
                break;
            case State.Freeze:
                FreezeWhenWatched();
                break;
            case State.PairUp:
                PairUp();
                break;
            case State.RandomMischief:
                RandomMischief();
                break;
            case State.Idle: // Handle Idle state
                Idle();
                break;
        }

        behaviorStarted = true;
    }
    private void Idle()
    {
        if (!behaviorStarted)
        { SetBehaviorLock(Random.Range(0.5f, 5f)); }
        // Check if the creature should stand still or choose a new point to wander to
        if (ShouldWander())
        {
            // Transition back to Wander state
            ChangeState(State.Wander);
        }
        else
        {
            // Optionally, you can add additional behavior while idle, like animations
            // For example, playing an idle animation or looking around
            PlayIdleAnimation(); // Implement this method to handle idle animations
        }
    }

    // Behavior Lock Countdown
    private void HandleBehaviorLockCountdown()
    {
        if (behaviorLocked)
        {
            behaviorLockedTime-=Time.deltaTime;

            if (behaviorLockedTime <= 0)
            {
                behaviorLocked = false;
                ChangeState(newStateOnUnlock);
                    // Unlock behavior when countdown is over
            }
        }
    }

    // State Transition Management
    private void HandleStateTransitions()
    {
        // Check for priority states first
        if (IsPlayerNearby() && !behaviorLocked)
        {
            ChangeState(State.RunAway);             // Switch to RunAway state immediately
            return;
        }

        switch (currentState)
        {
            case State.Wander:
                // Check for conditions to transition to other states
                if (ShouldTaunt())
                {
                    ChangeState(State.Taunt);
                }
                else if (ShouldEngageInMischief())
                {
                    ChangeState(State.RandomMischief);
                }
                break;

            case State.Taunt:
                // Transition to RunAway after taunting
                break;

            case State.RunAway:
                // Check conditions to transition back to Wander
                break;

            case State.Freeze:
                // Check conditions to transition back to previous state
                break;

            case State.PairUp:
                // Conditions for switching back to Wander or other states
                break;

            case State.RandomMischief:
                // Logic to transition back to Wander or other states
                break;
        }
    }

    // Evaluation of Surroundings
    private void EvaluateSurroundings()
    {
        // Check proximity to player
        CheckPlayerProximity();
    }


    private bool ShouldWander()
    {
        // Implement logic to decide if the creature should wander again or stay idle
        // For example, using a random chance to determine if it should stay idle
        return Random.Range(0, 100) < 1; // 50% chance to wander again
    }
    private void Wander()
    {
        if (!behaviorStarted)
        {
            SelectRandomWalkingPoint();
            
        }

        navMeshAgent.speed = walkingSpeed;
        // Check if the creature is currently moving
        if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance <= _distanceToStop)
        {
            // Transition to Idle state after reaching the point
            if (ShouldIdle())
                ChangeState(State.Idle);
            else
               SelectRandomWalkingPoint();
        }
  
    }


    private void RunAway()
    {

        if (behaviorStarted) return;
        navMeshAgent.speed = runningSpeed;
            SelectRandomScatterPoint();

        if (IsPlayerNearby())
            SetBehaviorLock(0.7f);

            ChangeState(State.Idle);
        
        
    }
    private void PlayIdleAnimation()
    {
        // Implementation for playing idle animations
        // You can access the animator component here
    }

    private void TauntPlayer()
    {
        if (behaviorStarted) return;
        // Execute taunt animation and transition to RunAway
        PlayTauntAnimation();                         // Function to play taunt animation
        SetBehaviorLock(2);                          // Lock behavior for a brief moment
        ChangeState(State.Idle);            // Transition to RunAway after taunting
    }

    private void FreezeWhenWatched()
    {
        // Stop movement when player looks directly at the creature
        StopMovement();                              // Function to halt movement
    }

    private void PairUp()
    {
        // Follow another creature and mirror its behavior
        FindNearbyCreatureToFollow();                // Logic to find and follow another creature
    }

    private void RandomMischief()
    {
        if (behaviorStarted) return;    
        // Execute a random playful action
        PlayMischiefAnimation();                     // Function to play mischief animation
        SetBehaviorLock(2);                          // Lock behavior for a brief moment
        ChangeState(State.Wander);               // Transition back to Wander after mischief
    }

    // Helper Functions
    private void SetBehaviorLock(float duration)
    {
        behaviorLocked = true;
        behaviorLockedTime = duration;
    }

    private void CheckPlayerProximity()
    {
        // Check if the player is within a certain range
        // This can be done with a trigger collider or raycasting for simplicity
    }

    private bool ShouldEngageInMischief()
    {
        // Implement logic to determine if the creature should engage in Mischief
        return false;//Random.Range(0, 1000) < 5; // Example: 10% chance
    }

    private bool ShouldTaunt()
    {
        // Implement logic to determine if the creature should taunt
        return false;// Random.Range(0, 1000) < 10; // Example: 5% chance
    }

    private bool ShouldIdle()
    {
        return Random.Range(0, 100) < 40 ;
    }

    private bool IsPlayerNearby()
    {
        // Check if the player is within a certain range (pseudo-implementation)
        // Example implementation could use Physics.OverlapSphere or a trigger collider
        if (Vector3.Distance(transform.position, _playerDataSO.playerObject.transform.position) < _distanceToPlayerToBeAlert)
            return true;

        return false; // Placeholder for actual proximity check
    }
    private void SelectRandomWalkingPoint()
    {
        if (walkingPoints.Length > 1) // We need at least two points to exclude the closest one
        {
            // Get a list of distances to each point
            Transform[] sortedPoints = SortPointsByDistance(walkingPoints);

            // Select a random point from the top 3 (but not the closest one)
            int randomIndex = Random.Range(1, Mathf.Min(4, sortedPoints.Length)); // Exclude index 0 (the closest one)
            navMeshAgent.SetDestination(sortedPoints[randomIndex].position);
        }
    }

    private void SelectRandomScatterPoint()
    {
        if (scatterPoints.Length > 1) // We need at least two points to exclude the closest one
        {
            // Get a list of distances to each point
            Transform[] sortedPoints = SortPointsByDistance(scatterPoints);

            // Select a random point from the top 3 (but not the closest one)
            int randomIndex = Random.Range(1, Mathf.Min(4, sortedPoints.Length)); // Exclude index 0 (the closest one)
            navMeshAgent.SetDestination(sortedPoints[randomIndex].position);
        }
    }

    // Helper function to sort points by distance
    private Transform[] SortPointsByDistance(Transform[] points)
    {
        Transform[] sortedPoints = points.OrderBy(point => Vector3.Distance(transform.position, point.position)).ToArray();
        return sortedPoints;
    }
    // Placeholder Functions for Animations and Movement
    private void PlayTauntAnimation()
    {
        // Implementation for playing taunt animation
    }

    private void PlayMischiefAnimation()
    {
        // Implementation for playing mischief animation
    }

    private void StopMovement()
    {
        navMeshAgent.isStopped = true;              // Stop the NavMeshAgent
    }

    private void FindNearbyCreatureToFollow()
    {
        // Implementation for finding and following nearby creatures
    }



    private bool isBeingSucked = false;
    public bool GetIsBeingSucked()
    {
        return isBeingSucked;
    }

    public float GetDistanceFromTheZone()
    {
        Vector3 suctionZonePosition = _playerDataSO.playerObject.transform.position;
        return Vector3.Distance(transform.position, suctionZonePosition);
    }

    public void OnSuck()
    {
        navMeshAgent.enabled = false;
        isBeingSucked = true;
    }
}
