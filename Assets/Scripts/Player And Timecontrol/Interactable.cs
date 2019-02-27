using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
   
    public Transform interactionTransform;
    
    void Awake()
    {
        interactionTransform = GetComponent<Transform>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, PlayerController.interactMinDistance);
    }

    public virtual void Interact()
    {
        Debug.Log("Object has been interacted with.");
        gameObject.SetActive(false);
    }

}
