using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using AngleMessage = RosMessageTypes.UnitySailor.SetUnitySailAngleMsg;



public class RigController : MonoBehaviour
{
    ROSConnection ros;
    
    public GameObject mast;
    public GameObject rig;

    public float currentRot = 0;
    private float goalRot = 0;
    private int rotDir;
    private float rotSpeed = 50;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<AngleMessage>("set_sail_angle", ROSrotateSail);
    }

    private void Update() {
        // Handle Rotation by A and D keys
        if (Input.GetKey("a")) KeyboardRotateSail(1, 1);
        if (Input.GetKey("d")) KeyboardRotateSail(1, -1);
    }

    private void FixedUpdate() {
        // Sail cannot rotate past 90 degrees in either direction
        if (currentRot < -90) currentRot = -90;
        if (currentRot > 90) currentRot = 90;
        if (goalRot < -90) goalRot = -90;
        if (goalRot > 90) goalRot = 90;

        // Rotation Loop
        if (rotDir != 0) {
            if (rotDir == 1 && currentRot < goalRot) {
                rig.transform.RotateAround(mast.transform.position, mast.transform.up, rotSpeed * Time.deltaTime);
                currentRot = currentRot + rotSpeed * Time.deltaTime;
            } else if (rotDir == -1 && currentRot > goalRot) {
                rig.transform.RotateAround(mast.transform.position, mast.transform.up, -rotSpeed * Time.deltaTime);
                currentRot = currentRot - rotSpeed * Time.deltaTime;
            } else {
                rotDir = 0;
            }
        }
    }

    void KeyboardRotateSail(float deltaAngle, int newRotDir) {
        rotDir = newRotDir;
        goalRot = currentRot + deltaAngle * rotDir;
    }

    void ROSrotateSail(AngleMessage msg) 
    {
        goalRot = msg.sail_angle;
        rotDir = goalRot < currentRot ? -1 : 1;
    }
}