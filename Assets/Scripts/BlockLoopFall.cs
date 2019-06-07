using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLoopFall : MonoBehaviour
{
    [SerializeField] private GameObject block;
    private Vector3 originalPos;
    void Start()
    {
        originalPos = gameObject.transform.position;
    }
    void Update()
    {
        if(block.transform.position.y < -1f)
        {
            block.transform.position = originalPos;
        }
    }
}
