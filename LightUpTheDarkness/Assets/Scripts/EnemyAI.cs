using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Idle, Follow, Retreat }
    public float followDistance = 10f; // Adjust this distance as needed
    public float retreatDistance = 5f; // Distance at which the enemy starts retreating
    public float retreatSpeed = 2f; // Speed at which the enemy retreats
    public float idleRadius = 5f; // Radius within which the enemy wanders in idle state

    private EnemyState currentState = EnemyState.Idle;
    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    private Vector3 initialPosition;
    private Vector3 idleDestination;

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
            case EnemyState.Idle:
                // Check if the player is within the follow distance
                if (distanceToPlayer <= followDistance)
                {
                    currentState = EnemyState.Follow;
                    break;
                }

                // Check if the enemy reached its idle destination
                if (Vector3.Distance(transform.position, idleDestination) < navMeshAgent.stoppingDistance)
                {
                    SetRandomIdleDestination();
                }
                break;

            case EnemyState.Follow:
                // Check if the player is facing the enemy (using dot product)
                Vector3 directionToEnemy = (transform.position - playerTransform.position).normalized;
                float dotProduct = Vector3.Dot(playerTransform.forward, directionToEnemy);

                // If the player is facing the enemy, make the enemy walk back
                if (dotProduct > 0.5f)
                {
                    // Calculate direction away from player
                    Vector3 retreatDirection = (transform.position - playerTransform.position).normalized;
                    Vector3 retreatPosition = transform.position + retreatDirection * retreatDistance;

                    currentState = EnemyState.Retreat;
                    navMeshAgent.SetDestination(retreatPosition);
                    transform.LookAt(playerTransform.position);
                    break;
                }

                // Calculate path to the player and set it as the destination
                navMeshAgent.SetDestination(playerTransform.position);
                break;

            case EnemyState.Retreat:
                // Check if the player is within the follow distance
                if (distanceToPlayer > followDistance)
                {
                    currentState = EnemyState.Idle;
                    SetRandomIdleDestination();
                }
                break;
        }
    }

    // Set a random destination within idleRadius
    void SetRandomIdleDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * idleRadius;
        randomDirection += initialPosition;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, idleRadius, 1);
        idleDestination = hit.position;
        navMeshAgent.SetDestination(idleDestination);

        //if I need the enemy to stay in it's area I need to comment this line
        initialPosition = idleDestination; // Update initialPosition
    }
}