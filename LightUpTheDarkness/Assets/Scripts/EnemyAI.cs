using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Walk, Follow, Retreat, Attack, Idle }
    public float followDistance = 10f; // Adjust this distance as needed
    public float retreatDistance = 5f; // Distance at which the enemy starts retreating
    public float retreatSpeed = 2f; // Speed at which the enemy retreats
    public float walkingRadius = 5f; // Radius within which the enemy wanders in idle state
    public float attackDuration = 1.5f; // Duration of attack animation
    public float idleDuration = 5.5f; // Duration of attack animation
    public float runningSpeed = 2f; // Duration of attack animation

    private EnemyState currentState = EnemyState.Walk;
    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    private Vector3 initialPosition;
    private Vector3 idleDestination;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float idleTimer = 0f;

    [SerializeField] Animator EnemyAnimator;
    [SerializeField] RuntimeAnimatorController IdleAnimation;
    [SerializeField] RuntimeAnimatorController WalkAnimation;
    [SerializeField] RuntimeAnimatorController FollowAnimation;
    [SerializeField] RuntimeAnimatorController AttackAnimation;

    // Reference to the flashlight GameObject
    [SerializeField] GameObject flashlight;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;
        SetRandomIdleDestination();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        switch (currentState)
        {
            //case EnemyState.Walk:
            //    EnemyAnimator.runtimeAnimatorController = WalkAnimation;
            //    navMeshAgent.speed = 0.5f;

            //    if (distanceToPlayer <= followDistance)
            //    {
            //        currentState = EnemyState.Follow;
            //        break;
            //    }

            //    if (Vector3.Distance(transform.position, idleDestination) < navMeshAgent.stoppingDistance)
            //    {
            //        SetRandomIdleDestination();
            //    }
            //    break;

            case EnemyState.Walk:
                EnemyAnimator.runtimeAnimatorController = WalkAnimation;
                navMeshAgent.speed = 0.5f;

                if (distanceToPlayer <= followDistance)
                {
                    // Check if player is within the navmesh area
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(playerTransform.position, out hit, 1.0f, NavMesh.AllAreas))
                    {
                        currentState = EnemyState.Follow;
                        break;
                    }
                }

                if (Vector3.Distance(transform.position, idleDestination) < navMeshAgent.stoppingDistance)
                {
                    SetRandomIdleDestination();
                }
                break;

            //case EnemyState.Follow:
            //    EnemyAnimator.runtimeAnimatorController = FollowAnimation;
            //    navMeshAgent.speed = 1f;

            //    if (distanceToPlayer <= navMeshAgent.stoppingDistance) //check if attack is on cooldown
            //    {
            //        currentState = EnemyState.Attack;
            //        AttackPlayer();
            //    }
            //    else if (distanceToPlayer > followDistance)
            //    {
            //        currentState = EnemyState.Walk;
            //        SetRandomIdleDestination();
            //    }
            //    else
            //    {
            //        Vector3 directionToEnemy = (transform.position - flashlight.transform.position).normalized;
            //        if (Vector3.Dot(flashlight.transform.forward, directionToEnemy) > 0.5f && flashlight.GetComponent<Light>().enabled)
            //        {
            //            Vector3 retreatDirection = (transform.position - playerTransform.position).normalized;
            //            Vector3 retreatPosition = transform.position + retreatDirection * retreatDistance;

            //            currentState = EnemyState.Retreat;
            //            navMeshAgent.SetDestination(retreatPosition);
            //            transform.LookAt(playerTransform.position);
            //        }
            //        else
            //        {
            //            navMeshAgent.SetDestination(playerTransform.position);
            //        }
            //    }
            //    break;

            case EnemyState.Follow:
                EnemyAnimator.runtimeAnimatorController = FollowAnimation;
                navMeshAgent.speed = runningSpeed;

                if (distanceToPlayer <= navMeshAgent.stoppingDistance) //check if attack is on cooldown
                {
                    currentState = EnemyState.Attack;
                    AttackPlayer();
                }
                else if (distanceToPlayer > followDistance)
                {
                    currentState = EnemyState.Walk;
                    SetRandomIdleDestination();
                }
                else
                {
                    // Check if player is within the navmesh area
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(playerTransform.position, out hit, 1.0f, NavMesh.AllAreas))
                    {
                        Vector3 directionToEnemy = (transform.position - flashlight.transform.position).normalized;
                        if (Vector3.Dot(flashlight.transform.forward, directionToEnemy) > 0.5f && flashlight.GetComponent<Light>().enabled)
                        {
                            Vector3 retreatDirection = (transform.position - playerTransform.position).normalized;
                            Vector3 retreatPosition = transform.position + retreatDirection * retreatDistance;

                            currentState = EnemyState.Retreat;
                            navMeshAgent.SetDestination(retreatPosition);
                            transform.LookAt(playerTransform.position);
                        }
                        else
                        {
                            navMeshAgent.SetDestination(playerTransform.position);
                        }
                    }
                    else
                    {
                        // Player is outside the navmesh, stop following
                        currentState = EnemyState.Walk;
                        SetRandomIdleDestination();
                    }
                }
                break;

            case EnemyState.Retreat:
                if (distanceToPlayer > followDistance)
                {
                    currentState = EnemyState.Walk;
                    SetRandomIdleDestination();
                }
                break;

            case EnemyState.Attack:
                if (!isAttacking)
                {
                    AttackPlayer();
                }
                else
                {
                    attackTimer += Time.deltaTime;
                    if (attackTimer >= attackDuration)
                    {
                        currentState = EnemyState.Idle;
                        StayIdle();
                        isAttacking = false;
                        attackTimer = 0f;
                    }
                }
                break;

            case EnemyState.Idle:
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleDuration)
                {
                    idleTimer = 0f;
                    currentState = EnemyState.Walk;
                    SetRandomIdleDestination();
                }
                break;
        }
    }

    void AttackPlayer()
    {
        isAttacking = true;
        EnemyAnimator.runtimeAnimatorController = AttackAnimation;
    }

    void StayIdle()
    {
        EnemyAnimator.runtimeAnimatorController = IdleAnimation;
    }

    void SetRandomIdleDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkingRadius;
        randomDirection += initialPosition;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkingRadius, 1);
        idleDestination = hit.position;
        navMeshAgent.SetDestination(idleDestination);
    }
}
