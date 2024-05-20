using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy : MonoBehaviour
{
    public int HP;
    public Animator animator;
    public void TakeDame(int damageAmount)
    {
        HP-= damageAmount;
        if(HP<=0)
        {
            animator.SetTrigger("die");
            //Removed collision component when enermy die
            GetComponent<CapsuleCollider>().enabled = false;  
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }    
}
