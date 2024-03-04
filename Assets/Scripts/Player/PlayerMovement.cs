using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Animator animator;

    [SerializeField] private float _moveSpeed;



    private void Update() //Fixedupdate
    {
        rb.velocity = new Vector3(joystick.Horizontal * _moveSpeed, rb.velocity.y, joystick.Vertical * _moveSpeed);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            animator.Play("RunFWD_AR_Anim");

        }
        else
        {
            animator.Play("IdleBattle01_AR_Anim");
        }
    }
}
