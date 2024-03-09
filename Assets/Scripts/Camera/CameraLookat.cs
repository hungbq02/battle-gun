using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookat : MonoBehaviour
{
    public float yAxis;
    public float xAxis;
    public float RotationSensitivity = 8f;

    public Transform target;
    private void Start()
    {
        //Tắt con trỏ
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        yAxis += Input.GetAxis("Mouse X") * RotationSensitivity;
        yAxis -= Input.GetAxis("Mouse Y") * RotationSensitivity;

        Vector3 targetRotation = new Vector3(xAxis, yAxis);
        transform.eulerAngles = targetRotation;
        transform.position = target.position - transform.forward * 3f;

    }
}
