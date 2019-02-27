using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    //Serializing the field allows you to keep the variable private, whilst being able to access it within the unity editor like a public variable.
    //Initialise Serialized variables
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerBody;
    [SerializeField] private KeyCode slowTimeKey;
    //Initialise private variables
    private float xAxisClamp;


    // Start is called before the first frame update
    void Start()
    {
        xAxisClamp = 0;
        LockCursor();
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotation();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CameraRotation() 
    {


        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY;

        if(xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

            transform.Rotate(Vector3.left * mouseY);
            playerBody.Rotate(Vector3.up * mouseX);
    }


    // had to code this in to fix a bug with the camera sometimes escaping the constraints.
    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;

    }
}
