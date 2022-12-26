using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnitySailor;
using AngleMessage = RosMessageTypes.UnitySailor.SetUnityRudderAngleMsg;


public class RudderController : MonoBehaviour
{
    ROSConnection ros;

    public GameObject rudderVizualizer;
    private Rigidbody rudderRB;

    public float currentRot = 0;
    private float goalRot = 0;
    private int rotDir;
    private float rotSpeed = 50;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<AngleMessage>("set_rudder_angle", ROSrotateRudder);

        rudderRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Handle Rotation by arrow keys
        if (Input.GetKey("left")) KeyboardRotateRudder(1, -1);
        if (Input.GetKey("right")) KeyboardRotateRudder(1, 1);
    }

    void FixedUpdate() {
        // Rudder cannot rotate past 90 degrees in either direction
        if (currentRot < -90) currentRot = -90;
        if (currentRot > 90) currentRot = 90;
        if (goalRot < -90) goalRot = -90;
        if (goalRot > 90) goalRot = 90;

        // Rotation Loop
        if (rotDir != 0) {
            if (rotDir == 1 && currentRot < goalRot) {
                currentRot += rotSpeed * Time.deltaTime;
            } else if (rotDir == -1 && currentRot > goalRot) {
                currentRot -= rotSpeed * Time.deltaTime;
            } else {
                rotDir = 0;
            }
        }

        // Rotate visible Rudder
        rudderVizualizer.transform.localRotation = Quaternion.Euler(0, 0, -currentRot);

        // Apply Rudder forces
        ApplyRudderForce();
    }

    void KeyboardRotateRudder(float deltaAngle, int newRotDir) {
        rotDir = newRotDir;
        goalRot = currentRot + deltaAngle * rotDir;
    }

    void ROSrotateRudder(AngleMessage msg) 
    {
        goalRot = msg.rudder_angle;
        rotDir = goalRot < currentRot ? -1 : 1;
    }

    void ApplyRudderForce() {
        // get absolute value of boat speed
        float boatSpeed = Mathf.Abs(transform.InverseTransformDirection(rudderRB.velocity).x);

        // Rudder force is dependent on boat speed and rudder rotation
        float rudderForce = currentRot * boatSpeed * 2f;

        // Apply force to Rudder
        rudderRB.AddForce(transform.up * rudderForce);
    }
}
