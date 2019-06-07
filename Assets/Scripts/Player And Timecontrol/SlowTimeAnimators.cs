using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTimeAnimators : MonoBehaviour
{
    [SerializeField] private Animator crusherAnimator;


    // Update is called once per frame
    void Update()
    {
        if (PlayerController.slowTime)
        {
            crusherAnimator.speed = 0.2f;
        }
        else
        {
            crusherAnimator.speed = 1f;
        }
    }
}
