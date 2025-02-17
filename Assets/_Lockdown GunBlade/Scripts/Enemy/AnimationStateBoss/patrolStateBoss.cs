using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class patrolStateBoss : StateMachineBehaviour
{
    float timer;
    List<Transform> wayPoints = new List<Transform>();
    NavMeshAgent agent;
    Transform player;
    float chaseRange = 18f;

    [SerializeField] protected PathMoving enemyPath;
    [SerializeField] protected Point currentPoint;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        timer = 0;
        //    GameObject obj= GameObject.FindGameObjectWithTag("WayPoints");
        player = GameObject.FindGameObjectWithTag("Player").transform;

        /*    foreach (Transform t in obj.transform)
            {
                wayPoints.Add(t);
            }

            agent.SetDestination(wayPoints[Random.Range(0, 3)].position);*/
        LoadEnemyPath();

        // Khởi tạo currentPoint từ enemyPath
        if (enemyPath != null)
        {
            currentPoint = enemyPath.GetPoint(Random.Range(0, enemyPath.transform.childCount));
            if (currentPoint != null)
            {
                agent.SetDestination(currentPoint.transform.position);
            }
            else
            {
                Debug.LogError("currentPoint is null, please check the points in enemyPath.");
            }
        }
        else
        {
            Debug.LogError("enemyPath is null, please check the PathManager instance.");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            if (currentPoint != null)
            {
                currentPoint = currentPoint.NextPoint;
                if (currentPoint != null)
                {
                    agent.SetDestination(currentPoint.transform.position);
                }
            }
        }

        if (timer > 6)
        {
            animator.SetBool("isPatrolling", false);
        }

        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < chaseRange)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }
    protected virtual void LoadEnemyPath()
    {
        if (this.enemyPath != null) return;
        this.enemyPath = PathManager.Instance.GetPath("path_1");
    }

}
