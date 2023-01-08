using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnitySailor;
using UnityBoatSensors = RosMessageTypes.UnitySailor.UnityBoatSensorsMsg;
using UnityBoatPosRot = RosMessageTypes.UnitySailor.UnityBoatPosRotMsg;

public class RosPublisherController : MonoBehaviour
{
    ROSConnection ros;

    public GameObject rudder;
    public GameObject rig;
    public GameObject boat;
    public GameObject wind;

    public Text dispAWS;
    public Text dispAWA;
    public Text dispSpeed;

    private float forwardSpeed;
    private float apparentWindDir;
    private float apparentWindSpeed;

    private float trueWindDir;
    private float trueWindSpeed;

    private float publishMessageFrequency = 1f;
    private float timeElapsed = 0;


    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<UnityBoatSensors>("boat_sensors");
        ros.RegisterPublisher<UnityBoatPosRot>("boat_posrot");
    }

    void FixedUpdate()
    {
        UpdateValues();

        timeElapsed += Time.deltaTime;
        if (timeElapsed > publishMessageFrequency)
        {
            timeElapsed = 0;
            PublishBoatSensors();
            PublishBoatPosition();
        }
    }

    void UpdateValues() {
        forwardSpeed = boat.GetComponent<SailboatController>().forwardSpeed;

        trueWindDir = wind.GetComponent<WindZone>().transform.rotation.eulerAngles.y;
        trueWindSpeed = wind.GetComponent<WindZone>().windMain;

        // headwind is the negative of the boat's velocity
        var headWindVector = -boat.GetComponent<SailboatController>().boatRB.velocity;

        // true wind vector is the windMain in the forward direction rotated by the windZone's rotation
        var trueWindVector = wind.GetComponent<WindZone>().transform.rotation * Vector3.forward * wind.GetComponent<WindZone>().windMain;

        // apparent wind vector is the sum of the headwind and truewind vectors
        var apparentWindVector = headWindVector + trueWindVector;

        apparentWindDir = Vector3.Angle(apparentWindVector, boat.transform.right);
        apparentWindSpeed = apparentWindVector.magnitude;

        dispAWA.text = "<size=18>AWA</size>\n<b>" + apparentWindDir.ToString("0.0") + "</b><size=12>deg</size>";
        dispAWS.text = "<size=18>AWS</size>\n<b>" + apparentWindSpeed.ToString("0.0") + "</b><size=12>kn</size>";
        dispSpeed.text = "<size=18>Speed</size>\n<b>" + forwardSpeed.ToString("0.0") + "</b><size=12>kn</size>";
    }

    void PublishBoatSensors()
    {
        float sailAngle = rig.GetComponent<RigController>().currentRot;
        float rudderAngle = rudder.GetComponent<RudderController>().currentRot;
        float boatForwardSpeed = boat.GetComponent<SailboatController>().forwardSpeed;

        float absWindAngle = wind.GetComponent<WindZone>().transform.rotation.eulerAngles.y;
        float relWindAngle = absWindAngle - boat.transform.rotation.eulerAngles.y;

        float absWindSpeed = wind.GetComponent<WindZone>().windMain;
        float relWindSpeed = absWindSpeed * Mathf.Cos(relWindAngle * Mathf.Deg2Rad);


        var msg = new UnityBoatSensors
        {
            boat_sail_angle = sailAngle,
            boat_rudder_angle = rudderAngle,
            boat_forward_speed = boatForwardSpeed,
            apparent_wind_dir = apparentWindDir,
            apparent_wind_speed = apparentWindSpeed,
        };

        ros.Publish("boat_sensors", msg);
    }

    void PublishBoatPosition()
    {
        var msg = new UnityBoatPosRot
        {
            pos_x = transform.position.x,
            pos_y = transform.position.y,
            pos_z = transform.position.z,
            rot_x = transform.rotation.x,
            rot_y = transform.rotation.y,
            rot_z = transform.rotation.z,
            rot_w = transform.rotation.w
        };

        ros.Publish("boat_posrot", msg);
    }
}
