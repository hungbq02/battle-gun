using UnityEngine;
using UnityEngine.AI;

public class powerUpStateBoss : StateMachineBehaviour
{
    float immuneTime;
    Collider collider;
    NavMeshAgent agent;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Enter Power");
        immuneTime = 5f;
        collider = animator.GetComponent<Collider>();
        agent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // Debug.Log(" time IMMUNE = "+ immuneTime);
        immuneTime -= Time.deltaTime;
        if (immuneTime < 0.1)
        {
            collider.enabled = true;
            agent.speed = 10;
            animator.GetComponent<CollisionEnermyAttackHandler>().damage = 200;
            animator.SetBool("isImmune", false);
        }
        else
        {
            collider.enabled = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isImmune", false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
