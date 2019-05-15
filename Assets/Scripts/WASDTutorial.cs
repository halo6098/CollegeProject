using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDTutorial : MonoBehaviour
{
    [SerializeField] private Animator WASDtutorialAnimator;

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || (Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.S))))))
        {
            WASDtutorialAnimator.SetTrigger("Moved");
        }
    }
}
