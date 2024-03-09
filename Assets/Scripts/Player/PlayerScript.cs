using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Animator animator;
    private Rigidbody rb;
    private string currentState;

    private float groundCheckDistance;

    private float speedWalk = 5f;
    private float jumpForce = 0;
    public bool isGrounded;

    private bool isShootPressed;
    private bool isShooting;


    [SerializeField] private float attackDelay = 0.3f;

    //ANIMATION STATE
    const string PLAYER_IDLE = "IdleBattle01_AR_Anim";
    const string PLAYER_JUMP = "Jump_AR_Anim";
    


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }
    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        // GROUND CHECK
        groundCheckDistance = GetComponent<CapsuleCollider>().height / 2f;

        RaycastHit hit;
      //  Debug.DrawRay(transform.position, -transform.up * groundCheckDistance, Color.red);
        if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);
        currentState = newState;
    }    

}
