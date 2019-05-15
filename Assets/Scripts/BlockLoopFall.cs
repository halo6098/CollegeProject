using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLoopFall : MonoBehaviour
{
    [SerializeField] private GameObject block;

    void Update()
    {
        if(block.transform.position.y < -1f)
        {
            block.transform.position = new Vector3(-1, 10, 15.9f);
        }
    }
}
