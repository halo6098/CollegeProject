using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public Transform interactionTransform;
    [SerializeField] GameObject Doors = null;
    [SerializeField] private Animator tutorialAnimator;


    private Animator animator;
    private bool tutorialComplete = false;
    void Awake()
    {
        interactionTransform = GetComponent<Transform>();
        if (Doors != null)
        {
            animator = Doors.GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (animator != null)
        {
            if (PlayerController.slowTime)
            {
                animator.speed = 0.2f;
                if (tutorialAnimator != null)
                {
                    tutorialAnimator.SetTrigger("MousePressed");
                }

            }
            else
            {
                animator.speed = 0.95f;
            }
        }
        if (this.name == "Button1" || this.name == "Button2")
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("DoorIdle"))
            {
                // do something
                animator.Play("DoorClose");

            }
        }

        if (animator.GetBool("OpenElevator"))
        {
                animator.SetBool("OpenElevator", !animator.GetBool("OpenElevator"));
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, PlayerController.interactMinDistance);
    }

    public virtual void Interact()
    {
        Debug.Log("Object has been interacted with.");

        if (this.name == "Button1" || this.name == "Button2")
        {
            animator.Play("DoorOpen");
            if (tutorialComplete == false)
            {
                if (tutorialAnimator != null)
                {
                    tutorialAnimator.SetTrigger("ButtonPress");
                }
                tutorialComplete = true;
            }

        }

        if (this.name == "Button3")
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("DoorOpen"))
            {
                animator.Play("DoorClose");
            }
            else
            {
                animator.Play("DoorOpen");
            }

        }

    }
}
