using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CannonRotate : MonoBehaviour
{
    [SerializeField] private float mouseXSens;
    [SerializeField] private float mouseYSens;
    [SerializeField] private float maxZAngle;
    [SerializeField] private float maxYAngle;
    [SerializeField] private Transform rotateYAxis;
    [SerializeField] private Transform rotateZAxis;

    private float rotationY;
    private float rotationZ;

    private void Update()
    {
        MyInput();
        CannonRotation();
    }

    private void CannonRotation()
    {
        //rotation cannon in Y AXIS
        rotateYAxis.localRotation = Quaternion.Euler(0, rotationY, 0);
        //rotation cannon barrel in Z AXIS
        rotateZAxis.localRotation = Quaternion.Euler(0, 0, rotationZ);
    }
    private void MyInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * (mouseXSens * 10) * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * (mouseYSens * 10) * Time.deltaTime;

        rotationY += mouseX;
        rotationY = Mathf.Clamp(rotationY, -maxYAngle, maxYAngle);

        rotationZ += mouseY;
        rotationZ = Mathf.Clamp(rotationZ, 0, maxZAngle);
    }
}
