using UnityEngine;
using UnityEngine.AI;

public class powerUpStateBoss : StateMachineBehaviour
{
    float immuneTime;
    int dameBoss;
    Collider collider;
    NavMeshAgent agent;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Enter Power");
        immuneTime = 5f;
        collider = animator.GetComponent<Collider>();
        agent = animator.GetComponent<NavMeshAgent>();
        dameBoss = animator.GetComponent<CollisionEnermyAttackHandler>().damage;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        immuneTime -= Time.deltaTime;
        if (immuneTime > 0)
        {
           // Debug.Log("IMMUNE");
            collider.enabled = false;
        }
        else
        {
            //Debug.Log("Complete Power");

            collider.enabled = true;
            agent.speed = 10;
            dameBoss = 2 * dameBoss;
            animator.SetBool("isImmune", false);
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
