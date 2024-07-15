
using UnityEngine;
using UnityEngine.AI;

public class chaseState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;
    float timeChase;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 5f;
        timeChase = 0;
        animator.transform.LookAt(player);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        float distance = Vector3.Distance(player.position, animator.transform.position);
    //    Debug.Log("distance = " + distance);

        //distance between player and enermy is
        if (distance > 12 && timeChase > 10f)
        {
            animator.SetBool("isChasing", false);
   //         Debug.Log("False ");
        }
        else
        {
            animator.SetBool("isChasing", true);
            timeChase += Time.deltaTime;
        //    Debug.Log("time = " + timeChase);
        }

        if (distance < 2.5)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
    }

}
